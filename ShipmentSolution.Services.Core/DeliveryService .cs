using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.DeliveryViewModels;

namespace ShipmentSolution.Services.Core
{
    public class DeliveryService : IDeliveryService
    {
        private readonly ApplicationDbContext context;

        public DeliveryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<DeliveryViewModel>> GetAllAsync()
        {
            try
            {
                return await context.Deliveries
                    .Where(d => !d.IsDeleted)
                    .Select(d => new DeliveryViewModel
                    {
                        DeliveryId = d.DeliveryId,
                        ShipmentInfo = $"Shipment #{d.ShipmentId} - {d.Shipment.ShippingMethod}",
                        MailCarrierName = d.MailCarrier.FirstName + " " + d.MailCarrier.LastName,
                        Route = d.Route.StartLocation + " → " + d.Route.EndLocation,
                        DateDelivered = d.DateDelivered
                    })
                    .ToListAsync();
            }
            catch
            {
                return new List<DeliveryViewModel>();
            }
        }

        public async Task CreateAsync(DeliveryCreateViewModel model)
        {
            try
            {
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
            catch
            {
                // log exception
                throw;
            }
        }

        public async Task<DeliveryEditViewModel> GetForEditAsync(int id)
        {
            var d = await context.Deliveries.FindAsync(id);
            if (d == null) throw new Exception("Delivery not found.");

            var shipments = await context.Shipments
                .Where(s => !s.IsDeleted)
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

        public async Task EditAsync(DeliveryEditViewModel model)
        {
            try
            {
                var d = await context.Deliveries.FindAsync(model.DeliveryId);
                if (d == null) throw new Exception("Delivery not found.");

                d.ShipmentId = model.ShipmentId;
                d.MailCarrierId = model.MailCarrierId;
                d.RouteId = model.RouteId;
                d.DateDelivered = model.DateDelivered;

                await context.SaveChangesAsync();
            }
            catch
            {
                // log exception
                throw;
            }
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

        public async Task SoftDeleteAsync(int id)
        {
            try
            {
                var d = await context.Deliveries.FindAsync(id);
                if (d != null)
                {
                    d.IsDeleted = true;
                    await context.SaveChangesAsync();
                }
            }
            catch
            {
                // log exception
                throw;
            }
        }

        public async Task<DeliveryCreateViewModel> GetCreateModelAsync()
        {
            var shipments = await context.Shipments
                .Where(s => !s.IsDeleted)
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
                // DateDelivered left null intentionally
            };
        }

        public async Task<PaginatedList<DeliveryViewModel>> GetPaginatedAsync(int pageIndex, int pageSize, string? searchTerm, string? mailCarrierFilter)
        {
            try
            {
                var query = context.Deliveries
                    .Where(d => !d.IsDeleted)
                    .Include(d => d.Shipment).ThenInclude(s => s.Customer)
                    .Include(d => d.Route)
                    .Include(d => d.MailCarrier)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(d =>
                        d.Shipment.Customer.FirstName.ToLower().Contains(searchTerm) ||
                        d.Shipment.Customer.LastName.ToLower().Contains(searchTerm) ||
                        d.MailCarrier.FirstName.ToLower().Contains(searchTerm) ||
                        d.MailCarrier.LastName.ToLower().Contains(searchTerm));
                }

                if (!string.IsNullOrWhiteSpace(mailCarrierFilter))
                {
                    query = query.Where(d =>
                        (d.MailCarrier.FirstName + " " + d.MailCarrier.LastName) == mailCarrierFilter);
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

        public async Task<List<string>> GetCarrierNamesAsync()
        {
            return await context.MailCarriers
                .Where(c => !c.IsDeleted)
                .Select(c => c.FirstName + " " + c.LastName)
                .Distinct()
                .ToListAsync();
        }

        public async Task<DeliveryDeleteViewModel> GetDeleteViewModelAsync(int id)
        {
            try
            {
                var delivery = await context.Deliveries
                    .Include(d => d.Shipment)
                    .Include(d => d.MailCarrier)
                    .Include(d => d.Route)
                    .FirstOrDefaultAsync(d => d.DeliveryId == id);

                if (delivery == null)
                {
                    throw new ArgumentException("Delivery not found.");
                }

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
    }
}
