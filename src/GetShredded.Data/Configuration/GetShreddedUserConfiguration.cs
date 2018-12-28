using GetShredded.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetShredded.Data.Configuration
{
    public class GetShreddedUserConfiguration : IEntityTypeConfiguration<GetShreddedUser>
    {
        public void Configure(EntityTypeBuilder<GetShreddedUser> builder)
        {
            builder.HasMany(x => x.Notifications)
                .WithOne(x => x.GetShreddedUser)
                .HasForeignKey(x => x.GetShreddedUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
