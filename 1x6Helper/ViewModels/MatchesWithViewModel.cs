using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static _1x6Helper.Services.Utilities;
using _1x6Helper.Models;
using _1x6Helper.Models.Api;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace _1x6Helper.ViewModels
{

    public partial class MatchesWithViewModel:ViewModelBase

        
{
        [ObservableProperty]
        private Dota1x6Match? _currentMatch;
        [ObservableProperty]
        private bool _isStatsVisible = true;
        [ObservableProperty]
        private History.MatchInfo _selectedMatchId;
        
        public ObservableCollection<Dota1x6Match.Player> CurrentMatchPlayers { get; } = new ObservableCollection<Dota1x6Match.Player>();
        [ObservableProperty]
        private string _yourDotaId;
        [ObservableProperty]
        private string _targetDotaId;
        public ObservableCollection<History.MatchInfo> MatchesWith { get; } = new ObservableCollection<History.MatchInfo>();
        [RelayCommand]
        private async Task Check()
        {
            MatchesWith.Clear();
            List<History.MatchInfo> matches = await FindMatchesWith(YourDotaId, TargetDotaId);
            int i = 1;
            foreach(var element in matches)
            {
                element.Number = i++;
                MatchesWith.Add(element);
            }
            
        }
        partial void OnSelectedMatchIdChanged(History.MatchInfo? value)
        {
            if (value == null) return;
            IsStatsVisible = false;
            LoadMatchDetailsAsync(value.MatchId.ToString());
        }
        public async Task LoadMatchDetailsAsync(string matchId)
        {
            CurrentMatchPlayers.Clear();
            Dota1x6Match? match = await GetMatchDataAsync(matchId);
            foreach(var element in match.Data.Players)
            {
                string heroUrl = $"https://cdn.dota1x6.com/v2/images/heroes/{element.HeroName}/80x47.png";
                string talentUrl = $"https://cdn.dota1x6.com/v2/images/talents/{element.Talents.Main?.ToLower()}/47x47.png";
                element.HeroIcon = await GetAvatarAsync(heroUrl);
                element.TalentIcon = await GetAvatarAsync(talentUrl);
                CurrentMatchPlayers.Add(element);
            }
            IsStatsVisible = true;
        }
    }

}
