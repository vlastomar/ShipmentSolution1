using Microsoft.AspNetCore.Mvc.Rendering;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.CustomerViewModels;
using System.Security.Claims;

namespace ShipmentSolution.Services.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerViewModel>> GetAllAsync();
        Task<PaginatedList<CustomerViewModel>> GetPaginatedAsync(int pageIndex, int pageSize, string? searchTerm, string userId, bool isAdmin);
        Task<CustomerEditViewModel?> GetForEditAsync(int id, string userId, ClaimsPrincipal user);

        Task CreateAsync(CustomerCreateViewModel model, string userId);
        Task<bool> EditAsync(CustomerEditViewModel model, string userId, ClaimsPrincipal user);

        Task DeleteAsync(int id);
        Task<CustomerViewModel> GetByIdAsync(int id);
        Task SoftDeleteAsync(int id);
        IEnumerable<SelectListItem> GetShippingMethodOptionsPublic();
    }
}