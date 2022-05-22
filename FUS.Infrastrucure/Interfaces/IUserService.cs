using System.Threading.Tasks;

namespace FUS.Infrastrucure.Interfaces
{
    public interface IUserService
    {
        Task<bool> CheckIfUserExistsAndActive(int userId);
    }
}
