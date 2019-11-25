using Microsoft.EntityFrameworkCore.Migrations;

namespace RazorPagesMovie.Migrations
{
    public partial class AddRatingColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AlterColumn<decimal>(
            //     name: "Price",
            //     table: "Movies",
            //     type: "decimal(18,2)",
            //     nullable: false,
            //     oldClrType: typeof(decimal),
            //     oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Rating",
                table: "Movies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Movies");

            // migrationBuilder.AlterColumn<decimal>(
            //     name: "Price",
            //     table: "Movies",
            //     type: "TEXT",
            //     nullable: false,
            //     oldClrType: typeof(decimal),
            //     oldType: "decimal(18,2)");
        }
    }
}
