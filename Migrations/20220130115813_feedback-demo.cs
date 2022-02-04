using Microsoft.EntityFrameworkCore.Migrations;

namespace Project40_API_Dot_NET.Migrations
{
    public partial class feedbackdemo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SuperVisorId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FieldName",
                table: "Plant",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Plant",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_SuperVisorId",
                table: "User",
                column: "SuperVisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_SuperVisorId",
                table: "User",
                column: "SuperVisorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_SuperVisorId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_SuperVisorId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SuperVisorId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "FieldName",
                table: "Plant");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Plant");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
