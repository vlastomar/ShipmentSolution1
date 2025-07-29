using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.ShipmentViewModels;
using System.Security.Claims;
using static ShipmentSolution.GCommon.ValidationConstants;

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
            try
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
            catch
            {
                return new List<ShipmentViewModel>();
            }
        }

        public async Task<ShipmentCreateViewModel> PrepareCreateViewModelAsync()
        {
            try
            {
                return new ShipmentCreateViewModel
                {
                    Customers = await GetCustomerListAsync(),
                    Carriers = await GetCarrierListAsync(),
                    Routes = await GetRouteListAsync(),
                    ShippingMethods = GetStaticShippingMethods()
                };
            }
            catch
            {
                return new ShipmentCreateViewModel();
            }
        }

        public async Task CreateAsync(ShipmentCreateViewModel model, string userId)
        {
            try
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
            catch
            {
                throw;
            }
        }

        public async Task<ShipmentEditViewModel?> GetForEditAsync(int id, string userId, ClaimsPrincipal user)
        {
            try
            {
                var shipment = await context.Shipments.FindAsync(id);
                if (shipment == null || shipment.IsDeleted)
                    return null;
                // Only allow Admins or the creator of the shipment
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
                    Customers = await GetCustomerListAsync(),
                    ShippingMethods = GetStaticShippingMethods()
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> EditAsync(ShipmentEditViewModel model, string userId, ClaimsPrincipal user)
        {
            try
            {
                var shipment = await context.Shipments.FindAsync(model.ShipmentId);
                if (shipment == null || shipment.IsDeleted)
                    return false;

                // Allow only Admins or the creator of the shipment to edit
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
            catch
            {
                return false;
            }
        }

        public async Task<ShipmentDeleteViewModel?> GetForDeleteAsync(int id)
        {
            try
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
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id, string userId, ClaimsPrincipal user)
        {
            try
            {
                var shipment = await context.Shipments.FirstOrDefaultAsync(s => s.ShipmentId == id);

                if (shipment == null || shipment.IsDeleted)
                    return false;

                // Only Admin or creator can delete
                if (!user.IsInRole("Administrator") && shipment.CreatedByUserId != userId)
                    return false;

                shipment.IsDeleted = true;
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // You can optionally log the exception here
                return false;
            }
        }


        public async Task<PaginatedList<ShipmentViewModel>> GetPaginatedAsync(int pageIndex, int pageSize, string? searchTerm, string? shippingMethod, string? userId, bool isAdmin)
        {
            try
            {
                var query = context.Shipments
                    .Include(s => s.Customer)
                    .Where(s => !s.IsDeleted);

                if (!isAdmin && !string.IsNullOrEmpty(userId))
                {
                    query = query.Where(s => s.CreatedByUserId == userId);
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
                    .Select(s => new ShipmentViewModel
                    {
                        ShipmentId = s.ShipmentId,
                        CustomerName = s.Customer.FirstName + " " + s.Customer.LastName,
                        ShippingMethod = s.ShippingMethod,
                        ShippingCost = (decimal)s.ShippingCost,
                        DeliveryDate = s.DeliveryDate,
                        CreatedByUserId = s.CreatedByUserId
                    })
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PaginatedList<ShipmentViewModel>
                {
                    Items = items,
                    PageIndex = pageIndex,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                };
            }
            catch
            {
                return new PaginatedList<ShipmentViewModel>();
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetCustomerListAsync()
        {
            try
            {
                return await context.Customers
                    .Select(c => new SelectListItem
                    {
                        Value = c.CustomerId.ToString(),
                        Text = c.FirstName + " " + c.LastName
                    })
                    .ToListAsync();
            }
            catch
            {
                return new List<SelectListItem>();
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetCarrierListAsync()
        {
            try
            {
                return await context.MailCarriers
                    .Select(c => new SelectListItem
                    {
                        Value = c.MailCarrierId.ToString(),
                        Text = c.FirstName + " " + c.LastName
                    })
                    .ToListAsync();
            }
            catch
            {
                return new List<SelectListItem>();
            }
        }

        public async Task<IEnumerable<SelectListItem>> GetRouteListAsync()
        {
            try
            {
                return await context.Routes
                    .Select(r => new SelectListItem
                    {
                        Value = r.RouteId.ToString(),
                        Text = $"{r.StartLocation} → {r.EndLocation}"
                    })
                    .ToListAsync();
            }
            catch
            {
                return new List<SelectListItem>();
            }
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
