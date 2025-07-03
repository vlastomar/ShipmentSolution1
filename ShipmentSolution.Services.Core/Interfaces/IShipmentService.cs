using Microsoft.AspNetCore.Mvc.Rendering;
using ShipmentSolution.Data;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.ShipmentViewModels;

namespace ShipmentSolution.Services.Core.Interfaces
{
    public interface IShipmentService
    {
        Task<IEnumerable<ShipmentViewModel>> GetAllAsync();
        Task<ShipmentCreateViewModel> PrepareCreateViewModelAsync();
        Task CreateAsync(ShipmentCreateViewModel model);

        Task<ShipmentEditViewModel> GetForEditAsync(int id);
        Task EditAsync(ShipmentEditViewModel model);

        Task<IEnumerable<SelectListItem>> GetCustomerListAsync();
        Task<IEnumerable<SelectListItem>> GetCarrierListAsync();
        Task<IEnumerable<SelectListItem>> GetRouteListAsync();
        Task<ShipmentDeleteViewModel> GetForDeleteAsync(int id);
        Task DeleteAsync(int id);
        Task<PaginatedList<ShipmentViewModel>> GetPaginatedAsync(int pageIndex, int pageSize, string? searchTerm, string? shippingMethod);
    }

}
