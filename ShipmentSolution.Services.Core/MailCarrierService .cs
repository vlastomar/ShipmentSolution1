using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.MailCarrierViewModels;

namespace ShipmentSolution.Services.Core
{
    public class MailCarrierService : IMailCarrierService
    {
        private readonly ApplicationDbContext context;

        public MailCarrierService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<MailCarrierViewModel>> GetAllAsync()
        {
            try
            {
                return await context.MailCarriers
                    .Where(mc => !mc.IsDeleted)
                    .Select(mc => new MailCarrierViewModel
                    {
                        MailCarrierId = mc.MailCarrierId,
                        FullName = mc.FirstName + " " + mc.LastName,
                        Email = mc.Email,
                        PhoneNumber = mc.PhoneNumber,
                        Status = mc.Status,
                        CurrentLocation = mc.CurrentLocation
                    }).ToListAsync();
            }
            catch
            {
                return Enumerable.Empty<MailCarrierViewModel>();
            }
        }

        public async Task<MailCarrierEditViewModel> GetForEditAsync(int id)
        {
            var mc = await context.MailCarriers.FindAsync(id);

            if (mc == null)
                throw new Exception("Mail carrier not found.");

            return new MailCarrierEditViewModel
            {
                MailCarrierId = mc.MailCarrierId,
                FullName = mc.FirstName + " " + mc.LastName,
                Email = mc.Email,
                PhoneNumber = mc.PhoneNumber,
                Status = mc.Status,
                CurrentLocation = mc.CurrentLocation,
                StatusOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "-- Select Status --", Value = "" },
                    new SelectListItem { Text = "Available", Value = "Available" },
                    new SelectListItem { Text = "On Break", Value = "On Break" },
                    new SelectListItem { Text = "On a Delivery", Value = "On a Delivery" }
                }
            };
        }

        public async Task<MailCarrierDeleteViewModel> GetByIdAsync(int id)
        {
            var mc = await context.MailCarriers.FindAsync(id);

            if (mc == null)
                throw new Exception("Mail carrier not found.");

            return new MailCarrierDeleteViewModel
            {
                MailCarrierId = mc.MailCarrierId,
                FullName = mc.FirstName + " " + mc.LastName,
                Email = mc.Email,
                PhoneNumber = mc.PhoneNumber,
                Status = mc.Status,
                CurrentLocation = mc.CurrentLocation
            };
        }

        public async Task CreateAsync(MailCarrierCreateViewModel model)
        {
            try
            {
                var names = model.FullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

                var mc = new Data.Models.MailCarrier
                {
                    FirstName = names.Length > 0 ? names[0] : string.Empty,
                    LastName = names.Length > 1 ? names[1] : string.Empty,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Status = model.Status,
                    CurrentLocation = model.CurrentLocation
                };

                context.MailCarriers.Add(mc);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Failed to create MailCarrier.");
            }
        }

        public async Task EditAsync(MailCarrierEditViewModel model)
        {
            try
            {
                var mc = await context.MailCarriers.FindAsync(model.MailCarrierId);

                if (mc != null)
                {
                    var names = model.FullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                    mc.FirstName = names.Length > 0 ? names[0] : string.Empty;
                    mc.LastName = names.Length > 1 ? names[1] : string.Empty;

                    mc.Email = model.Email;
                    mc.PhoneNumber = model.PhoneNumber;
                    mc.Status = model.Status;
                    mc.CurrentLocation = model.CurrentLocation;

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to update MailCarrier.");
            }
        }

        public async Task SoftDeleteAsync(int id)
        {
            try
            {
                var mc = await context.MailCarriers.FindAsync(id);
                if (mc != null)
                {
                    mc.IsDeleted = true;
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to delete MailCarrier.");
            }
        }

        public async Task<PaginatedList<MailCarrierViewModel>> GetPaginatedAsync(int pageIndex, int pageSize, string? searchTerm, string? statusFilter)
        {
            try
            {
                var query = context.MailCarriers
                    .Where(mc => !mc.IsDeleted);

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(mc =>
                        mc.FirstName.ToLower().Contains(searchTerm) ||
                        mc.LastName.ToLower().Contains(searchTerm));
                }

                if (!string.IsNullOrEmpty(statusFilter))
                {
                    query = query.Where(mc => mc.Status == statusFilter);
                }

                var totalItems = await query.CountAsync();

                var carriers = await query
                    .OrderBy(mc => mc.FirstName)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Select(mc => new MailCarrierViewModel
                    {
                        MailCarrierId = mc.MailCarrierId,
                        FullName = mc.FirstName + " " + mc.LastName,
                        Email = mc.Email,
                        PhoneNumber = mc.PhoneNumber,
                        Status = mc.Status,
                        CurrentLocation = mc.CurrentLocation
                    })
                    .ToListAsync();

                return new PaginatedList<MailCarrierViewModel>
                {
                    Items = carriers,
                    PageIndex = pageIndex,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
                };
            }
            catch (Exception)
            {
                throw new Exception("Failed to retrieve mail carriers.");
            }
        }

    }
}
