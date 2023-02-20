using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsBook.Migrations
{
    /// <inheritdoc />
    public partial class Picture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteNews_News_NewsId",
                table: "FavouriteNews");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteNews_Users_UserId",
                table: "FavouriteNews");

            migrationBuilder.DropIndex(
                name: "IX_FavouriteNews_UserId",
                table: "FavouriteNews");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Profile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pictures_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteNews_UserId_NewsId",
                table: "FavouriteNews",
                columns: new[] { "UserId", "NewsId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_UserId",
                table: "Pictures",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteNews_News_NewsId",
                table: "FavouriteNews",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteNews_Users_UserId",
                table: "FavouriteNews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteNews_News_NewsId",
                table: "FavouriteNews");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteNews_Users_UserId",
                table: "FavouriteNews");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_FavouriteNews_UserId_NewsId",
                table: "FavouriteNews");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteNews_UserId",
                table: "FavouriteNews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteNews_News_NewsId",
                table: "FavouriteNews",
                column: "NewsId",
                principalTable: "News",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteNews_Users_UserId",
                table: "FavouriteNews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
