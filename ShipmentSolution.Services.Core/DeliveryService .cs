using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.DeliveryViewModels;
using System.Security.Claims;

namespace ShipmentSolution.Services.Core
{
    public class DeliveryService : IDeliveryService
    {
        private readonly ApplicationDbContext context;

        public DeliveryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<PaginatedList<DeliveryViewModel>> GetPaginatedAsync(
    int pageIndex,
    int pageSize,
    string? searchTerm,
    string? mailCarrierFilter,
    string? userId,
    bool isAdmin,
    bool isLoggedIn)
        {
            try
            {
                var query = context.Deliveries
                    .Where(d => !d.IsDeleted)
                    .Include(d => d.Shipment).ThenInclude(s => s.Customer)
                    .Include(d => d.Route)
                    .Include(d => d.MailCarrier)
                    .AsQueryable();

                if (!isLoggedIn)
                {
                    query = query.Where(d => false); // Unauthenticated users see nothing
                }
                else if (!isAdmin && !string.IsNullOrEmpty(userId))
                {
                    // Only own records for non-admins
                    query = query.Where(d => d.Shipment.CreatedByUserId == userId);

                    // Apply mail carrier filter only within user's data
                    if (!string.IsNullOrWhiteSpace(mailCarrierFilter))
                    {
                        query = query.Where(d =>
                            (d.MailCarrier.FirstName + " " + d.MailCarrier.LastName) == mailCarrierFilter);
                    }
                }
                else if (isAdmin)
                {
                    // Admin can see and filter all
                    if (!string.IsNullOrWhiteSpace(mailCarrierFilter))
                    {
                        query = query.Where(d =>
                            (d.MailCarrier.FirstName + " " + d.MailCarrier.LastName) == mailCarrierFilter);
                    }
                }

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(d =>
                        d.Shipment.Customer.FirstName.ToLower().Contains(searchTerm) ||
                        d.Shipment.Customer.LastName.ToLower().Contains(searchTerm) ||
                        d.MailCarrier.FirstName.ToLower().Contains(searchTerm) ||
                        d.MailCarrier.LastName.ToLower().Contains(searchTerm));
                }

                var totalItems = await query.CountAsync();

                var items = await query
                    .OrderByDescending(d => d.DateDelivered)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Select(d => new DeliveryViewModel
                    {
                        DeliveryId = d.DeliveryId,
                        ShipmentInfo = $"Shipment #{d.ShipmentId} for {d.Shipment.Customer.FirstName} {d.Shipment.Customer.LastName}",
                        MailCarrierName = $"{d.MailCarrier.FirstName} {d.MailCarrier.LastName}",
                        Route = $"{d.Route.StartLocation} - {d.Route.EndLocation}",
                        DateDelivered = d.DateDelivered
                    })
                    .ToListAsync();

                return new PaginatedList<DeliveryViewModel>
                {
                    Items = items,
                    PageIndex = pageIndex,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
                };
            }
            catch
            {
                return new PaginatedList<DeliveryViewModel>
                {
                    Items = new List<DeliveryViewModel>(),
                    PageIndex = pageIndex,
                    TotalPages = 1
                };
            }
        }

        public async Task<DeliveryCreateViewModel> GetCreateModelAsync(string userId, ClaimsPrincipal user)
        {
            var shipmentsQuery = context.Shipments
                .Where(s => !s.IsDeleted);

            if (!user.IsInRole("Administrator"))
            {
                shipmentsQuery = shipmentsQuery.Where(s => s.CreatedByUserId == userId);
            }

            var shipments = await shipmentsQuery
                .Select(s => new SelectListItem
                {
                    Value = s.ShipmentId.ToString(),
                    Text = $"#{s.ShipmentId} - {s.ShippingMethod}"
                }).ToListAsync();

            var mailCarriers = await context.MailCarriers
                .Where(m => !m.IsDeleted)
                .Select(m => new SelectListItem
                {
                    Value = m.MailCarrierId.ToString(),
                    Text = m.FirstName + " " + m.LastName
                }).ToListAsync();

            var routes = await context.Routes
                .Where(r => !r.IsDeleted)
                .Select(r => new SelectListItem
                {
                    Value = r.RouteId.ToString(),
                    Text = r.StartLocation + " → " + r.EndLocation
                }).ToListAsync();

            return new DeliveryCreateViewModel
            {
                Shipments = shipments,
                MailCarriers = mailCarriers,
                Routes = routes
            };
        }

