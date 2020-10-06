using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Peddle.Offer.Domain.Entities;

namespace Peddle.Offer.Infrastructure.Persistence
{
    public class ApplicationDbContext :DbContext
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<InstantOffer> InstantOffers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}