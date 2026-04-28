using _1x6Helper.Models.Api;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using static _1x6Helper.Models.Api.Dota1x6Match;

namespace _1x6Helper.Views;

public partial class GeneralView : UserControl
{
    public GeneralView()
    {
        InitializeComponent();
    }
    private async void OnImagePointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(sender as Control);
        if (point.Properties.IsLeftButtonPressed)
        {
            if(sender is Control control && control.DataContext is Dota1x6Match.Player player)
            {
                var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
                if (clipboard != null)
                {
                    await clipboard.SetTextAsync(player.Id.ToString());
                }
            }
            
        }

    }
}