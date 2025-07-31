using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.MailCarrierViewModels;

namespace ShipmentSolution.Services.Core.Interfaces
{
    public interface IMailCarrierService
    {
        Task<IEnumerable<MailCarrierViewModel>> GetAllAsync();

        Task<PaginatedList<MailCarrierViewModel>> GetPaginatedAsync(
            int pageIndex,
            int pageSize,
            string? searchTerm,
            string? statusFilter,
            string? userId,
            bool isAdmin);

        Task<MailCarrierEditViewModel> GetForEditAsync(int id);

        Task<MailCarrierDeleteViewModel> GetByIdAsync(int id);

        // ✅ Updated to accept userId for CreatedByUserId assignment
        Task CreateAsync(MailCarrierCreateViewModel model, string userId);

        Task EditAsync(MailCarrierEditViewModel model);

        Task SoftDeleteAsync(int id);
    }
}
