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
using System.Security.Claims;
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

        private int MapTextToPriority(string? text)
        {
            return text switch
            {
                "Low" => 1,
                "Medium" => 2,
                "High" => 3,
                _ => 0
            };
        }

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
                    Priority = r.Priority,
                    CreatedByUserId = r.CreatedByUserId,
                    IsOwnedByCurrentUser = false // default; set only in paginated context
                })
                .ToListAsync();
        }

        public async Task CreateAsync(RouteCreateViewModel model, string userId)
        {
            if (string.IsNullOrWhiteSpace(model.Stops))
                throw new ArgumentException("Stops is required and must be a number.");

            var entity = new Route
            {
                StartLocation = model.StartLocation,
                EndLocation = model.EndLocation,
                Stops = int.Parse(model.Stops),
                Distance = (float)model.Distance,
                Priority = model.Priority,
                MailCarrierId = model.MailCarrierId,
                CreatedByUserId = userId
            };

            context.Routes.Add(entity);
            await context.SaveChangesAsync();
        }

        public async Task<RouteEditViewModel> GetForEditAsync(int id)
        {
            var route = await context.Routes.FindAsync(id)
                ?? throw new Exception("Route not found.");

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
            if (string.IsNullOrWhiteSpace(model.Stops))
                throw new ArgumentException("Stops is required and must be a number.");
            var route = await context.Routes.FindAsync(model.RouteId)
                ?? throw new Exception("Route not found.");

            route.StartLocation = model.StartLocation;
            route.EndLocation = model.EndLocation;
            route.Stops = int.Parse(model.Stops);
            route.Distance = (float)model.Distance;
            route.Priority = model.Priority;

            await context.SaveChangesAsync();
        }

        public async Task<RouteViewModel> GetByIdAsync(int id)
        {
            var entity = await context.Routes.FindAsync(id)
                ?? throw new Exception("Route not found.");

            return new RouteViewModel
            {
                RouteId = entity.RouteId,
                StartLocation = entity.StartLocation,
                EndLocation = entity.EndLocation,
                Stops = entity.Stops,
                Distance = entity.Distance,
                Priority = entity.Priority,
                CreatedByUserId = entity.CreatedByUserId,
                IsOwnedByCurrentUser = false
            };
        }

        public async Task<RouteDeleteViewModel> GetDeleteViewModelAsync(int id)
        {
            var route = await context.Routes.FindAsync(id)
                ?? throw new Exception("Route not found.");

            return new RouteDeleteViewModel
            {
                RouteId = route.RouteId,
                StartLocation = route.StartLocation,
                EndLocation = route.EndLocation,
                Stops = route.Stops,
                Distance = route.Distance,
                Priority = route.Priority
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
                    Text = $"{c.FirstName} {c.LastName}"
                })
                .ToListAsync();
        }

        public async Task<PaginatedList<RouteViewModel>> GetPaginatedAsync(
            int pageIndex, int pageSize, string? searchTerm, string? priorityFilter, string? userId, bool isAdmin)
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
                int parsedPriority = MapTextToPriority(priorityFilter);
                if (parsedPriority > 0)
                {
                    query = query.Where(r => r.Priority == parsedPriority);
                }
            }

            if (!isAdmin && !string.IsNullOrEmpty(userId))
            {
                query = query.Where(r => r.CreatedByUserId == userId);
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
                    Priority = r.Priority,
                    CreatedByUserId = r.CreatedByUserId,
                    IsOwnedByCurrentUser = r.CreatedByUserId == userId
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

        public async Task<IEnumerable<SelectListItem>> GetMailCarriersAsync(string userId, ClaimsPrincipal user)
        {
            var isAdmin = user.IsInRole("Administrator");

            var query = context.MailCarriers.Where(c => !c.IsDeleted);

            if (!isAdmin)
            {
                query = query.Where(c => c.CreatedByUserId == userId);
            }

            return await query
                .Select(c => new SelectListItem
                {
                    Value = c.MailCarrierId.ToString(),
                    Text = c.FirstName + " " + c.LastName
                })
                .ToListAsync();
        }


    }
}
