using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core.ViewModels.RequestViewModels;
using DennisKae.alamos_kalender_import.Core.ViewModels.ResponseViewModels;
using Refit;

namespace DennisKae.alamos_kalender_import.Core.Services.RestServices.Interfaces
{
    /// <summary>Service mit den REST API Methoden für Auth Themen</summary>
    public interface IAuthRestService
    {
        [Post("/rest/login")]
        Task<LoginResponseViewModel> Login(LoginRequestViewModel model);
    }
}