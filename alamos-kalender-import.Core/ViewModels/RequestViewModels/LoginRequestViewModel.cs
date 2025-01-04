using System.Text.Json.Serialization;

namespace DennisKae.alamos_kalender_import.Core.ViewModels.RequestViewModels
{
    public class LoginRequestViewModel
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }
        
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; } = "WEB";
    }
}