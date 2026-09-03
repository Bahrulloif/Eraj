namespace Domain.Enum;

// Discriminator for Picture.ProductId, which by itself is only unique within one
// product table (each of the 11 product entities has its own auto-increment Id).
// Without this, two different product types landing in the same SubCategoryId with
// a matching Id would share (and corrupt/delete) each other's pictures.
public enum ProductType
{
    Car = 1,
    Motorbike = 2,
    Truck = 3,
    SpareAccessorTransp = 4,
    NoteBook = 5,
    SmartPhone = 6,
    Tablet = 7,
    SpareAccessorKomp = 8,
    Apartment = 9,
    CommercialRealEstate = 10,
    Cottage = 11
}
