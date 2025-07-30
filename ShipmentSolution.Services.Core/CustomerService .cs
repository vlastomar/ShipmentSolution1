using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.CustomerViewModels;
using System.Security.Claims;

namespace ShipmentSolution.Services.Core
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext context;

        public CustomerService(ApplicationDbContext context)
        {
            this.context = context;
        }

        private IEnumerable<SelectListItem> GetShippingMethodOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "-- Select Method --", Value = "" },
                new SelectListItem { Text = "Standard", Value = "Standard" },
                new SelectListItem { Text = "Express", Value = "Express" },
                new SelectListItem { Text = "Overnight", Value = "Overnight" }
            };
        }

        public async Task<IEnumerable<CustomerViewModel>> GetAllAsync()
        {
            return await context.Customers
                .Where(c => !c.IsDeleted)
                .Select(c => new CustomerViewModel
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email
                })
                .ToListAsync();
        }

        public async Task CreateAsync(CustomerCreateViewModel model, string userId)
        {
            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                PreferredShippingMethod = model.PreferredShippingMethod,
                ShippingCostThreshold = model.ShippingCostThreshold,
                CreatedByUserId = userId
            };

            context.Customers.Add(customer);
            await context.SaveChangesAsync();
        }

        public async Task<CustomerEditViewModel?> GetForEditAsync(int id, string userId, ClaimsPrincipal user)
        {
            var customer = await context.Customers.FindAsync(id);
            if (customer == null || customer.IsDeleted)
                return null; // Already nullable

            if (!user.IsInRole("Administrator") && customer.CreatedByUserId != userId)
                return null;

            return new CustomerEditViewModel
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                City = customer.City,
                State = customer.State,
                ZipCode = customer.ZipCode,
                PreferredShippingMethod = customer.PreferredShippingMethod,
                ShippingCostThreshold = customer.ShippingCostThreshold,
                ShippingMethodOptions = GetShippingMethodOptions()
            };
        }


        public async Task<bool> EditAsync(CustomerEditViewModel model, string userId, ClaimsPrincipal user)
        {
            var customer = await context.Customers.FindAsync(model.CustomerId);

            if (customer == null || customer.IsDeleted)
                return false;

            if (!user.IsInRole("Administrator") && customer.CreatedByUserId != userId)
                return false;

            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Email = model.Email;
            customer.PhoneNumber = model.PhoneNumber;
            customer.City = model.City;
            customer.State = model.State;
            customer.ZipCode = model.ZipCode;
            customer.PreferredShippingMethod = model.PreferredShippingMethod;
            customer.ShippingCostThreshold = model.ShippingCostThreshold;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await context.Customers.FindAsync(id);
            if (customer == null || customer.IsDeleted)
                throw new KeyNotFoundException("Customer not found.");

            customer.IsDeleted = true;
            await context.SaveChangesAsync();
        }

        public async Task<CustomerViewModel> GetByIdAsync(int id)
        {
            var entity = await context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == id && !c.IsDeleted);

            if (entity == null)
                throw new KeyNotFoundException("Customer not found.");

            return new CustomerViewModel
            {
                CustomerId = entity.CustomerId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email
            };
        }

        public async Task SoftDeleteAsync(int id)
        {
            var customer = await context.Customers.FindAsync(id);
            if (customer == null || customer.IsDeleted)
                throw new KeyNotFoundException("Customer not found.");

            customer.IsDeleted = true;
            await context.SaveChangesAsync();
        }

        public async Task<PaginatedList<CustomerViewModel>> GetPaginatedAsync(int pageIndex, int pageSize, string? searchTerm, string? userId, bool isAdmin, bool isLoggedIn)
        {
            var query = context.Customers.Where(c => !c.IsDeleted);

            if (!isLoggedIn)
            {
                // Unlogged users see nothing
                query = query.Where(c => false);
            }
            else if (!isAdmin)
            {
                // Registered users see only their records
                query = query.Where(c => c.CreatedByUserId == userId);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c =>
                    c.FirstName.ToLower().Contains(searchTerm.ToLower()) ||
                    c.LastName.ToLower().Contains(searchTerm.ToLower()));
            }

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.FirstName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CustomerViewModel
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email
                })
                .ToListAsync();

            return new PaginatedList<CustomerViewModel>
            {
                Items = items,
                PageIndex = pageIndex,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
        }

        public IEnumerable<SelectListItem> GetShippingMethodOptionsPublic() => GetShippingMethodOptions();
    }
}
