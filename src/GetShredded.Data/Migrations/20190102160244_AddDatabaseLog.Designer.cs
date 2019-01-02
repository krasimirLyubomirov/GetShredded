﻿// <auto-generated />
using System;
using GetShredded.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GetShredded.Data.Migrations
{
    [DbContext(typeof(GetShreddedContext))]
    [Migration("20190102160244_AddDatabaseLog")]
    partial class AddDatabaseLog
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GetShredded.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CommentedOn");

                    b.Property<int?>("GetShreddedDiaryId");

                    b.Property<string>("GetShreddedUserId");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.HasKey("Id");

                    b.HasIndex("GetShreddedDiaryId");

                    b.HasIndex("GetShreddedUserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("GetShredded.Models.DatabaseLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<bool>("Handled");

                    b.Property<string>("LogType")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("GetShredded.Models.DiaryRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GetShreddedUserId");

                    b.Property<double>("Rating");

                    b.HasKey("Id");

                    b.HasIndex("GetShreddedUserId");

                    b.ToTable("DiaryRatings");
                });

            modelBuilder.Entity("GetShredded.Models.DiaryType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("DiaryTypes");
                });

            modelBuilder.Entity("GetShredded.Models.GetShreddedDiary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("DiaryTypeId");

                    b.Property<string>("ImageUrl");

                    b.Property<DateTime>("LastEditedOn");

                    b.Property<string>("Summary")
                        .HasMaxLength(200);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("DiaryTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("GetShreddedDiaries");
                });

            modelBuilder.Entity("GetShredded.Models.GetShreddedRating", b =>
                {
                    b.Property<int>("GetShreddedDiaryId");

                    b.Property<int>("DiaryRatingId");

                    b.HasKey("GetShreddedDiaryId", "DiaryRatingId");

                    b.HasIndex("DiaryRatingId");

                    b.ToTable("GetShreddedRatings");
                });

            modelBuilder.Entity("GetShredded.Models.GetShreddedUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("GetShredded.Models.GetShreddedUserDiary", b =>
                {
                    b.Property<string>("GetShreddedUserId");

                    b.Property<int>("GetShreddedDiaryId");

                    b.HasKey("GetShreddedUserId", "GetShreddedDiaryId");

                    b.HasIndex("GetShreddedDiaryId");

                    b.ToTable("GetShreddedUserDiaries");
                });

            modelBuilder.Entity("GetShredded.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("IsReaded");

                    b.Property<string>("ReceiverId");

                    b.Property<DateTime>("SendOn");

                    b.Property<string>("SenderId");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(400);

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("GetShredded.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GetShreddedUserId");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<bool>("Seen");

                    b.Property<int>("UpdatedDiaryId");

                    b.HasKey("Id");

                    b.HasIndex("GetShreddedUserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("GetShredded.Models.Page", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(3000);

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("GetShreddedDiaryId");

                    b.Property<string>("Title")
                        .HasMaxLength(100);

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("GetShreddedDiaryId");

                    b.HasIndex("UserId");

                    b.ToTable("Pages");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("GetShredded.Models.Comment", b =>
                {
                    b.HasOne("GetShredded.Models.GetShreddedDiary", "GetShreddedDiary")
                        .WithMany("Comments")
                        .HasForeignKey("GetShreddedDiaryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("GetShredded.Models.GetShreddedUser", "GetShreddedUser")
                        .WithMany("Comments")
                        .HasForeignKey("GetShreddedUserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("GetShredded.Models.DiaryRating", b =>
                {
                    b.HasOne("GetShredded.Models.GetShreddedUser", "GetShreddedUser")
                        .WithMany("DiaryRatings")
                        .HasForeignKey("GetShreddedUserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("GetShredded.Models.GetShreddedDiary", b =>
                {
                    b.HasOne("GetShredded.Models.DiaryType", "Type")
                        .WithMany("Diaries")
                        .HasForeignKey("DiaryTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("GetShredded.Models.GetShreddedUser", "User")
                        .WithMany("GetShreddedDiaries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("GetShredded.Models.GetShreddedRating", b =>
                {
                    b.HasOne("GetShredded.Models.DiaryRating", "DiaryRating")
                        .WithMany("GetShreddedRatings")
                        .HasForeignKey("DiaryRatingId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GetShredded.Models.GetShreddedDiary", "GetShreddedDiary")
                        .WithMany("Ratings")
                        .HasForeignKey("GetShreddedDiaryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GetShredded.Models.GetShreddedUserDiary", b =>
                {
                    b.HasOne("GetShredded.Models.GetShreddedDiary", "GetShreddedDiary")
                        .WithMany("Followers")
                        .HasForeignKey("GetShreddedDiaryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GetShredded.Models.GetShreddedUser", "GetShreddedUser")
                        .WithMany("FollowedDiaries")
                        .HasForeignKey("GetShreddedUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GetShredded.Models.Message", b =>
                {
                    b.HasOne("GetShredded.Models.GetShreddedUser", "Receiver")
                        .WithMany("ReceivedMessages")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("GetShredded.Models.GetShreddedUser", "Sender")
                        .WithMany("SendMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GetShredded.Models.Notification", b =>
                {
                    b.HasOne("GetShredded.Models.GetShreddedUser", "GetShreddedUser")
                        .WithMany("Notifications")
                        .HasForeignKey("GetShreddedUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GetShredded.Models.Page", b =>
                {
                    b.HasOne("GetShredded.Models.GetShreddedDiary", "GetShreddedDiary")
                        .WithMany("Pages")
                        .HasForeignKey("GetShreddedDiaryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GetShredded.Models.GetShreddedUser", "GetShreddedUser")
                        .WithMany("Pages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("GetShredded.Models.GetShreddedUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("GetShredded.Models.GetShreddedUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GetShredded.Models.GetShreddedUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("GetShredded.Models.GetShreddedUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
