using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderProductType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductType",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            // Backfill: existing Orders rows predate the discriminator and were only ever
            // matched by (ProductId, SubCategoryId). Infer ProductType by joining each product
            // table on that same pair - this holds as long as no two product types actually
            // collided on (ProductId, SubCategoryId) before this migration (same assumption the
            // Pictures.ProductType backfill in 20260903053607_AddPictureProductType relied on).
            // Values match Domain.Enum.ProductType.
            migrationBuilder.Sql(@"UPDATE ""Orders"" o SET ""ProductType"" = 1 FROM ""Cars"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Orders"" o SET ""ProductType"" = 2 FROM ""Motorbikes"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Orders"" o SET ""ProductType"" = 3 FROM ""Trucks"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Orders"" o SET ""ProductType"" = 4 FROM ""SpareAccessorTransps"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Orders"" o SET ""ProductType"" = 5 FROM ""NoteBooks"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Orders"" o SET ""ProductType"" = 6 FROM ""SmartPhones"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Orders"" o SET ""ProductType"" = 7 FROM ""Tablets"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Orders"" o SET ""ProductType"" = 8 FROM ""SpareAccessorKomps"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Orders"" o SET ""ProductType"" = 9 FROM ""Apartments"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Orders"" o SET ""ProductType"" = 10 FROM ""CommercialRealEstates"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Orders"" o SET ""ProductType"" = 11 FROM ""Cottages"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(13)",
                oldMaxLength: 13);
        }
    }
}
