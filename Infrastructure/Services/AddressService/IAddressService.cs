using Domain.DTOs.AddressDTO;
using Domain.Filters.AddressFilter;
using Domain.Responses;

namespace Infrastructure.Services.AddressService;

public interface IAddressService
{
    Task<Response<List<GetAddressDTO>>> GetAddress(AddressFilter filter, string currentUserId, bool isPrivileged);
    Task<Response<GetAddressDTO>> GetAddressById(int addressId, string currentUserId, bool isPrivileged);
    Task<Response<GetAddressDTO>> AddAddress(AddAddressDTO address, string currentUserId);
    Task<Response<string>> UpdateAddress(UpdateAddressDTO address, string currentUserId, bool isPrivileged);
    Task<Response<GetAddressDTO>> DeleteAddress(int addressId, string currentUserId, bool isPrivileged);
}
