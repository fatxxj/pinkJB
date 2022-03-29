using Microsoft.EntityFrameworkCore.Migrations;

namespace pinkJB_core.Migrations
{
    public partial class Zaliha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductCategory",
                table: "Products",
                newName: "amountLeft");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "amountLeft",
                table: "Products",
                newName: "ProductCategory");
        }
    }
}
