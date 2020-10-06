using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class InstantOfferConfiguration : IEntityTypeConfiguration<InstantOffer>
    {
        public void Configure(EntityTypeBuilder<InstantOffer> builder)
        {
            builder.Property(t => t.Id)
                .IsRequired();
        }
    }
}