        public async Task<List<string>> GetCarrierNamesAsync()
        {
            return await context.MailCarriers
                .Where(c => !c.IsDeleted)
                .Select(c => c.FirstName + " " + c.LastName)
                .Distinct()
                .ToListAsync();
        }

        public async Task<DeliveryViewModel> GetByIdAsync(int id)
        {
            var d = await context.Deliveries
                .Include(x => x.Shipment)
                .Include(x => x.MailCarrier)
                .Include(x => x.Route)
                .FirstOrDefaultAsync(x => x.DeliveryId == id);

            if (d == null) throw new Exception("Delivery not found.");

            return new DeliveryViewModel
            {
                DeliveryId = d.DeliveryId,
                ShipmentInfo = $"Shipment #{d.ShipmentId} - {d.Shipment.ShippingMethod}",
                MailCarrierName = d.MailCarrier.FirstName + " " + d.MailCarrier.LastName,
                Route = d.Route.StartLocation + " → " + d.Route.EndLocation,
                DateDelivered = d.DateDelivered
            };
        }

        public async Task<DeliveryDeleteViewModel> GetDeleteViewModelAsync(int id, string userId, ClaimsPrincipal user)
        {
            try
            {
                var delivery = await context.Deliveries
                    .Include(d => d.Shipment)
                    .Include(d => d.MailCarrier)
                    .Include(d => d.Route)
                    .FirstOrDefaultAsync(d => d.DeliveryId == id);

                if (delivery == null)
                    throw new ArgumentException("Delivery not found.");
                if (!user.IsInRole("Administrator") && delivery.Shipment.CreatedByUserId != userId)
                    return null;

                return new DeliveryDeleteViewModel
                {
                    DeliveryId = delivery.DeliveryId,
                    ShipmentInfo = $"#{delivery.Shipment.ShipmentId} - {delivery.Shipment.ShippingMethod}",
                    MailCarrierName = delivery.MailCarrier.FirstName + " " + delivery.MailCarrier.LastName,
                    Route = $"{delivery.Route.StartLocation} → {delivery.Route.EndLocation}",
                    DateDelivered = delivery.DateDelivered
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching the delivery for deletion.", ex);
            }
        }

        public async Task<DeliveryEditViewModel> GetForEditAsync(int id, string userId, ClaimsPrincipal user)
        {
            var d = await context.Deliveries
                .Include(d => d.Shipment)
                .FirstOrDefaultAsync(d => d.DeliveryId == id && !d.IsDeleted);

            if (d == null) throw new Exception("Delivery not found.");

            // Only admin or owner can edit
            if (!user.IsInRole("Administrator") && d.Shipment.CreatedByUserId != userId)
                return null;

            bool isAdmin = user.IsInRole("Administrator");

            var shipments = await context.Shipments
                .Where(s => !s.IsDeleted && (isAdmin || s.CreatedByUserId == userId))
                .Select(s => new SelectListItem
                {
                    Value = s.ShipmentId.ToString(),
                    Text = $"#{s.ShipmentId} - {s.ShippingMethod}"
                }).ToListAsync();

            var mailCarriers = await context.MailCarriers
                .Where(m => !m.IsDeleted && (isAdmin || m.CreatedByUserId == userId))
                .Select(m => new SelectListItem
                {
                    Value = m.MailCarrierId.ToString(),
                    Text = m.FirstName + " " + m.LastName
                }).ToListAsync();

            var routes = await context.Routes
                .Where(r => !r.IsDeleted && (isAdmin || r.CreatedByUserId == userId))
                .Select(r => new SelectListItem
                {
                    Value = r.RouteId.ToString(),
                    Text = r.StartLocation + " → " + r.EndLocation
                }).ToListAsync();

            return new DeliveryEditViewModel
            {
                DeliveryId = d.DeliveryId,
                ShipmentId = d.ShipmentId,
                MailCarrierId = d.MailCarrierId,
                RouteId = d.RouteId,
                DateDelivered = d.DateDelivered,
                Shipments = shipments,
                MailCarriers = mailCarriers,
                Routes = routes
            };
        }


        public async Task CreateAsync(DeliveryCreateViewModel model, string userId)
        {
            try
            {
                var shipment = await context.Shipments
                    .FirstOrDefaultAsync(s => s.ShipmentId == model.ShipmentId && !s.IsDeleted);

                if (shipment == null)
                    throw new ArgumentException("Shipment not found.");

                var isAdmin = context.Users
                    .Where(u => u.Id == userId)
                    .Any(u => u.UserName == "admin@example.com");

                if (!isAdmin && shipment.CreatedByUserId != userId)
                    throw new UnauthorizedAccessException("Not your shipment.");

                var delivery = new Delivery
                {
                    ShipmentId = model.ShipmentId!.Value,
                    MailCarrierId = model.MailCarrierId!.Value,
                    RouteId = model.RouteId!.Value,
                    DateDelivered = model.DateDelivered!.Value
                };

                context.Deliveries.Add(delivery);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating delivery.", ex);
            }
        }

        public async Task<bool> EditAsync(DeliveryEditViewModel model, string userId, ClaimsPrincipal user)
        {
            try
            {
                var d = await context.Deliveries
                   .Include(d => d.Shipment)
                   .FirstOrDefaultAsync(d => d.DeliveryId == model.DeliveryId && !d.IsDeleted);

                if (d == null) return false;

                if (!user.IsInRole("Administrator") && d.Shipment.CreatedByUserId != userId)
                    return false;

                d.ShipmentId = model.ShipmentId;
                d.MailCarrierId = model.MailCarrierId;
                d.RouteId = model.RouteId;
                d.DateDelivered = model.DateDelivered;

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while editing the delivery.", ex);
            }
        }

        public async Task<bool> SoftDeleteAsync(int id, string userId, ClaimsPrincipal user)
        {
            try
            {
                var delivery = await context.Deliveries
                    .Include(d => d.Shipment)
                    .FirstOrDefaultAsync(d => d.DeliveryId == id && !d.IsDeleted);

                if (delivery == null)
                    return false;

                if (!user.IsInRole("Administrator") && delivery.Shipment.CreatedByUserId != userId)
                    return false;

                delivery.IsDeleted = true;
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Error: ", e);
            }
        }

        public async Task<IEnumerable<DeliveryViewModel>> GetAllAsync()
        {
            return await context.Deliveries
                .Where(d => !d.IsDeleted)
                .Include(d => d.Shipment).ThenInclude(s => s.Customer)
                .Include(d => d.Route)
                .Include(d => d.MailCarrier)
                .Select(d => new DeliveryViewModel
                {
                    DeliveryId = d.DeliveryId,
                    ShipmentInfo = $"Shipment #{d.ShipmentId} for {d.Shipment.Customer.FirstName} {d.Shipment.Customer.LastName}",
                    MailCarrierName = $"{d.MailCarrier.FirstName} {d.MailCarrier.LastName}",
                    Route = $"{d.Route.StartLocation} - {d.Route.EndLocation}",
                    DateDelivered = d.DateDelivered
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetCarrierListAsync(string userId, ClaimsPrincipal user)
        {
            var isAdmin = user.IsInRole("Administrator");

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
            var isAdmin = user.IsInRole("Administrator");

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

        public async Task<DeliveryCreateViewModel> PrepareCreateViewModelAsync(string userId, ClaimsPrincipal user)
        {
            bool isAdmin = user.IsInRole("Administrator");

            var model = new DeliveryCreateViewModel
            {
                Shipments = await context.Shipments
                    .Where(s => !s.IsDeleted && (isAdmin || s.CreatedByUserId == userId))
                    .Select(s => new SelectListItem
                    {
                        Value = s.ShipmentId.ToString(),
                        Text = $"#{s.ShipmentId} - {s.ShippingMethod}"
                    })
                    .ToListAsync(),

                MailCarriers = await context.MailCarriers
                    .Where(c => !c.IsDeleted && (isAdmin || c.CreatedByUserId == userId))
                    .Select(c => new SelectListItem
                    {
                        Value = c.MailCarrierId.ToString(),
                        Text = $"{c.FirstName} {c.LastName}"
                    })
                    .ToListAsync(),

                Routes = await context.Routes
                    .Where(r => !r.IsDeleted && (isAdmin || r.CreatedByUserId == userId))
                    .Select(r => new SelectListItem
                    {
                        Value = r.RouteId.ToString(),
                        Text = $"{r.StartLocation} → {r.EndLocation}"
                    })
                    .ToListAsync()
            };

            return model;
        }

        public async Task<IEnumerable<SelectListItem>> GetShipmentListAsync(string userId, ClaimsPrincipal user)
        {
            bool isAdmin = user.IsInRole("Administrator");

            return await context.Shipments
                .Where(s => !s.IsDeleted && (isAdmin || s.CreatedByUserId == userId))
                .Select(s => new SelectListItem
                {
                    Value = s.ShipmentId.ToString(),
                    Text = $"#{s.ShipmentId} - {s.ShippingMethod}"
                })
                .ToListAsync();
        }


    }
}
