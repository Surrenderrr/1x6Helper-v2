using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace _1x6Helper.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly Lazy<GeneralViewModel> _generalViewModel = new();
        private readonly Lazy<MatchesWithViewModel> _matchesWithViewModel = new();
        private readonly Lazy<ImbaHeroesViewModel> _imbaHeroesViewModel = new();
        [ObservableProperty]
        private bool _IsPaneOpen = true;
        [ObservableProperty]
        private ViewModelBase _currentPage;
        public MainWindowViewModel()
        {
            // Устанавливаем начальную страницу при запуске приложения
            _currentPage = _generalViewModel.Value;
        }

        [RelayCommand]
        private void TogglePane()
        {
            IsPaneOpen = !IsPaneOpen;
        }
        [RelayCommand]
        private void OpenGeneral() => CurrentPage = _generalViewModel.Value;
        [RelayCommand]
        private void OpenMatchesWith() => CurrentPage = _matchesWithViewModel.Value;
        [RelayCommand]
        private void OpenImbaHeroes() => CurrentPage = _imbaHeroesViewModel.Value;
    }
}
