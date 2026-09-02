using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductOwnerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Trucks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Tablets",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "SpareAccessorTransps",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "SpareAccessorKomps",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "SmartPhones",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "NoteBooks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Motorbikes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Cottages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "CommercialRealEstates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Cars",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Apartments",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Tablets");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "SpareAccessorTransps");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "SpareAccessorKomps");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "SmartPhones");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "NoteBooks");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Motorbikes");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Cottages");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "CommercialRealEstates");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Apartments");
        }
    }
}
