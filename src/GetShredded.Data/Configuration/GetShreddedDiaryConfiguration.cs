using GetShredded.Data.Constants;
using GetShredded.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetShredded.Data.Configuration
{
    public class GetShreddedDiaryConfiguration : IEntityTypeConfiguration<GetShreddedDiary>
    {
        public void Configure(EntityTypeBuilder<GetShreddedDiary> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedOn).IsRequired();

            builder.Property(x => x.LastEditedOn).IsRequired();

            builder.Property(x => x.ImageUrl).IsRequired(false);

            builder.HasOne(x => x.Type)
                .WithMany(x => x.Diaries)
                .HasForeignKey(x => x.DiaryTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Comments)
                .WithOne(x => x.GetShreddedDiary)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.Summary)
                .IsRequired(false)
                .HasMaxLength(ConfigurationConstants.GetShreddedDiarySummary);

            builder.HasOne(x => x.User)
                .WithMany(x => x.GetShreddedDiaries)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.Title).IsRequired()
                .HasMaxLength(ConfigurationConstants.TitleLength);

            builder.Ignore(x => x.Rating);

            builder.Ignore(x => x.Length);
        }
    }
}
