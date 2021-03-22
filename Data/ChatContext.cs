using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TechnicalSupport.Models;
using Microsoft.Extensions.Configuration;
using System.Configuration;

#nullable disable

namespace TechnicalSupport.Data
{
    public partial class ChatContext : DbContext
    {


        public ChatContext(DbContextOptions<ChatContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<User> Users { get; set; }       
        public virtual DbSet<Dialog> Dialogs { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<WorkTime> WorkTimes { get; set; }


        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                                      .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                      .AddJsonFile("appsettings.json")
                                      .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

    }
}
