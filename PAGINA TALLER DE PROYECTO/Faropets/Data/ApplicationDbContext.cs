using Faropets.Areas.Clientes.Models;
using Faropets.Areas.Users.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Faropets.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        static DbContextOptions<ApplicationDbContext> _options;
        public ApplicationDbContext() : base(_options)
        {

        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            _options = options;
        }
        public DbSet<TUsers> TUsers { get; set; }
        public DbSet<TClients> TClients { get; set; }
        public DbSet<TReports_Clients> TReports_Clients { get; set; }
        public DbSet<TPayments_clients> TPayments_clients { get; set; }
        public DbSet<TClients_interests> TClientes_Interests { get; set; }
        public DbSet<TClientes_interests_reports> TClientes_Interests_reports { get; set; }
        public DbSet<TPaymkents_Reports_Clients_Interests> TPayments_Reports_Clientes_Interests { get; set; }



    }
}
