using GetShredded.Data.Constants;
using GetShredded.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetShredded.Data.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Message)
                .IsRequired(true)
                .HasMaxLength(ConfigurationConstants.CommentContentLength);

            builder.Property(x => x.CommentedOn).IsRequired(true);

            builder.HasOne(x => x.GetShreddedUser)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.GetShreddedUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
