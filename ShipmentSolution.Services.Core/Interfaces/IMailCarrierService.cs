using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.MailCarrierViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Services.Core.Interfaces
{
    public interface IMailCarrierService
    {
        Task<IEnumerable<MailCarrierViewModel>> GetAllAsync();
        Task<PaginatedList<MailCarrierViewModel>> GetPaginatedAsync(int pageIndex, int pageSize, string? searchTerm, string? statusFilter);
        Task<MailCarrierEditViewModel> GetForEditAsync(int id);
        Task<MailCarrierDeleteViewModel> GetByIdAsync(int id);
        Task CreateAsync(MailCarrierCreateViewModel model);
        Task EditAsync(MailCarrierEditViewModel model);
        Task SoftDeleteAsync(int id);
    }
}
