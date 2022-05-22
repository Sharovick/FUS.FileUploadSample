using FUS.Core.Entities;
using FUS.Infrastrucure.Interfaces;
using FUS.Infrastrucure.Repository;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FUS.Infrastrucure.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IGenericRepository<User> _userRepository;
        public UserService(ILogger<UserService> logger,
                            IGenericRepository<User> userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<bool> CheckIfUserExistsAndActive(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user != null;
        }
    }
}
