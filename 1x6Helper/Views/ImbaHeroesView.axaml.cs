using _1x6Helper.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Threading.Tasks;

namespace _1x6Helper.Views;

public partial class ImbaHeroesView : UserControl
{
    private ImbaHeroesViewModel? _vm;
    public ImbaHeroesView()
    {
        InitializeComponent();
        _vm = new ImbaHeroesViewModel();
        DataContext = _vm;
        Loaded += async (_, _) => await OnLoaded();

        _vm.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(ImbaHeroesViewModel.ShowSelectedOnly))
                UpdateStatus();
        };
        this.SizeChanged += (_, e) =>
        {
            HeroesScrollViewer.Height = e.NewSize.Height - 90;
        };
    }
    private async Task OnLoaded()
    {
        StatusText.Text = "Loading heroes...";
        await _vm!.LoadAsync();
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        int total = _vm!.StrengthHeroes.Count
                  + _vm.AgilityHeroes.Count
                  + _vm.IntellectHeroes.Count
                  + _vm.AllAtributeHeroes.Count;

        StatusText.Text = _vm.ShowSelectedOnly
            ? $"{total} heroes selected"
            : $"{total} heroes loaded";
    }

}