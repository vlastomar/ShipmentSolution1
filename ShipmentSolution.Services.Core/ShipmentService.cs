using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.ShipmentViewModels;
using System.Security.Claims;

namespace ShipmentSolution.Services.Core
{
    public class ShipmentService : IShipmentService
    {
        private readonly ApplicationDbContext context;

        public ShipmentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ShipmentViewModel>> GetAllAsync()
        {
            return await context.Shipments
                .Include(s => s.Customer)
                .Where(s => !s.IsDeleted)
                .Select(s => new ShipmentViewModel
                {
                    ShipmentId = s.ShipmentId,
                    CustomerName = s.Customer.FirstName + " " + s.Customer.LastName,
                    ShippingMethod = s.ShippingMethod,
                    ShippingCost = (decimal)s.ShippingCost,
                    DeliveryDate = s.DeliveryDate
                })
                .ToListAsync();
        }

        public async Task<ShipmentCreateViewModel> PrepareCreateViewModelAsync(string userId, ClaimsPrincipal user)
    {
        return new ShipmentCreateViewModel
        {
            Customers = await GetCustomerListAsync(userId, user),
            Carriers = await GetCarrierListAsync(userId, user),
            Routes = await GetRouteListAsync(userId, user),
            ShippingMethods = GetStaticShippingMethods()
        };
    }


        public async Task CreateAsync(ShipmentCreateViewModel model, string userId)
        {
            var shipment = new ShipmentEntity
            {
                Weight = model.Weight,
                Dimensions = model.Dimensions,
                ShippingMethod = model.ShippingMethod,
                ShippingCost = (float)model.ShippingCost,
                DeliveryDate = (DateTime)model.DeliveryDate,
                CustomerId = model.CustomerId,
                CreatedByUserId = userId
            };

            await context.Shipments.AddAsync(shipment);
            await context.SaveChangesAsync();

            var delivery = new Delivery
            {
                ShipmentId = shipment.ShipmentId,
                MailCarrierId = model.CarrierId,
                RouteId = model.RouteId,
                DateDelivered = DateTime.UtcNow
            };

            await context.Deliveries.AddAsync(delivery);
            await context.SaveChangesAsync();
        }

        public async Task<ShipmentEditViewModel?> GetForEditAsync(int id, string userId, ClaimsPrincipal user)
        {
            var shipment = await context.Shipments.FindAsync(id);
            if (shipment == null || shipment.IsDeleted)
                return null;

            if (!user.IsInRole("Administrator") && shipment.CreatedByUserId != userId)
                return null;

            return new ShipmentEditViewModel
            {
                ShipmentId = shipment.ShipmentId,
                Weight = shipment.Weight,
                Dimensions = shipment.Dimensions,
                ShippingMethod = shipment.ShippingMethod,
                ShippingCost = shipment.ShippingCost,
                DeliveryDate = shipment.DeliveryDate,
                CustomerId = shipment.CustomerId,
                Customers = await GetCustomerListAsync(userId, user),
                ShippingMethods = GetStaticShippingMethods()
            };
        }


        public async Task<bool> EditAsync(ShipmentEditViewModel model, string userId, ClaimsPrincipal user)
        {
            var shipment = await context.Shipments.FindAsync(model.ShipmentId);
            if (shipment == null || shipment.IsDeleted)
                return false;

            if (!user.IsInRole("Administrator") && shipment.CreatedByUserId != userId)
                return false;

            shipment.Weight = model.Weight;
            shipment.Dimensions = model.Dimensions;
            shipment.ShippingMethod = model.ShippingMethod;
            shipment.ShippingCost = model.ShippingCost;
            shipment.DeliveryDate = model.DeliveryDate;
            shipment.CustomerId = model.CustomerId;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<ShipmentDeleteViewModel?> GetForDeleteAsync(int id)
        {
            var shipment = await context.Shipments
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(s => s.ShipmentId == id);

            if (shipment == null)
                return null;

            return new ShipmentDeleteViewModel
            {
                ShipmentId = shipment.ShipmentId,
                CustomerName = shipment.Customer.FirstName + " " + shipment.Customer.LastName,
                ShippingMethod = shipment.ShippingMethod,
                ShippingCost = (decimal)shipment.ShippingCost,
                DeliveryDate = shipment.DeliveryDate
            };
        }

        public async Task<bool> DeleteAsync(int id, string userId, ClaimsPrincipal user)
        {
            var shipment = await context.Shipments.FirstOrDefaultAsync(s => s.ShipmentId == id);

            if (shipment == null || shipment.IsDeleted)
                return false;

            if (!user.IsInRole("Administrator") && shipment.CreatedByUserId != userId)
                return false;

            shipment.IsDeleted = true;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<PaginatedList<ShipmentViewModel>> GetPaginatedAsync(
            int pageIndex,
            int pageSize,
            string? searchTerm,
            string? shippingMethod,
            string? userId,
            bool isAdmin)
        {
            var query = context.Shipments
                .Include(s => s.Customer)
                .Where(s => !s.IsDeleted);

            if (!isAdmin && !string.IsNullOrEmpty(userId))
            {
                query = query.Where(s => s.CreatedByUserId == userId);
            }
            else if (!isAdmin && string.IsNullOrEmpty(userId))
            {
                query = query.Where(s => false); // Anonymous users see nothing
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s =>
                    s.Customer.FirstName.Contains(searchTerm) ||
                    s.Customer.LastName.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(shippingMethod))
            {
                query = query.Where(s => s.ShippingMethod == shippingMethod);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(s => s.DeliveryDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new ShipmentViewModel
                {
                    ShipmentId = s.ShipmentId,
                    CustomerName = s.Customer.FirstName + " " + s.Customer.LastName,
                    ShippingMethod = s.ShippingMethod,
                    ShippingCost = (decimal)s.ShippingCost,
                    DeliveryDate = s.DeliveryDate,
                    CreatedByUserId = s.CreatedByUserId
                })
                .ToListAsync();

            return new PaginatedList<ShipmentViewModel>
            {
                Items = items,
                PageIndex = pageIndex,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<IEnumerable<SelectListItem>> GetCustomerListAsync(string userId, ClaimsPrincipal user)
        {
            bool isAdmin = user.IsInRole("Administrator");

            var query = context.Customers.AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(c => c.CreatedByUserId == userId);
            }

            return await query
                .Select(c => new SelectListItem
                {
                    Value = c.CustomerId.ToString(),
                    Text = c.FirstName + " " + c.LastName
                })
                .ToListAsync();
        }


        public async Task<IEnumerable<SelectListItem>> GetCarrierListAsync(string userId, ClaimsPrincipal user)
        {
            bool isAdmin = user.IsInRole("Administrator");

            var query = context.MailCarriers.AsQueryable();

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


        public async Task<IEnumerable<SelectListItem>> GetRouteListAsync(string userId, ClaimsPrincipal user)
        {
            bool isAdmin = user.IsInRole("Administrator");

            var query = context.Routes.AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(r => r.CreatedByUserId == userId);
            }

            return await query
                .Select(r => new SelectListItem
                {
                    Value = r.RouteId.ToString(),
                    Text = $"{r.StartLocation} → {r.EndLocation}"
                })
                .ToListAsync();
        }



        private List<SelectListItem> GetStaticShippingMethods()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Ground", Text = "Ground" },
                new SelectListItem { Value = "Express", Text = "Express" }
            };
        }
    }
}