using FUS.Core.Entities;
using FUS.Infrastrucure.Interfaces;
using FUS.Infrastrucure.Repository;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FUS.Infrastrucure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly IGenericRepository<Customer> _customerRepository;
        public CustomerService(ILogger<CustomerService> logger,
                            IGenericRepository<Customer> customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
        }

        public async Task<bool> CheckIfCustomerExistsAndActive(int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            return customer != null;
        }
    }
}
