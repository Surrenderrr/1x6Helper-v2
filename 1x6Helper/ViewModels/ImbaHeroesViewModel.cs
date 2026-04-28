using _1x6Helper.Models.Api;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace _1x6Helper.ViewModels
{
    public partial class ImbaHeroesViewModel : ViewModelBase
    {
        private const string SavePath = "imba_heroes.json";
        private readonly List<HeroCardViewModel> _allHeroes = new();
        [ObservableProperty]
        private bool _showSelectedOnly = false;
        partial void OnShowSelectedOnlyChanged(bool value)
        {
            if (value)
                ApplySelectedFilter();
            else
                RestoreAllHeroes();
        }
        public ObservableCollection<HeroCardViewModel> StrengthHeroes { get; set; } = new();
        public ObservableCollection<HeroCardViewModel> AgilityHeroes { get; set; } = new();
        public ObservableCollection<HeroCardViewModel> IntellectHeroes { get; set; } = new();
        public ObservableCollection<HeroCardViewModel> AllAtributeHeroes { get; set; } = new();

        private void ApplySelectedFilter()
        {
            if (!ShowSelectedOnly) return;

            var collections = new[]
            {
        StrengthHeroes, AgilityHeroes,
        IntellectHeroes, AllAtributeHeroes
    };

            foreach (var col in collections)
            {
                var toRemove = col
                    .Where(h => h.Abilities.All(a => !a.IsSelected))
                    .ToList();
                foreach (var h in toRemove)
                    col.Remove(h);
            }
        }
        private void RestoreAllHeroes()
        {
            StrengthHeroes.Clear();
            AgilityHeroes.Clear();
            IntellectHeroes.Clear();
            AllAtributeHeroes.Clear();

            foreach (var hero in _allHeroes)
            {
                switch (hero.HeroInfo.Attribute)
                {
                    case "Strength": StrengthHeroes.Add(hero); break;
                    case "Agility": AgilityHeroes.Add(hero); break;
                    case "Intellect": IntellectHeroes.Add(hero); break;
                    case "All": AllAtributeHeroes.Add(hero); break;
                }
            }
        }

        [RelayCommand]
        private void Save()
        {
            var dict = StrengthHeroes
        .Concat(AgilityHeroes)
        .Concat(IntellectHeroes)
        .Concat(AllAtributeHeroes)
        .Where(h => h.Abilities.Any(a => a.IsSelected))
        .ToDictionary(
            h => h.HeroInfo.Name!,
            h => h.Abilities
                  .Where(a => a.IsSelected)
                  .Select(a => a.TalentKey)
                  .ToList()
        );

            var json = JsonSerializer.Serialize(dict, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SavePath, json);
        }
        [RelayCommand]
        private void Reset()
        {
            foreach (var hero in StrengthHeroes
        .Concat(AgilityHeroes)
        .Concat(IntellectHeroes)
        .Concat(AllAtributeHeroes))
            {
                foreach (var ab in hero.Abilities)
                    ab.IsSelected = false;
            }
        }
        public async Task LoadAsync()
        {
            using var http = new HttpClient();
            var json = await http.GetStringAsync("https://stats.dota1x6.com/api/v2/heroes/");
            var heroes = JsonSerializer.Deserialize<Heroes>(json);
            if (heroes?.Data?.Heroes == null) return;

            Dictionary<string, List<string>> saved = LoadSaved();

            foreach (var hero in heroes.Data.Heroes)
            {
                if (hero.Name == null || hero.Talents == null) continue;
                var card = new HeroCardViewModel(hero);

                if (saved.TryGetValue(hero.Name, out var selectedTalents))
                    foreach (var ab in card.Abilities)
                        if (selectedTalents.Contains(ab.TalentKey))
                            ab.IsSelected = true;

                _allHeroes.Add(card);
                switch (hero.Attribute)
                {
                    case "Strength": StrengthHeroes.Add(card); break;
                    case "Agility": AgilityHeroes.Add(card); break;
                    case "Intellect": IntellectHeroes.Add(card); break;
                    case "All": AllAtributeHeroes.Add(card); break;
                }
            }

            var all = StrengthHeroes
                .Concat(AgilityHeroes)
                .Concat(IntellectHeroes)
                .Concat(AllAtributeHeroes);

            await Task.WhenAll(all.Select(h => h.LoadImagesAsync()));
        }
        private Dictionary<string, List<string>> LoadSaved()
        {
            if (!File.Exists(SavePath))
            {
                File.WriteAllText(SavePath, "{}");
                return new();
            }
            ;
            try
            {
                var json = File.ReadAllText(SavePath);
                return JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json) ?? new();
            }
            catch { return new(); }
        }
    }

}
