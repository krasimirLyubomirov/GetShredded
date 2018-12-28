using GetShredded.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetShredded.Data.Configuration
{
    public class DiaryRatingConfiguration : IEntityTypeConfiguration<DiaryRating>
    {
        public void Configure(EntityTypeBuilder<DiaryRating> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Rating).IsRequired();

            builder.HasOne(x => x.GetShreddedUser)
                .WithMany(x => x.DiaryRatings)
                .HasForeignKey(x => x.GetShreddedUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
