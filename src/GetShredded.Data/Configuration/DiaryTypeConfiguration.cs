using GetShredded.Data.Constants;
using GetShredded.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetShredded.Data.Configuration
{
    public class DiaryTypeConfiguration : IEntityTypeConfiguration<DiaryType>
    {
        public void Configure(EntityTypeBuilder<DiaryType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(ConfigurationConstants.TitleLength);

            builder.HasMany(x => x.Diaries)
                .WithOne(x => x.Type)
                .HasForeignKey(x => x.DiaryTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
