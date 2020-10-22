using Microsoft.EntityFrameworkCore.Migrations;

namespace aspnetcore_backgroundservice.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreItems",
                columns: table => new
                {
                    StoreItemId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Stock = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreItems", x => x.StoreItemId);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    StoreId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StoreWorth = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.StoreId);
                });

            migrationBuilder.InsertData(
                table: "StoreItems",
                columns: new[] { "StoreItemId", "Name", "Price", "Stock" },
                values: new object[] { 1, "Book", 50f, 10 });

            migrationBuilder.InsertData(
                table: "StoreItems",
                columns: new[] { "StoreItemId", "Name", "Price", "Stock" },
                values: new object[] { 2, "Pan", 340.5f, 20 });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "StoreId", "StoreWorth" },
                values: new object[] { 1, 0f });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreItems");

            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
