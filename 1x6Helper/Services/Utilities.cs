using _1x6Helper.Models.Api;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Media.TextFormatting.Unicode;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _1x6Helper.Services
{
    public static class Utilities
    {
        private static readonly HttpClient _httpclient = new();
        //Получение информации о профиле игрока 1x6
        public static async Task<PlayerStats?> GetPlayerStats(string dotaId, CancellationToken cancellationToken = default)
        {
            try
            {
                using HttpResponseMessage response = await _httpclient.GetAsync($"https://stats.dota1x6.com/api/v2/players/?playerId={dotaId}", cancellationToken);
                PlayerStats? data = await response.Content.ReadFromJsonAsync<PlayerStats>(cancellationToken);
                return data;
            }
            catch (Exception e) when (e is not OperationCanceledException) { return null; }
        }
        //Получаем информацию о стимпрофиле игрока 1x6
                public static async Task<SteamProfile?> GetSteamProfileAsync(string dotaId, CancellationToken cancellationToken = default)
        {
            try
            {
                using HttpResponseMessage response = await _httpclient.GetAsync($"https://stats.dota1x6.com/api/v2/players/steam-profile?playerId={dotaId}", cancellationToken);
                SteamProfile? data = await response.Content.ReadFromJsonAsync<SteamProfile>(cancellationToken);
                return data;
            }
            catch (Exception e) when (e is not OperationCanceledException) { return null; }
        }
        //Получаем BitMap-изображение по ссылке
        public static async Task<Bitmap> GetAvatarAsync(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                using HttpResponseMessage response = await _httpclient.GetAsync(url, cancellationToken);
                if(response == null) return new Bitmap(AssetLoader.Open(new Uri("avares://1x6Helper/Assets/Pictures/no image.png")));
                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                Bitmap Avatar = new(stream);
                return Avatar;            
                
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                return new Bitmap(AssetLoader.Open(new Uri("avares://1x6Helper/Assets/Pictures/no image.png")));
            }
        }
        //Получаем историю матчей
        public static async Task<History?> GetMatchHistoryAsync(string dotaId, int matchesCount, string page = "1", CancellationToken cancellationToken = default)
        {
            try
            {
                using HttpResponseMessage response = await _httpclient.GetAsync($"https://stats.dota1x6.com/api/v2/matches/history?playerId={dotaId}&page={page}&count={matchesCount}", cancellationToken);
                History? data = await response.Content.ReadFromJsonAsync<History>(cancellationToken);
                return data;
            }
            catch (Exception e) when (e is not OperationCanceledException) { return null; }
        }
        //Получаем полную информацию о матче
        public static async Task<Dota1x6Match?> GetMatchDataAsync(string MatchId, CancellationToken cancellationToken = default)
        {
            try
            {
                using HttpResponseMessage response = await _httpclient.GetAsync($"https://stats.dota1x6.com/api/v2/matches/?dotaMatchId={MatchId}", cancellationToken);
                Dota1x6Match? data = await response.Content.ReadFromJsonAsync<Dota1x6Match>(cancellationToken);
                return data;
            }
            catch (Exception e) when (e is not OperationCanceledException) { return null; }
        }
        //Получаем краткую информацию о матче
        public static async Task<Dota1x6ShortMatch?> GetShorMatchDataAsync(string MatchId, CancellationToken cancellationToken = default)
        {
            try
            {
                using HttpResponseMessage response = await _httpclient.GetAsync($"https://stats.dota1x6.com/api/v2/matches/short?dotaMatchId={MatchId}", cancellationToken);
                Dota1x6ShortMatch? data = await response.Content.ReadFromJsonAsync<Dota1x6ShortMatch>(cancellationToken);
                return data;
            }
            catch (Exception e) when (e is not OperationCanceledException) { return null; }
        }
        //Поиск тиммейтов за последние 20 матчей
        public static async Task<List<Dota1x6Match.Player>> GetPlayerTeammatesAsync(string dotaId, CancellationToken cancellationToken = default)

        {
            
            
                List<Dota1x6Match.Player> playerTeammates = new List<Dota1x6Match.Player>();
                History? playerMatches = await GetMatchHistoryAsync(dotaId, 20, "1", cancellationToken);
                if (playerMatches == null) { return playerTeammates; }
                foreach (var element in playerMatches.Data.Values)
                {
                    Dota1x6Match? matchInfo = await GetMatchDataAsync(element.MatchId.ToString(), cancellationToken);
                    if (matchInfo == null) continue;
                    else if (matchInfo.Data.Meta.IsDuo == false)
                    {
                        Dota1x6Match.Player Solo = new Dota1x6Match.Player();
                        Solo.PlayerIcon = new Bitmap(AssetLoader.Open(new Uri("avares://1x6Helper/Assets/Pictures/solo.png")));
                        Solo.Name = "Solo";
                        playerTeammates.Add(Solo);
                        continue;
                    }
                    var targetplayer = matchInfo.Data.Players!.FirstOrDefault(p => p.Id == long.Parse(dotaId));
                    playerTeammates.Add(matchInfo.Data.Players!.FirstOrDefault(p => p.TeamId == targetplayer.TeamId && p.Id != targetplayer.Id));

                }
                return playerTeammates;
            
            
        }
        //Заполнить список всеми MatchId игрока
        public static async Task<List<History.MatchInfo>> FillListWithMatches(string DotaId, CancellationToken cancellationToken = default)
        {
            List<History.MatchInfo> matchIds = new List<History.MatchInfo>();
            History? history = await GetMatchHistoryAsync(DotaId, 20, "1", cancellationToken);
            int total_pages = history.Data.TotalPages;
            int current_page = 1;
            while (current_page <= total_pages)
            {
                history = await GetMatchHistoryAsync(DotaId, 25, current_page.ToString(), cancellationToken);
                if (history == null) { break; }
                foreach (History.MatchInfo matchInfo in history.Data.Values)
                {
                    matchIds.Add(matchInfo);
                }
                current_page++;

            }
            return matchIds;
        }
        //Находит общие матчи с конкретным игроком
        public static async Task<List<History.MatchInfo>> FindMatchesWith(string yourDotaId, string targetDotaId, CancellationToken cancellationToken = default)
        {
            List<History.MatchInfo> yourMatchIds = await FillListWithMatches(yourDotaId, cancellationToken);
            List<History.MatchInfo> targetMatchIds = await FillListWithMatches(targetDotaId, cancellationToken);
            List<string> matches = new List<string>();
            List<History.MatchInfo> matchInfo = new List<History.MatchInfo>();
            var ids = new HashSet<long>(targetMatchIds.Select(x => x.MatchId));

            var common = yourMatchIds
                .Where(x => ids.Contains(x.MatchId))
                .ToList();
            foreach(var element in common)
            {
                matches.Add(element.MatchId.ToString());
                matchInfo.Add(element);
            }
           return matchInfo;

        }
        //Подгружаем героев и их таланты в dictionary, где ключ - имя героя, значение - массив его талантов
        public static async Task<Dictionary<string, List<string>>> GetHeroesDictionaryAsync(CancellationToken cancellationToken = default)
        {

            try
            {
                Dictionary<string, List<string>> heroesDictionary = new Dictionary<string, List<string>>();
                using HttpResponseMessage response = await _httpclient.GetAsync("https://stats.dota1x6.com/api/v2/heroes/", cancellationToken);
                Heroes data = await response.Content.ReadFromJsonAsync<Heroes>(cancellationToken);
                foreach (var element in data.Data.Heroes)
                {
                    heroesDictionary.Add(element.Name, element.Talents);
                }
                return heroesDictionary;
            }
            catch (Exception e) when (e is not OperationCanceledException) { return null; }
        }
        //Получаем список всех героев
        public static async Task<Heroes> GetHeroesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using HttpResponseMessage response = await _httpclient.GetAsync($"https://stats.dota1x6.com/api/v2/heroes/", cancellationToken);
                Heroes? data = await response.Content.ReadFromJsonAsync<Heroes>(cancellationToken);
                return data;
            }
            catch (Exception e) when (e is not OperationCanceledException) { return null; }
        }
        //Получаем картинку персонажа
        public static async Task<Bitmap> GetHeroImage(string heroName,CancellationToken cancellationToken = default)
        {
            try
            {
                using HttpResponseMessage response = await _httpclient.GetAsync($"https://cdn.dota1x6.com/v2/images/heroes/{heroName}/80x47.png",cancellationToken);
                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                Bitmap Avatar = new(stream);
                return Avatar;

                
            }
            catch (Exception e) when (e is not OperationCanceledException) { return new Bitmap(AssetLoader.Open(new Uri("avares://1x6Helper/Assets/Pictures/no image.png"))); }
        }
        //Получаем картинку таланта
        public static async Task<Bitmap> GetHeroTalentImage(string talentName, CancellationToken cancellationToken = default)
        {
            try
            {
                using HttpResponseMessage response = await _httpclient.GetAsync($"https://cdn.dota1x6.com/v2/images/talents/{talentName.ToLower()}/47x47.png", cancellationToken);
                var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                Bitmap Avatar = new(stream);
                return Avatar;
            }
            catch (Exception e) when (e is not OperationCanceledException) { return new Bitmap(AssetLoader.Open(new Uri("avares://1x6Helper/Assets/Pictures/no image.png"))); }
        }
        //Порядочность(decency)
        public static async Task<float> CountDecency(string playerId, Dictionary<string,List<string>> imbaHeroes, CancellationToken cancellationToken = default)
        {
            float poryad = 0;
            int matches_count = 25;
            History? history = await GetMatchHistoryAsync(playerId, 25 ,"1", cancellationToken);
            foreach (var element in history.Data.Values)
            {
                if(element.HeroName == null || element.Talent == null || element.IsFinished == false) { matches_count--; continue; }
                if (imbaHeroes.ContainsKey(element.HeroName) && imbaHeroes[element.HeroName].Contains(element.Talent))
                {
                    poryad++;
                }
            }

            return poryad / matches_count;
        }
        public static async Task<string> GetMmrBySeason(string dotaId, bool IsDuo, CancellationToken cancellationToken = default)
        {
            DateTime MinDate = new DateTime(2026, 2, 1);
            int current_page = 1;
            try
            {
                History? history = await GetMatchHistoryAsync(dotaId,20, current_page.ToString(),cancellationToken);
                if (history == null) return "-";
                int total_pages = history.Data.TotalCount;
                while( current_page <= total_pages )
                {
                    history = await GetMatchHistoryAsync(dotaId, 20, current_page.ToString(), cancellationToken);
                    foreach (var element in history.Data.Values)
                    {
                        
                        if (element.CreatedAtUtc < MinDate)
                        {
                            Dota1x6Match match = await GetMatchDataAsync(element.MatchId.ToString(), cancellationToken);
                            if (match.Data.Meta.Season == "Season 6" && match.Data.Meta.IsDuo == IsDuo)
                            {
                                var player = match.Data.Players.FirstOrDefault(p => p.Id == long.Parse(dotaId));
                                int? rating = player.RatingStart + player.RatingChange;
                                if (rating < 0) return "0";
                                else if (rating == null) return "-";
                                else { return (player.RatingStart + player.RatingChange).ToString(); }
                            }
                        }
                        
                    }
                    current_page++;
                }
               
                
            }
            catch (Exception e) when (e is not OperationCanceledException) { return "error"; }
            return "-";
            
        }
    }
}
