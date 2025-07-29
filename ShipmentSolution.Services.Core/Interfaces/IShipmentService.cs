using Microsoft.AspNetCore.Mvc.Rendering;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.ShipmentViewModels;
using System.Security.Claims;

namespace ShipmentSolution.Services.Core.Interfaces
{
    public interface IShipmentService
    {
        Task<IEnumerable<ShipmentViewModel>> GetAllAsync();

        Task<ShipmentCreateViewModel> PrepareCreateViewModelAsync();

        Task CreateAsync(ShipmentCreateViewModel model, string userId);

        Task<ShipmentEditViewModel> GetForEditAsync(int id, string userId, ClaimsPrincipal user);

        Task<bool> EditAsync(ShipmentEditViewModel model, string userId, ClaimsPrincipal user);

        Task<ShipmentDeleteViewModel> GetForDeleteAsync(int id);

        Task<bool> DeleteAsync(int id, string userId, ClaimsPrincipal user);

        Task<PaginatedList<ShipmentViewModel>> GetPaginatedAsync(
            int pageIndex,
            int pageSize,
            string? searchTerm,
            string? shippingMethod,
            string? userId,
            bool isAdmin);

        Task<IEnumerable<SelectListItem>> GetCustomerListAsync();

        Task<IEnumerable<SelectListItem>> GetCarrierListAsync();

        Task<IEnumerable<SelectListItem>> GetRouteListAsync();
    }
}
