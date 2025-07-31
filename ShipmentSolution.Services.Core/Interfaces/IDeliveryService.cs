using Microsoft.AspNetCore.Mvc.Rendering;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.DeliveryViewModels;
using System.Security.Claims;

namespace ShipmentSolution.Services.Core.Interfaces
{
    public interface IDeliveryService
    {
        Task<IEnumerable<SelectListItem>> GetShipmentListAsync(string userId, ClaimsPrincipal user);

        Task<DeliveryCreateViewModel> PrepareCreateViewModelAsync(string userId, ClaimsPrincipal user);

        Task<IEnumerable<SelectListItem>> GetCarrierListAsync(string userId, ClaimsPrincipal user);
        Task<IEnumerable<SelectListItem>> GetRouteListAsync(string userId, ClaimsPrincipal user);

        Task<IEnumerable<DeliveryViewModel>> GetAllAsync();

        Task<DeliveryCreateViewModel> GetCreateModelAsync(string userId, ClaimsPrincipal user);

        Task CreateAsync(DeliveryCreateViewModel model, string userId);

        Task<DeliveryEditViewModel> GetForEditAsync(int id, string userId, ClaimsPrincipal user);

        Task<bool> EditAsync(DeliveryEditViewModel model, string userId, ClaimsPrincipal user);

        Task<DeliveryViewModel> GetByIdAsync(int id);

        Task<bool> SoftDeleteAsync(int id, string userId, ClaimsPrincipal user);

        Task<PaginatedList<DeliveryViewModel>> GetPaginatedAsync(
            int pageIndex,
            int pageSize,
            string? searchTerm,
            string? statusFilter,
            string userId,
            bool isAdmin,
            bool isLoggedIn); // ← added parameter

        Task<List<string>> GetCarrierNamesAsync();

        Task<DeliveryDeleteViewModel> GetDeleteViewModelAsync(int id, string userId, ClaimsPrincipal user);
    }
}
