using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using static _1x6Helper.Services.Utilities;

namespace _1x6Helper.ViewModels
{
    public partial class AbilityViewModel:ViewModelBase
{
        public string HeroKey { get; }
        public string TalentKey { get; }

        [ObservableProperty]
        private Bitmap? _abilityImage;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BorderColor))]
        [NotifyPropertyChangedFor(nameof(AbilityBackground))]
        [NotifyPropertyChangedFor(nameof(AbilityOpacity))]
        private bool _isSelected;

        public IBrush BorderColor => IsSelected
    ? new SolidColorBrush(Color.Parse("#00d4ff"))
    : new SolidColorBrush(Color.Parse("#1e4d6b"));

        public IBrush AbilityBackground => IsSelected
            ? new SolidColorBrush(Color.Parse("#003d52"))
            : new SolidColorBrush(Color.Parse("#0a1e2c"));
        public double AbilityOpacity => IsSelected ? 1.0 : 0.45;

        [RelayCommand]
        private void Toggle() => IsSelected = !IsSelected;

        public AbilityViewModel(string heroKey, string talentKey)
        {
            HeroKey = heroKey;
            TalentKey = talentKey;
        }

        public async Task LoadImageAsync()
        {
            AbilityImage = await GetHeroTalentImage(TalentKey);
        }
    }
}
