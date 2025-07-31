using Microsoft.AspNetCore.Mvc.Rendering;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.RouteViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShipmentSolution.Services.Core.Interfaces
{


    public interface IRouteService
    {
        Task<IEnumerable<RouteViewModel>> GetAllAsync();

        Task<PaginatedList<RouteViewModel>> GetPaginatedAsync(
            int pageIndex,
            int pageSize,
            string? searchTerm,
            string? priorityFilter,
            string? userId,
            bool isAdmin);

        Task CreateAsync(RouteCreateViewModel model, string userId);

        Task<RouteEditViewModel> GetForEditAsync(int id);
        Task<IEnumerable<SelectListItem>> GetMailCarriersAsync(string userId, ClaimsPrincipal user);


        Task EditAsync(RouteEditViewModel model);

        Task<RouteViewModel> GetByIdAsync(int id);

        Task<RouteDeleteViewModel> GetDeleteViewModelAsync(int id);

        Task SoftDeleteAsync(int id);

        Task<IEnumerable<SelectListItem>> GetCarrierListAsync();
    }
}
