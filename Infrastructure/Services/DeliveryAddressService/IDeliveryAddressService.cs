using Domain.DTOs.DeliveryAddressDTO;
using Domain.Filters.DeliveryAddressFilter;
using Domain.Responses;

namespace Infrastructure.Services.DeliveryAddressService;

public interface IDeliveryAddressService
{
  Task<Response<List<GetDeliveryAddressDTO>>> GetDeliveryAddress(DeliveryAddressFilter filter, string currentUserId, bool isPrivileged);
  Task<Response<GetDeliveryAddressDTO>> GetDeliveryAddressById(int deliveryAddressId, string currentUserId, bool isPrivileged);
  Task<Response<string>> AddDeliveryAddress(AddDeliveryAddressDTO deliveryAddress, string currentUserId);
  Task<Response<GetDeliveryAddressDTO>> UpdateDeliveryAddress(AddDeliveryAddressDTO deliveryAddress, string currentUserId, bool isPrivileged);
  Task<Response<string>> DeleteDeliveryAddress(int deliveryAddressId, string currentUserId, bool isPrivileged);
}
