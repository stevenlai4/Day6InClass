using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Day4InClass.Data.Migrations
{
    public partial class InitialToDoSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    IsComplete = table.Column<bool>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ToDos",
                columns: new[] { "Id", "CreatedOn", "Description", "IsComplete", "Priority" },
                values: new object[] { 1, new DateTime(2020, 12, 4, 10, 41, 9, 866, DateTimeKind.Local).AddTicks(5807), "Bake Cookies", false, 1 });

            migrationBuilder.InsertData(
                table: "ToDos",
                columns: new[] { "Id", "CreatedOn", "Description", "IsComplete", "Priority" },
                values: new object[] { 2, new DateTime(2020, 12, 4, 10, 41, 9, 874, DateTimeKind.Local).AddTicks(2936), "Eat the cookies", false, 1 });

            migrationBuilder.InsertData(
                table: "ToDos",
                columns: new[] { "Id", "CreatedOn", "Description", "IsComplete", "Priority" },
                values: new object[] { 3, new DateTime(2020, 12, 4, 10, 41, 9, 874, DateTimeKind.Local).AddTicks(3010), "Run around the block", false, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDos");
        }
    }
}
