using FUS.Core.Entities;
using FUS.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FUS.Core.EFCore
{
    public class FUSContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<FileType> FileTypes { get; set; }
        public DbSet<File> Files { get; set; }

        public FUSContext(DbContextOptions<FUSContext> options) : base(options) { }

        public void SeedData()
        {
            var user = new User
            {
                Id = 1,
                IsActive = true,
                Name = "User 1"
            };
            Users.Add(user);
            var customer = new Customer
            {
                Id = 1,
                IsActive = true,
                Name = "Customer 1",
                UserId = 1
            };
            Customers.Add(customer);

            var filetypes = new List<FileType>
                {
                    new FileType
                    {
                        Id = 1,
                        Name = nameof(FileTypeEnum.PassportScan)
                    },
                    new FileType
                    {
                        Id = 2,
                        Name = nameof(FileTypeEnum.GdprBaseAgreement)
                    },
                    new FileType
                    {
                        Id = 3,
                        Name = nameof(FileTypeEnum.GdprAnexOneAgreement)
                    },
                    new FileType
                    {
                        Id = 4,
                        Name = nameof(FileTypeEnum.PartnershipAgreement)
                    },
                    new FileType
                    {
                        Id = 5,
                        Name = nameof(FileTypeEnum.PolicyDocument)
                    }
                };
            FileTypes.AddRange(filetypes);
            SaveChanges();
        }
    }
}
