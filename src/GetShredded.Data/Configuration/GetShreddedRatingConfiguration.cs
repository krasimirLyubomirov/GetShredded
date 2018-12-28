using GetShredded.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetShredded.Data.Configuration
{
    public class GetShreddedRatingConfiguration : IEntityTypeConfiguration<GetShreddedRating>
    {
        public void Configure(EntityTypeBuilder<GetShreddedRating> builder)
        {
            builder.HasKey(x => new { x.GetShreddedDiaryId, x.DiaryRatingId });

            builder.HasOne(x => x.GetShreddedDiary)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.GetShreddedDiaryId);

            builder.HasOne(x => x.DiaryRating)
                .WithMany(x => x.GetShreddedRatings)
                .HasForeignKey(x => x.DiaryRatingId);
        }
    }
}
