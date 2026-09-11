using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubCategoryAndCartProductType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductType",
                table: "SubCategories",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductType",
                table: "Carts",
                type: "integer",
                nullable: true);

            // Backfill SubCategories.ProductType by name - the seeded dev DB names are the only
            // signal available (no FK to any product table exists to join on instead). Values
            // match Domain.Enum.ProductType. Any subcategory whose name doesn't match one of
            // these (e.g. "Запасная подкатегория (можно удалить)") is left NULL on purpose -
            // guessing wrong here would be worse than leaving it unset for an admin to fill in.
            migrationBuilder.Sql(@"UPDATE ""SubCategories"" SET ""ProductType"" = 1 WHERE ""SubCategoryName"" = 'Автомобили';");
            migrationBuilder.Sql(@"UPDATE ""SubCategories"" SET ""ProductType"" = 2 WHERE ""SubCategoryName"" = 'Мотоциклы';");
            migrationBuilder.Sql(@"UPDATE ""SubCategories"" SET ""ProductType"" = 3 WHERE ""SubCategoryName"" = 'Грузовики';");
            migrationBuilder.Sql(@"UPDATE ""SubCategories"" SET ""ProductType"" = 4 WHERE ""SubCategoryName"" = 'Запчасти для транспорта';");
            migrationBuilder.Sql(@"UPDATE ""SubCategories"" SET ""ProductType"" = 5 WHERE ""SubCategoryName"" = 'Ноутбуки';");
            migrationBuilder.Sql(@"UPDATE ""SubCategories"" SET ""ProductType"" = 6 WHERE ""SubCategoryName"" = 'Смартфоны';");
            migrationBuilder.Sql(@"UPDATE ""SubCategories"" SET ""ProductType"" = 7 WHERE ""SubCategoryName"" = 'Планшеты';");
            migrationBuilder.Sql(@"UPDATE ""SubCategories"" SET ""ProductType"" = 8 WHERE ""SubCategoryName"" = 'Запчасти для компьютеров';");
            migrationBuilder.Sql(@"UPDATE ""SubCategories"" SET ""ProductType"" = 9 WHERE ""SubCategoryName"" = 'Квартиры';");
            migrationBuilder.Sql(@"UPDATE ""SubCategories"" SET ""ProductType"" = 10 WHERE ""SubCategoryName"" = 'Коммерческая недвижимость';");
            migrationBuilder.Sql(@"UPDATE ""SubCategories"" SET ""ProductType"" = 11 WHERE ""SubCategoryName"" = 'Коттеджи';");

            // Backfill Carts.ProductType by joining on (ProductId, SubCategoryId), same approach
            // as Picture/Order's own ProductType backfills. Existing Cart rows predate
            // Cart.SubCategoryId ever being set by the API (it existed on the entity but was
            // never mapped to CartDTO - see CartDTO.cs's comment) so it's 0 on every pre-existing
            // row; no real product has SubCategoryId 0, so this join intentionally matches
            // nothing for old rows and leaves them NULL rather than guessing off ProductId alone
            // (which collides across all 11 tables). Only new Cart rows, added after this
            // migration with a real SubCategoryId from the client, will ever get backfilled here
            // in practice - which is correct.
            migrationBuilder.Sql(@"UPDATE ""Carts"" o SET ""ProductType"" = 1 FROM ""Cars"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Carts"" o SET ""ProductType"" = 2 FROM ""Motorbikes"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Carts"" o SET ""ProductType"" = 3 FROM ""Trucks"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Carts"" o SET ""ProductType"" = 4 FROM ""SpareAccessorTransps"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Carts"" o SET ""ProductType"" = 5 FROM ""NoteBooks"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Carts"" o SET ""ProductType"" = 6 FROM ""SmartPhones"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Carts"" o SET ""ProductType"" = 7 FROM ""Tablets"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Carts"" o SET ""ProductType"" = 8 FROM ""SpareAccessorKomps"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Carts"" o SET ""ProductType"" = 9 FROM ""Apartments"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Carts"" o SET ""ProductType"" = 10 FROM ""CommercialRealEstates"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
            migrationBuilder.Sql(@"UPDATE ""Carts"" o SET ""ProductType"" = 11 FROM ""Cottages"" t WHERE o.""ProductId"" = t.""Id"" AND o.""SubCategoryId"" = t.""SubCategoryId"";");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Carts");
        }
    }
}
