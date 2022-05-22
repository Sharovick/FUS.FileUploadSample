using FUS.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FUS.Infrastrucure.Interfaces
{
    public interface IFileService
    {
        Task<int> InserFile(int customerId, Document file);
    }
}
