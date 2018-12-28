using GetShredded.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GetShredded.Data.Configuration
{
    public class GetShreddedUserDiaryConfiguration : IEntityTypeConfiguration<GetShreddedUserDiary>
    {
        public void Configure(EntityTypeBuilder<GetShreddedUserDiary> builder)
        {
            builder.HasKey(x => new { x.GetShreddedUserId, x.GetShreddedDiaryId });
        }
    }
}
