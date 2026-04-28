using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _1x6Helper.Models.Api;

public class SteamProfile
{
    [JsonPropertyName("data")]
    public SteamProfileData? Data { get; set; }

    [JsonPropertyName("errors")]
    public List<object>? Errors { get; set; }

    [JsonPropertyName("hasErrors")]
    public bool HasErrors { get; set; }
    
    public class SteamProfileData
    {
        [JsonPropertyName("steamId")]
        public long MatchId { get; set; }
        [JsonPropertyName("personaname")]
        public string? personaname { get; set; }
        [JsonPropertyName("profileurl")]
        public string? profileurl { get; set; }
        [JsonPropertyName("avatarfull")]
        public string? avatarfull {  get; set; }
    }
}
