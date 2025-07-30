using Microsoft.AspNetCore.Mvc.Rendering;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.CustomerViewModels;
using System.Security.Claims;

namespace ShipmentSolution.Services.Core.Interfaces
{
    public interface ICustomerService
    {
        // Get all non-deleted customers (used in older context, likely can be removed if unused)
        Task<IEnumerable<CustomerViewModel>> GetAllAsync();

        // Get paginated customer list, filtered by search term and optionally limited by userId if not admin
        Task<PaginatedList<CustomerViewModel>> GetPaginatedAsync(
            int pageIndex,
            int pageSize,
            string? searchTerm,
            string userId,
            bool isAdmin,
            bool isLoggedIn);

        // Get customer for editing, ensuring access by owner or admin
        Task<CustomerEditViewModel?> GetForEditAsync(int id, string userId, ClaimsPrincipal user);

        // Create a new customer with ownership tracking
        Task CreateAsync(CustomerCreateViewModel model, string userId);

        // Edit existing customer if user is owner or admin
        Task<bool> EditAsync(CustomerEditViewModel model, string userId, ClaimsPrincipal user);

        // Hard delete (optional; may not be used)
        Task DeleteAsync(int id);

        // Get basic customer view model by id
        Task<CustomerViewModel> GetByIdAsync(int id);

        // Soft delete customer (mark as deleted)
        Task SoftDeleteAsync(int id);

        // Dropdown values for PreferredShippingMethod
        IEnumerable<SelectListItem> GetShippingMethodOptionsPublic();
    }
}
