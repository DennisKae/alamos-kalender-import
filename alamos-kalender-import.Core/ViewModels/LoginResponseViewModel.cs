using System.Text.Json.Serialization;

namespace DennisKae.alamos_kalender_import.Core.ViewModels
{
    public class LoginResponseViewModel
    {
        [JsonPropertyName("token")]
        public string ApiToken { get; set; }
    }
}