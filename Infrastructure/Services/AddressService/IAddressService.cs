using Domain.DTOs.AddressDTO;
using Domain.Filters.AddressFilter;
using Domain.Responses;

namespace Infrastructure.Services.AddressService;

public interface IAddressService
{
    Task<Response<List<GetAddressDTO>>> GetAddress(AddressFilter filter);
    Task<Response<GetAddressDTO>> GetAddressById(int addressId);
    Task<Response<GetAddressDTO>> AddAddress(AddAddressDTO address);
    Task<Response<string>> UpdateAddress(UpdateAddressDTO address);
    Task<Response<GetAddressDTO>> DeleteAddress(int addressId);
}
