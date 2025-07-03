using Microsoft.AspNetCore.Mvc.Rendering;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.RouteViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Services.Core.Interfaces
{
    public interface IRouteService
    {
        Task<IEnumerable<RouteViewModel>> GetAllAsync();
        Task<PaginatedList<RouteViewModel>> GetPaginatedAsync(
    int pageIndex, int pageSize, string? searchTerm, string? priorityFilter);
        Task CreateAsync(RouteCreateViewModel model);
        Task<RouteEditViewModel> GetForEditAsync(int id);
        Task EditAsync(RouteEditViewModel model);
        Task<RouteViewModel> GetByIdAsync(int id);
        Task SoftDeleteAsync(int id);
        Task<IEnumerable<SelectListItem>> GetCarrierListAsync();
        Task<RouteDeleteViewModel> GetDeleteViewModelAsync(int id);

    }
}
