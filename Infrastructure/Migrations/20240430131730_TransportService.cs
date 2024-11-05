using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TransportService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubCategoryId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    YearOfIssue = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Engine = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    Gearbox = table.Column<string>(type: "text", nullable: false),
                    DriverUnit = table.Column<string>(type: "text", nullable: false),
                    EngineCapacity = table.Column<string>(type: "text", nullable: false),
                    Mileage = table.Column<string>(type: "text", nullable: false),
                    ManufacturerState = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    FuelPer100km = table.Column<string>(type: "text", nullable: false),
                    NumberOfSeats = table.Column<string>(type: "text", nullable: false),
                    Condition = table.Column<string>(type: "text", nullable: false),
                    AccelerTo100km = table.Column<string>(type: "text", nullable: true),
                    TrunkVolume = table.Column<string>(type: "text", nullable: true),
                    Clearance = table.Column<string>(type: "text", nullable: true),
                    SteeringWheel = table.Column<string>(type: "text", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: true),
                    PowerSteering = table.Column<string>(type: "text", nullable: true),
                    InteriorColor = table.Column<string>(type: "text", nullable: true),
                    SettingsMemory = table.Column<string>(type: "text", nullable: true),
                    MultimediaAndNavigation = table.Column<string>(type: "text", nullable: true),
                    ClimateControl = table.Column<string>(type: "text", nullable: true),
                    DrivingAssistance = table.Column<string>(type: "text", nullable: true),
                    AntiTheftSystem = table.Column<string>(type: "text", nullable: true),
                    Airbags = table.Column<string>(type: "text", nullable: true),
                    Heating = table.Column<string>(type: "text", nullable: true),
                    TiresAndWheels = table.Column<string>(type: "text", nullable: true),
                    Headlights = table.Column<string>(type: "text", nullable: true),
                    AudioSystems = table.Column<string>(type: "text", nullable: true),
                    ElectricLifts = table.Column<string>(type: "text", nullable: true),
                    ElectricDrive = table.Column<string>(type: "text", nullable: true),
                    ActiveSafety = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motorbikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubCategoryId = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    YearOfIssue = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ManufacturerCountry = table.Column<string>(type: "text", nullable: true),
                    EngineType = table.Column<string>(type: "text", nullable: true),
                    EngineCapacity = table.Column<string>(type: "text", nullable: true),
                    Power = table.Column<string>(type: "text", nullable: true),
                    FuelSupply = table.Column<string>(type: "text", nullable: true),
                    NumberOfCycles = table.Column<string>(type: "text", nullable: true),
                    GearBox = table.Column<string>(type: "text", nullable: true),
                    Mileage = table.Column<string>(type: "text", nullable: true),
                    Passengers = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motorbikes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpareAccessorKomps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubCategoryId = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpareAccessorKomps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpareAccessorTransps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubCategoryId = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpareAccessorTransps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubCategoryId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    PriceDiscount = table.Column<decimal>(type: "numeric", nullable: false),
                    Brand = table.Column<string>(type: "text", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    YearOfIssue = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BodyType = table.Column<string>(type: "text", nullable: true),
                    Power = table.Column<string>(type: "text", nullable: false),
                    EngineType = table.Column<string>(type: "text", nullable: false),
                    EngineCapacity = table.Column<string>(type: "text", nullable: false),
                    EnvironmentalClass = table.Column<string>(type: "text", nullable: true),
                    Transmission = table.Column<string>(type: "text", nullable: false),
                    WheelFormula = table.Column<string>(type: "text", nullable: true),
                    LoadCapacity = table.Column<string>(type: "text", nullable: false),
                    PermittedMaximumWeight = table.Column<string>(type: "text", nullable: false),
                    Mileage = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trucks", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Motorbikes");

            migrationBuilder.DropTable(
                name: "SpareAccessorKomps");

            migrationBuilder.DropTable(
                name: "SpareAccessorTransps");

            migrationBuilder.DropTable(
                name: "Trucks");
        }
    }
}
