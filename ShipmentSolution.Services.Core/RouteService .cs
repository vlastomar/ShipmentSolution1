using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.RouteViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentSolution.Services.Core
{
    public class RouteService : IRouteService
    {
        private readonly ApplicationDbContext context;

        public RouteService(ApplicationDbContext context)
        {
            this.context = context;
        }

        /*public async Task<IEnumerable<RouteViewModel>> GetAllAsync()
        {
            return await context.Routes
                .Select(r => new RouteViewModel
                {
                    RouteId = r.RouteId,
                    StartLocation = r.StartLocation,
                    EndLocation = r.EndLocation,
                    Stops = r.Stops,
                    Distance = r.Distance,
                    Priority = r.Priority.ToString()
                })
                .ToListAsync();
        }*/
        public async Task<IEnumerable<RouteViewModel>> GetAllAsync()
        {
            return await context.Routes
                .Where(r => !r.IsDeleted)
                .Select(r => new RouteViewModel
                {
                    RouteId = r.RouteId,
                    StartLocation = r.StartLocation,
                    EndLocation = r.EndLocation,
                    Stops = r.Stops,
                    Distance = r.Distance,
                    Priority = r.Priority.ToString()
                })
                .ToListAsync();
        }

        public async Task CreateAsync(RouteCreateViewModel model)
        {
            var entity = new Route
            {
                StartLocation = model.StartLocation,
                EndLocation = model.EndLocation,
                Stops = int.Parse(model.Stops),
                Distance = (float)model.Distance,
                Priority = model.Priority,
                MailCarrierId = model.MailCarrierId
            };
            context.Routes.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task<RouteEditViewModel> GetForEditAsync(int id)
        {
            var route = await context.Routes.FindAsync(id);
            return new RouteEditViewModel
            {
                RouteId = route.RouteId,
                StartLocation = route.StartLocation,
                EndLocation = route.EndLocation,
                Stops = route.Stops.ToString(),
                Distance = route.Distance,
                Priority = route.Priority
            };
        }

        public async Task EditAsync(RouteEditViewModel model)
        {
            var route = await context.Routes.FindAsync(model.RouteId);
            route.StartLocation = model.StartLocation;
            route.EndLocation = model.EndLocation;
            route.Stops = int.Parse(model.Stops);
            route.Distance = (float)model.Distance;
            route.Priority = model.Priority;

            await context.SaveChangesAsync();
        }

        public async Task<RouteViewModel> GetByIdAsync(int id)
        {
            var entity = await context.Routes.FindAsync(id);
            return new RouteViewModel
            {
                RouteId = entity.RouteId,
                StartLocation = entity.StartLocation,
                EndLocation = entity.EndLocation,
                Stops = entity.Stops,
                Distance = entity.Distance,
                Priority = entity.Priority.ToString()
            };
        }

        public async Task SoftDeleteAsync(int id)
        {
            var route = await context.Routes.FindAsync(id);
            if (route != null)
            {
                route.IsDeleted = true;
                await context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<SelectListItem>> GetCarrierListAsync()
        {
            return await context.MailCarriers
                .Where(c => !c.IsDeleted)
                .Select(c => new SelectListItem
                {
                    Value = c.MailCarrierId.ToString(),
                    Text = c.FirstName
                })
                .ToListAsync();
        }
        public async Task<RouteDeleteViewModel> GetDeleteViewModelAsync(int id)
        {
            var route = await context.Routes.FindAsync(id);

            return new RouteDeleteViewModel
            {
                RouteId = route.RouteId,
                StartLocation = route.StartLocation,
                EndLocation = route.EndLocation,
                Stops = route.Stops,
                Distance = route.Distance,
                Priority = route.Priority.ToString()
            };
        }
        public async Task<PaginatedList<RouteViewModel>> GetPaginatedAsync(
     int pageIndex, int pageSize, string? searchTerm, string? priorityFilter)
        {
            var query = context.Routes
                .Where(r => !r.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(r =>
                    r.StartLocation.Contains(searchTerm) ||
                    r.EndLocation.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(priorityFilter))
            {
                query = query.Where(r => r.Priority.ToString() == priorityFilter);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Select(r => new RouteViewModel
                {
                    RouteId = r.RouteId,
                    StartLocation = r.StartLocation,
                    EndLocation = r.EndLocation,
                    Stops = r.Stops,
                    Distance = r.Distance,
                    Priority = r.Priority.ToString()
                })
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<RouteViewModel>
            {
                Items = items,
                PageIndex = pageIndex,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }



    }
}
