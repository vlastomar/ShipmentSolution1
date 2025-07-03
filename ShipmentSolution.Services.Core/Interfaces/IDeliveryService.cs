using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.DeliveryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Services.Core.Interfaces
{
    public interface IDeliveryService
    {

        Task<IEnumerable<DeliveryViewModel>> GetAllAsync();
        Task<DeliveryCreateViewModel> GetCreateModelAsync();
        Task CreateAsync(DeliveryCreateViewModel model);
        Task<DeliveryEditViewModel> GetForEditAsync(int id);
        Task EditAsync(DeliveryEditViewModel model);
        Task<DeliveryViewModel> GetByIdAsync(int id);
        Task SoftDeleteAsync(int id);
        Task<PaginatedList<DeliveryViewModel>> GetPaginatedAsync(int pageIndex, int pageSize, string? searchTerm, string? statusFilter);
        Task<List<string>> GetCarrierNamesAsync();



    }
}
