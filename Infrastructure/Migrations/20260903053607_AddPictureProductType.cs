using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPictureProductType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductType",
                table: "Pictures",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // Backfill: existing Pictures rows predate the discriminator and were only ever
            // matched by (ProductId, SubCategoryId). Infer ProductType by joining each product
            // table on that same pair - this holds as long as no two product types actually
            // collided on (ProductId, SubCategoryId) before this migration, which is the
            // assumption this whole migration is built on. Values match Domain.Enum.ProductType.
            migrationBuilder.Sql(@"UPDATE ""Pictures"" p SET ""ProductType"" = 1 FROM ""Cars"" t WHERE p.""ProductId"" = t.""Id"" AND p.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Pictures"" p SET ""ProductType"" = 2 FROM ""Motorbikes"" t WHERE p.""ProductId"" = t.""Id"" AND p.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Pictures"" p SET ""ProductType"" = 3 FROM ""Trucks"" t WHERE p.""ProductId"" = t.""Id"" AND p.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Pictures"" p SET ""ProductType"" = 4 FROM ""SpareAccessorTransps"" t WHERE p.""ProductId"" = t.""Id"" AND p.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Pictures"" p SET ""ProductType"" = 5 FROM ""NoteBooks"" t WHERE p.""ProductId"" = t.""Id"" AND p.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Pictures"" p SET ""ProductType"" = 6 FROM ""SmartPhones"" t WHERE p.""ProductId"" = t.""Id"" AND p.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Pictures"" p SET ""ProductType"" = 7 FROM ""Tablets"" t WHERE p.""ProductId"" = t.""Id"" AND p.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Pictures"" p SET ""ProductType"" = 8 FROM ""SpareAccessorKomps"" t WHERE p.""ProductId"" = t.""Id"" AND p.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Pictures"" p SET ""ProductType"" = 9 FROM ""Apartments"" t WHERE p.""ProductId"" = t.""Id"" AND p.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Pictures"" p SET ""ProductType"" = 10 FROM ""CommercialRealEstates"" t WHERE p.""ProductId"" = t.""Id"" AND p.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Pictures"" p SET ""ProductType"" = 11 FROM ""Cottages"" t WHERE p.""ProductId"" = t.""Id"" AND p.""SubCategoryId"" = t.""SubCategoryId"";");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_ProductType_ProductId",
                table: "Pictures",
                columns: new[] { "ProductType", "ProductId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pictures_ProductType_ProductId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Pictures");
        }
    }
}
