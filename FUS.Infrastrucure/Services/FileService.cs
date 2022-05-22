using FUS.Common.Models;
using FUS.Core.Entities;
using FUS.Infrastrucure.Interfaces;
using FUS.Infrastrucure.Repository;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FUS.Infrastrucure.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly IGenericRepository<File> _fileRepository;
        public FileService(ILogger<FileService> logger,
                            IGenericRepository<File> fileRepository)
        {
            _logger = logger;
            _fileRepository = fileRepository;
        }

        public async Task<int> InserFile(int customerId, Document file)
        {
            var newFile = new File
            {
                Type = file.Type,
                FilePath = file.FilePath,
                CustomerId = customerId
            };
            var inserted = await _fileRepository.InsertAsync(newFile);
            return inserted.Id;
        }
    }
}
