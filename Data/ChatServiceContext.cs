using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Models.ServiceModels;

namespace TechnicalSupport.Data
{
    public partial class ChatServiceContext : DbContext
    {

        public  DbSet<AdminToken> AdminTokens { get; set; }
        public  DbSet<TraceLog> TraceLogs { get; set; }
        public  DbSet<ErrorLog> TechnicalLogs { get; set; }

        public ChatServiceContext(DbContextOptions<ChatServiceContext> options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                      .AddJsonFile("appsettings.json")
                                      .Build();
                optionsBuilder.UseSqlServer(config.GetConnectionString("ServiceDbConnection"));
            }
            
        }
    }
}
