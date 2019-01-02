using GetShredded.Data.Configuration;
using GetShredded.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GetShredded.Data
{
    public class GetShreddedContext : IdentityDbContext<GetShreddedUser>
    {
        public GetShreddedContext(DbContextOptions<GetShreddedContext> options)
            : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<DiaryRating> DiaryRatings { get; set; }

        public DbSet<DiaryType> DiaryTypes { get; set; }

        public DbSet<GetShreddedDiary> GetShreddedDiaries { get; set; }

        public DbSet<GetShreddedRating> GetShreddedRatings { get; set; }

        public DbSet<GetShreddedUserDiary> GetShreddedUserDiaries { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Page> Pages { get; set; }

        public DbSet<DatabaseLog> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new DiaryRatingConfiguration());
            builder.ApplyConfiguration(new DiaryTypeConfiguration());
            builder.ApplyConfiguration(new GetShreddedDiaryConfiguration());
            builder.ApplyConfiguration(new GetShreddedRatingConfiguration());
            builder.ApplyConfiguration(new GetShreddedUserConfiguration());
            builder.ApplyConfiguration(new GetShreddedUserDiaryConfiguration());
            builder.ApplyConfiguration(new MessageConfiguration());
            builder.ApplyConfiguration(new NotificationConfiguration());
            builder.ApplyConfiguration(new PageConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
