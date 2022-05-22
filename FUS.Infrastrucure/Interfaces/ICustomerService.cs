using System.Threading.Tasks;

namespace FUS.Infrastrucure.Interfaces
{
    public interface ICustomerService
    {
        Task<bool> CheckIfCustomerExistsAndActive(int customerId);
    }
}
