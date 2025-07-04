using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.CustomerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Services.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerViewModel>> GetAllAsync();
        Task<PaginatedList<CustomerViewModel>> GetPaginatedAsync(int pageIndex, int pageSize, string? searchTerm);
        Task<CustomerEditViewModel> GetForEditAsync(int id);
        Task CreateAsync(CustomerCreateViewModel model);
        Task EditAsync(CustomerEditViewModel model);
        Task DeleteAsync(int id);
        Task<CustomerViewModel> GetByIdAsync(int id);
        Task SoftDeleteAsync(int id);
    }
}
