using _1x6Helper.Models.Api;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _1x6Helper.Services.Utilities;

namespace _1x6Helper.ViewModels
{
    public partial class HeroCardViewModel:ViewModelBase
{
        public Heroes.HeroInfo HeroInfo { get; }
        public List<AbilityViewModel> Abilities { get; } = new();
        public string DisplayName => HeroInfo.UserFriendlyName ?? HeroInfo.Name ?? "";

        [ObservableProperty]
        private Bitmap? _heroImage;

        public HeroCardViewModel(Heroes.HeroInfo info)
        {
            HeroInfo = info;
            if (info.Talents == null || info.Name == null) return;
            foreach (var talent in info.Talents)
                Abilities.Add(new AbilityViewModel(info.Name, talent));
        }

        public async Task LoadImagesAsync()
        {
            HeroImage = await GetHeroImage(HeroInfo.Name);
            await Task.WhenAll(Abilities.Select(a => a.LoadImageAsync()));
        }
    }
}
