using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GetShredded.Data.Migrations
{
    public partial class AddedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiaryRatings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Rating = table.Column<double>(nullable: false),
                    GetShreddedUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiaryRatings_AspNetUsers_GetShreddedUserId",
                        column: x => x.GetShreddedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DiaryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SendOn = table.Column<DateTime>(nullable: false),
                    SenderId = table.Column<string>(nullable: true),
                    ReceiverId = table.Column<string>(nullable: true),
                    Text = table.Column<string>(maxLength: 400, nullable: false),
                    IsReaded = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedDiaryId = table.Column<int>(nullable: false),
                    Seen = table.Column<bool>(nullable: false),
                    Message = table.Column<string>(maxLength: 200, nullable: false),
                    GetShreddedUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_GetShreddedUserId",
                        column: x => x.GetShreddedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GetShreddedDiaries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(maxLength: 200, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastEditedOn = table.Column<DateTime>(nullable: false),
                    DiaryTypeId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetShreddedDiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GetShreddedDiaries_DiaryTypes_DiaryTypeId",
                        column: x => x.DiaryTypeId,
                        principalTable: "DiaryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GetShreddedDiaries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GetShreddedUserId = table.Column<string>(nullable: true),
                    GetShreddedDiaryId = table.Column<int>(nullable: true),
                    Message = table.Column<string>(maxLength: 300, nullable: false),
                    CommentedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_GetShreddedDiaries_GetShreddedDiaryId",
                        column: x => x.GetShreddedDiaryId,
                        principalTable: "GetShreddedDiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_GetShreddedUserId",
                        column: x => x.GetShreddedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "GetShreddedRatings",
                columns: table => new
                {
                    DiaryRatingId = table.Column<int>(nullable: false),
                    GetShreddedDiaryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetShreddedRatings", x => new { x.GetShreddedDiaryId, x.DiaryRatingId });
                    table.ForeignKey(
                        name: "FK_GetShreddedRatings_DiaryRatings_DiaryRatingId",
                        column: x => x.DiaryRatingId,
                        principalTable: "DiaryRatings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GetShreddedRatings_GetShreddedDiaries_GetShreddedDiaryId",
                        column: x => x.GetShreddedDiaryId,
                        principalTable: "GetShreddedDiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GetShreddedUserDiaries",
                columns: table => new
                {
                    GetShreddedUserId = table.Column<string>(nullable: false),
                    GetShreddedDiaryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetShreddedUserDiaries", x => new { x.GetShreddedUserId, x.GetShreddedDiaryId });
                    table.ForeignKey(
                        name: "FK_GetShreddedUserDiaries_GetShreddedDiaries_GetShreddedDiaryId",
                        column: x => x.GetShreddedDiaryId,
                        principalTable: "GetShreddedDiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GetShreddedUserDiaries_AspNetUsers_GetShreddedUserId",
                        column: x => x.GetShreddedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    GetShreddedDiaryId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(maxLength: 3000, nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_GetShreddedDiaries_GetShreddedDiaryId",
                        column: x => x.GetShreddedDiaryId,
                        principalTable: "GetShreddedDiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_GetShreddedDiaryId",
                table: "Comments",
                column: "GetShreddedDiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_GetShreddedUserId",
                table: "Comments",
                column: "GetShreddedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiaryRatings_GetShreddedUserId",
                table: "DiaryRatings",
                column: "GetShreddedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GetShreddedDiaries_DiaryTypeId",
                table: "GetShreddedDiaries",
                column: "DiaryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GetShreddedDiaries_UserId",
                table: "GetShreddedDiaries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GetShreddedRatings_DiaryRatingId",
                table: "GetShreddedRatings",
                column: "DiaryRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_GetShreddedUserDiaries_GetShreddedDiaryId",
                table: "GetShreddedUserDiaries",
                column: "GetShreddedDiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_GetShreddedUserId",
                table: "Notifications",
                column: "GetShreddedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_GetShreddedDiaryId",
                table: "Pages",
                column: "GetShreddedDiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_UserId",
                table: "Pages",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "GetShreddedRatings");

            migrationBuilder.DropTable(
                name: "GetShreddedUserDiaries");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "DiaryRatings");

            migrationBuilder.DropTable(
                name: "GetShreddedDiaries");

            migrationBuilder.DropTable(
                name: "DiaryTypes");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}
