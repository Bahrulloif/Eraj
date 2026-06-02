using Domain.Entities;
using AutoMapper;
using Domain.DTOs.CatalogDTOs;
using Domain.DTOs.SubCategoryDTOs;
using Domain.Entities.KompTech;
using Domain.DTOs.KomTechDTOs.NoteBookDTOs;
using Domain.DTOs.KomTechDTOs.SmartPhoneDTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Domain.DTOs.KomTechDTOs.TabletDTOs;
using Domain.DTOs.KomTechDTOs.SpareAccessorKompDTOs;
using Domain.DTOs.CategoryDTOs;
using Domain.DTOs.ProfileDTO;
using Domain.DTOs.OrderDTO;
using Domain.DTOs.CartDTO;
using Domain.DTOs.AddressDTO;
using Domain.DTOs.DeliveryAddressDTO;
using Domain.DTOs.TransportDTOs.CarsDTOs;
using Domain.DTOs.TransportDTOs.MotorbikeDTOs;
using Domain.DTOs.TransportDTOs.TruckDTOs;
using Domain.DTOs.TransportDTOs.SpareAccessorTranspDTOs;
using Domain.Entities.Transport;

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

        CreateMap<Address, GetAddressDTO>().ReverseMap();
        CreateMap<Address, AddAddressDTO>().ReverseMap();
        CreateMap<Address, UpdateAddressDTO>().ReverseMap();

        CreateMap<DeliveryAddress, GetDeliveryAddressDTO>().ReverseMap();
        CreateMap<DeliveryAddress, AddDeliveryAddressDTO>().ReverseMap();

        CreateMap<Car, GetCarDTO>().ReverseMap();
        CreateMap<Car, AddCarDTO>().ReverseMap();

        CreateMap<Motorbike, GetMotorbikeDTO>().ReverseMap();
        CreateMap<Motorbike, AddMotorbikeDTO>().ReverseMap();

        CreateMap<Truck, GetTruckDTO>().ReverseMap();
        CreateMap<Truck, AddTruckDTO>().ReverseMap();

        CreateMap<SpareAccessorKomp, GetSpareAccessorKompDTO>().ReverseMap();
        CreateMap<SpareAccessorKomp, AddSpareAccessorKompDTO>().ReverseMap();

        CreateMap<SpareAccessorTransp, GetSpareAccessorTranspDTO>().ReverseMap();
        CreateMap<SpareAccessorTransp, AddSpareAccessorTranspDTO>().ReverseMap();

    }
}
