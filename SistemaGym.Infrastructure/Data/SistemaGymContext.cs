using Microsoft.EntityFrameworkCore;
using SistemaGym.Core.Entities;
using System.Reflection;

namespace SistemaGym.Infrastructure.Data
{
    public partial class SistemaGymContext : DbContext
    {
        public SistemaGymContext()
        {
        }

        public SistemaGymContext(DbContextOptions<SistemaGymContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}