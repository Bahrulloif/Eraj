using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncEntityTypesWithSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tables are freshly created and empty at this point in the migration history,
            // so a literal USING cast is safe — there is no real data to preserve or corrupt.
            migrationBuilder.Sql("ALTER TABLE \"Trucks\" ALTER COLUMN \"YearOfIssue\" TYPE integer USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Trucks\" ALTER COLUMN \"Transmission\" TYPE integer USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Trucks\" ALTER COLUMN \"Power\" TYPE integer USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Trucks\" ALTER COLUMN \"PermittedMaximumWeight\" TYPE numeric USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Trucks\" ALTER COLUMN \"Mileage\" TYPE integer USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Trucks\" ALTER COLUMN \"LoadCapacity\" TYPE numeric USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Trucks\" ALTER COLUMN \"EngineType\" TYPE integer USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Trucks\" ALTER COLUMN \"EngineCapacity\" TYPE numeric USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Trucks\" ALTER COLUMN \"BodyType\" TYPE integer USING NULL::integer;");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "SpareAccessorTransps",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Compatibility",
                table: "SpareAccessorTransps",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "SpareAccessorTransps",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "SpareAccessorKomps",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Compatibility",
                table: "SpareAccessorKomps",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Condition",
                table: "SpareAccessorKomps",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("ALTER TABLE \"NoteBooks\" ALTER COLUMN \"Core\" TYPE integer USING 0;");

            migrationBuilder.AddColumn<string>(
                name: "GPU",
                table: "NoteBooks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OS",
                table: "NoteBooks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessorName",
                table: "NoteBooks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScreenResolution",
                table: "NoteBooks",
                type: "text",
                nullable: true);

            migrationBuilder.Sql("ALTER TABLE \"Cars\" ALTER COLUMN \"YearOfIssue\" TYPE integer USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Cars\" ALTER COLUMN \"TrunkVolume\" TYPE integer USING NULL::integer;");
            migrationBuilder.Sql("ALTER TABLE \"Cars\" ALTER COLUMN \"SteeringWheel\" TYPE integer USING NULL::integer;");
            migrationBuilder.Sql("ALTER TABLE \"Cars\" ALTER COLUMN \"SettingsMemory\" TYPE boolean USING NULL::boolean;");
            migrationBuilder.Sql("ALTER TABLE \"Cars\" ALTER COLUMN \"PowerSteering\" TYPE boolean USING NULL::boolean;");
            migrationBuilder.Sql("ALTER TABLE \"Cars\" ALTER COLUMN \"NumberOfSeats\" TYPE integer USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Cars\" ALTER COLUMN \"Mileage\" TYPE integer USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Cars\" ALTER COLUMN \"Gearbox\" TYPE integer USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Cars\" ALTER COLUMN \"FuelPer100km\" TYPE numeric USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Cars\" ALTER COLUMN \"EngineCapacity\" TYPE numeric USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Cars\" ALTER COLUMN \"Condition\" TYPE integer USING 0;");
            migrationBuilder.Sql("ALTER TABLE \"Cars\" ALTER COLUMN \"Clearance\" TYPE integer USING NULL::integer;");
            migrationBuilder.Sql("ALTER TABLE \"Cars\" ALTER COLUMN \"AccelerTo100km\" TYPE double precision USING NULL::double precision;");

            migrationBuilder.AddColumn<int>(
                name: "FuelType",
                table: "Cars",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "SpareAccessorTransps");

            migrationBuilder.DropColumn(
                name: "Compatibility",
                table: "SpareAccessorTransps");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "SpareAccessorTransps");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "SpareAccessorKomps");

            migrationBuilder.DropColumn(
                name: "Compatibility",
                table: "SpareAccessorKomps");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "SpareAccessorKomps");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "GPU",
                table: "NoteBooks");

            migrationBuilder.DropColumn(
                name: "OS",
                table: "NoteBooks");

            migrationBuilder.DropColumn(
                name: "ProcessorName",
                table: "NoteBooks");

            migrationBuilder.DropColumn(
                name: "ScreenResolution",
                table: "NoteBooks");

            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "Cars");

            migrationBuilder.AlterColumn<DateTime>(
                name: "YearOfIssue",
                table: "Trucks",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Transmission",
                table: "Trucks",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Power",
                table: "Trucks",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "PermittedMaximumWeight",
                table: "Trucks",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Mileage",
                table: "Trucks",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "LoadCapacity",
                table: "Trucks",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "EngineType",
                table: "Trucks",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "EngineCapacity",
                table: "Trucks",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "BodyType",
                table: "Trucks",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Core",
                table: "NoteBooks",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "YearOfIssue",
                table: "Cars",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "TrunkVolume",
                table: "Cars",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SteeringWheel",
                table: "Cars",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SettingsMemory",
                table: "Cars",
                type: "text",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PowerSteering",
                table: "Cars",
                type: "text",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumberOfSeats",
                table: "Cars",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Mileage",
                table: "Cars",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Gearbox",
                table: "Cars",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "FuelPer100km",
                table: "Cars",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "EngineCapacity",
                table: "Cars",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Condition",
                table: "Cars",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Clearance",
                table: "Cars",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccelerTo100km",
                table: "Cars",
                type: "text",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);
        }
    }
}
