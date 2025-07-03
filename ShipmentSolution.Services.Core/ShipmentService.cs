using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.ShipmentViewModels;
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
                    Customers = await context.Customers
                        .Select(c => new SelectListItem
                        {
                            Value = c.CustomerId.ToString(),
                            Text = $"{c.FirstName} {c.LastName}"
                        }).ToListAsync(),

                    Carriers = await context.MailCarriers
                        .Select(c => new SelectListItem
                        {
                            Value = c.MailCarrierId.ToString(),
                            Text = c.FirstName
                        }).ToListAsync(),

                    Routes = await context.Routes
                        .Select(r => new SelectListItem
                        {
                            Value = r.RouteId.ToString(),
                            Text = $"{r.StartLocation} → {r.EndLocation}"
                        }).ToListAsync()
                };
            }
            catch
            {
                return new ShipmentCreateViewModel();
            }
        }

        public async Task CreateAsync(ShipmentCreateViewModel model)
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
                    CustomerId = model.CustomerId
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

        public async Task<ShipmentEditViewModel> GetForEditAsync(int id)
        {
            try
            {
                var shipment = await context.Shipments.FindAsync(id);

                return new ShipmentEditViewModel
                {
                    ShipmentId = shipment.ShipmentId,
                    Weight = shipment.Weight,
                    Dimensions = shipment.Dimensions,
                    ShippingMethod = shipment.ShippingMethod,
                    ShippingCost = shipment.ShippingCost,
                    DeliveryDate = shipment.DeliveryDate,
                    CustomerId = shipment.CustomerId,
                    Customers = await context.Customers
                        .Select(c => new SelectListItem
                        {
                            Value = c.CustomerId.ToString(),
                            Text = c.FirstName + " " + c.LastName
                        }).ToListAsync()
                };
            }
            catch
            {
                return new ShipmentEditViewModel();
            }
        }

        public async Task EditAsync(ShipmentEditViewModel model)
        {
            try
            {
                var shipment = await context.Shipments.FindAsync(model.ShipmentId);

                shipment.Weight = model.Weight;
                shipment.Dimensions = model.Dimensions;
                shipment.ShippingMethod = model.ShippingMethod;
                shipment.ShippingCost = model.ShippingCost;
                shipment.DeliveryDate = model.DeliveryDate;
                shipment.CustomerId = model.CustomerId;

                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<ShipmentDeleteViewModel> GetForDeleteAsync(int id)
        {
            try
            {
                var shipment = await context.Shipments
                    .Include(s => s.Customer)
                    .FirstOrDefaultAsync(s => s.ShipmentId == id);

                if (shipment == null)
                    throw new ArgumentException("Shipment not found");

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
                return new ShipmentDeleteViewModel();
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var shipment = await context.Shipments.FirstOrDefaultAsync(s => s.ShipmentId == id);
                if (shipment != null)
                {
                    shipment.IsDeleted = true;
                    await context.SaveChangesAsync();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<PaginatedList<ShipmentViewModel>> GetPaginatedAsync(int pageIndex, int pageSize, string? searchTerm, string? shippingMethod)
        {
            try
            {
                var query = context.Shipments
                    .Include(s => s.Customer)
                    .Where(s => !s.IsDeleted);

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
                        DeliveryDate = s.DeliveryDate
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
    }
}
