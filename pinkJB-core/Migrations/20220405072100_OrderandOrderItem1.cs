using Microsoft.EntityFrameworkCore.Migrations;

namespace pinkJB_core.Migrations
{
    public partial class OrderandOrderItem1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Zaliha",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zaliha",
                table: "Products");
        }
    }
}
