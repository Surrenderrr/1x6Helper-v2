using _1x6Helper.Models;
using _1x6Helper.Models.Api;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static _1x6Helper.Services.Utilities;

namespace _1x6Helper.ViewModels
{
    public partial class GeneralViewModel:ViewModelBase
    {
        private static readonly Bitmap noImage = new Bitmap(AssetLoader.Open(new Uri("avares://1x6Helper/Assets/Pictures/no image.png")));
        [ObservableProperty]
        private bool _isListEmpty = false;
        [ObservableProperty]
        private string _playerName = "No player";
        [ObservableProperty]
        private string _playerId;
        [ObservableProperty]
        private Bitmap _avatar = noImage;
        [ObservableProperty]
        private string _averagePlace;
        [ObservableProperty]
        private string _totalMatches;
        [ObservableProperty]
        private string _currentSeasonRating;
        [ObservableProperty]
        private string _currentSeasonDuoRating;
        [ObservableProperty]
        private string _previousSeasonRating;
        [ObservableProperty]
        private string _previousSeasonDuoRating;
        [ObservableProperty]
        private string _playerIdWatermark = "Enter Dota ID";
        [ObservableProperty]
        private string _decency;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotRunning))]
        [NotifyPropertyChangedFor(nameof(ButtonLabel))]
        [NotifyPropertyChangedFor(nameof(ButtonBrush))]
        private bool _isRunning = false;

        public bool IsNotRunning => !IsRunning;
        public string ButtonLabel => IsRunning ? "Cancel" : "Check";
        public string ButtonBrush => IsRunning ? "#c74949" : "#49a0c7";

        private CancellationTokenSource? _cts;
        public ObservableCollection<Dota1x6Match.Player> CurrentPlayerTeammates { get; } = new ObservableCollection<Dota1x6Match.Player>();

        [RelayCommand]
        private async Task Check()
        {
            Reset();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            IsRunning = true;
            try
            {
                
                SteamProfile? player = await GetSteamProfileAsync(PlayerId, token);
                PlayerStats? playerStats = await GetPlayerStats(PlayerId, token);

                Avatar = await GetAvatarAsync(player.Data.avatarfull, token);
                PlayerName = player.Data.personaname;
                TotalMatches = playerStats.Data.MatchCount.ToString();
                AveragePlace = (playerStats.Data.AvgPlace).ToString("F2");
                CurrentSeasonRating = playerStats.Data.Rating.ToString();
                CurrentSeasonDuoRating = playerStats.Data.DuoRating.ToString();
                var json = File.ReadAllText("imba_heroes.json");
                Dictionary<string,List<string>> imba_heroes = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json) ?? new();
                Decency = (await CountDecency(PlayerId, imba_heroes, token)).ToString("P0");
                CurrentPlayerTeammates.Clear();
                List<Dota1x6Match.Player> playersList = await GetPlayerTeammatesAsync(PlayerId, token);
                foreach(var element in playersList)
                {
                    if(element.PlayerIcon == null)
                        { 
                            element.PlayerIcon = await GetAvatarAsync((await GetSteamProfileAsync(element.Id.ToString(), token)).Data.avatarfull, token);
                        }
                    CurrentPlayerTeammates.Add(element);
                }
                token.ThrowIfCancellationRequested();
                PreviousSeasonDuoRating = await GetMmrBySeason(PlayerId, true, token);
                PreviousSeasonRating = await GetMmrBySeason(PlayerId, false, token);
            }
            catch (OperationCanceledException)
            {
                PlayerIdWatermark = "Enter Dota ID";
            }
            catch (Exception)
            {
                PlayerId = "";
                PlayerIdWatermark = "Error: Player not found";
            }
            finally
            {
                IsRunning = false;
                _cts.Dispose();
                _cts = null;
            }
        }
        [RelayCommand]
        private void Toggle()
        {
            if (IsRunning)
                _cts?.Cancel();
            else
                _ = Check();
        }
        private void Reset()
        {
            PlayerName = "No player";
            Avatar = noImage;
            AveragePlace = string.Empty;
            TotalMatches = string.Empty;
            CurrentSeasonRating = string.Empty;
            CurrentSeasonDuoRating = string.Empty;
            PreviousSeasonRating = string.Empty;
            PreviousSeasonDuoRating = string.Empty;
            Decency = string.Empty;
            ClearListAsync();
        }
        private async Task ClearListAsync()
        {
            IsListEmpty = true;
            await Task.Delay(500);
            CurrentPlayerTeammates.Clear();
            IsListEmpty = false; ;
        }

    }
}
