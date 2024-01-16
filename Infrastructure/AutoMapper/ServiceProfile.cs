using Domain.Entities;
using AutoMapper;
using Domain.DTOs.CatalogDTOs;
using Domain.DTOs.SubCategoryDTOs;
using Domain.Entities.KompTech;
using Domain.DTOs.KomTechDTOs.NoteBookDTOs;
using Domain.DTOs.KomTechDTOs.SmartPhoneDTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Domain.DTOs.KomTechDTOs.TabletDTOs;
using Domain.DTOs.CategoryDTOs;
using Domain.DTOs.ProfileDTO;
using Domain.DTOs.OrderDTO;
using Domain.DTOs.CartDTO;

namespace Infrastructure.AutoMapper;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Catalog, GetCatalogDTO>().ReverseMap();
        CreateMap<Catalog, AddCatalogDTO>().ReverseMap();

        CreateMap<Category, GetCategoryDTO>().ReverseMap();
        CreateMap<Category, AddCategoryDTO>().ReverseMap();

        CreateMap<SubCategory, GetSubCategoryDTO>().ReverseMap();
        CreateMap<SubCategory, AddSubCategoryDTO>().ReverseMap();

        CreateMap<NoteBook, GetNoteBookDTO>().ReverseMap();
        CreateMap<NoteBook, AddNoteBookDTO>().ReverseMap();

        CreateMap<SmartPhone, GetSmartPhoneDTO>().ReverseMap();
        CreateMap<SmartPhone, AddSmartPhoneDTO>().ReverseMap();

        CreateMap<Tablet, GetTabletDTO>().ReverseMap();
        CreateMap<Tablet, AddTabletDTO>().ReverseMap();

        CreateMap<ProfileUser, GetProfileDTO>().ReverseMap();
        CreateMap<ProfileUser, AddProfileDTO>().ReverseMap();
        CreateMap<ProfileUser, UpdateProfileDTO>().ReverseMap();

        CreateMap<Order, GetOrderDTO>().ReverseMap();
        CreateMap<Order, AddOrderDTO>().ReverseMap();

        CreateMap<Cart, GetCartDTO>().ReverseMap();
        CreateMap<Cart, AddCartDTO>().ReverseMap();

    }
}
