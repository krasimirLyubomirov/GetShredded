using GetShredded.Data.Constants;
using GetShredded.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetShredded.Data.Configuration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Seen)
                .IsRequired(true);

            builder.Property(x => x.Message)
                .IsRequired(true)
                .HasMaxLength(ConfigurationConstants.NotificationContentLength);

            builder.Property(x => x.UpdatedDiaryId)
                .IsRequired();
        }
    }
}
