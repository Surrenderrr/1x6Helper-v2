using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
//https://stats.dota1x6.com/api/v2/heroes/
namespace _1x6Helper.Models.Api;

public class Heroes
{
        [JsonPropertyName("data")]
        public HeroesData? Data { get; set; }

        [JsonPropertyName("errors")]
        public List<object>? Errors { get; set; }

        [JsonPropertyName("hasErrors")]
        public bool HasErrors { get; set; }
    

    public class HeroesData
    {
        [JsonPropertyName("heroes")]
        public List<HeroInfo>? Heroes { get; set; }

        [JsonPropertyName("lastAddedHero")]
        public HeroInfo? LastAddedHero { get; set; }
    }

    public class HeroInfo
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("userFriendlyName")]
        public string? UserFriendlyName { get; set; }

        [JsonPropertyName("urlName")]
        public string? UrlName { get; set; }

        [JsonPropertyName("attribute")]
        public string? Attribute { get; set; }

        [JsonPropertyName("talents")]
        public List<string>? Talents { get; set; }
    }
}
