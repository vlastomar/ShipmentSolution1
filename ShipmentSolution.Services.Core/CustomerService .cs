using Microsoft.EntityFrameworkCore;
using ShipmentSolution.Data;
using ShipmentSolution.Data.Models;
using ShipmentSolution.Services.Core.Interfaces;
using ShipmentSolution.Web.ViewModels.Common;
using ShipmentSolution.Web.ViewModels.CustomerViewModels;

namespace ShipmentSolution.Services.Core
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext context;

        public CustomerService(ApplicationDbContext context)
        {
            this.context = context;
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

        public async Task CreateAsync(CustomerCreateViewModel model)
        {
            try
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
                    ShippingCostThreshold = model.ShippingCostThreshold
                };

                context.Customers.Add(customer);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the customer.", ex);
            }
        }

        public async Task<CustomerEditViewModel> GetForEditAsync(int id)
        {
            var customer = await context.Customers.FindAsync(id);
            if (customer == null || customer.IsDeleted)
            {
                throw new KeyNotFoundException("Customer not found.");
            }

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
                ShippingCostThreshold = customer.ShippingCostThreshold
            };
        }

        public async Task EditAsync(CustomerEditViewModel model)
        {
            try
            {
                var customer = await context.Customers.FindAsync(model.CustomerId);
                if (customer == null || customer.IsDeleted)
                {
                    throw new KeyNotFoundException("Customer not found.");
                }

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
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while editing the customer.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var customer = await context.Customers.FindAsync(id);
                if (customer == null || customer.IsDeleted)
                {
                    throw new KeyNotFoundException("Customer not found.");
                }

                customer.IsDeleted = true;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while deleting the customer.", ex);
            }
        }

        public async Task<CustomerViewModel> GetByIdAsync(int id)
        {
            var entity = await context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == id && !c.IsDeleted);

            if (entity == null)
            {
                throw new KeyNotFoundException("Customer not found.");
            }

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
            try
            {
                var customer = await context.Customers.FindAsync(id);
                if (customer == null || customer.IsDeleted)
                {
                    throw new KeyNotFoundException("Customer not found.");
                }

                customer.IsDeleted = true;
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while soft deleting the customer.", ex);
            }
        }

        public async Task<PaginatedList<CustomerViewModel>> GetPaginatedAsync(int pageIndex, int pageSize, string? searchTerm)
        {
            var query = context.Customers
                .Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c =>
                    c.FirstName.Contains(searchTerm) ||
                    c.LastName.Contains(searchTerm) ||
                    c.Email.Contains(searchTerm));
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
    }
}
