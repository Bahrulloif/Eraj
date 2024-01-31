using Domain.DTOs.DeliveryAddressDTO;
using Domain.Filters.DeliveryAddressFilter;
using Domain.Responses;

namespace Infrastructure.Services.DeliveryAddressService;

public interface IDeliveryAddressService
{
  Task<Response<List<GetDeliveryAddressDTO>>> GetDeliveryAddress(DeliveryAddressFilter filter);
  Task<Response<GetDeliveryAddressDTO>> GetDeliveryAddressById(int deliveryAddressId);
  Task<Response<string>> AddDeliveryAddress(AddDeliveryAddressDTO deliveryAddress);
  Task<Response<GetDeliveryAddressDTO>> UpdateDeliveryAddress(AddDeliveryAddressDTO deliveryAddress);
  Task<Response<string>> DeleteDeliveryAddress(int deliveryAddressId);
}

