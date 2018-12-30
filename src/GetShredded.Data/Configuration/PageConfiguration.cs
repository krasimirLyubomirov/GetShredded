using GetShredded.Common;
using GetShredded.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetShredded.Data.Configuration
{
    public class PageConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired(false)
                .HasMaxLength(GlobalConstants.TitleLength);

            builder.Property(x => x.Content)
                .IsRequired()
                .HasMaxLength(GlobalConstants.PageContentLength);

            builder.HasOne(x => x.GetShreddedUser)
                .WithMany(x => x.Pages)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.CreatedOn)
                .IsRequired();

            builder.Ignore(x => x.Length);
        }
    }
}
