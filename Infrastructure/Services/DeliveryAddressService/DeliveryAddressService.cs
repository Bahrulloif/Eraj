using AutoMapper;
using Domain.DTOs.DeliveryAddressDTO;
using Domain.Entities;
using Domain.Filters.DeliveryAddressFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.DeliveryAddressService;

public class DeliveryAddressService : IDeliveryAddressService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    public DeliveryAddressService(IMapper mapper, DataContext context)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Response<List<GetDeliveryAddressDTO>>> GetDeliveryAddress(DeliveryAddressFilter filter)
    {

        if (filter.Id != null)
        {
            var find = await _context.DeliveryAddresses.Where(d => d.Id == filter.Id).ToListAsync();
            var mapped = _mapper.Map<List<GetDeliveryAddressDTO>>(find);
            return new Response<List<GetDeliveryAddressDTO>>(mapped);
        }
        var result = await _context.DeliveryAddresses.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
        var response = _mapper.Map<List<GetDeliveryAddressDTO>>(result);
        return new Response<List<GetDeliveryAddressDTO>>(response);

    }
    public async Task<Response<GetDeliveryAddressDTO>> GetDeliveryAddressById(int deliveryAddressId)
    {
        var find = await _context.DeliveryAddresses.FirstOrDefaultAsync(d => d.Id == deliveryAddressId);
        if (find != null)
        {
            var mapped = _mapper.Map<GetDeliveryAddressDTO>(find);
            return new Response<GetDeliveryAddressDTO>(mapped);
        }
        return new Response<GetDeliveryAddressDTO>(System.Net.HttpStatusCode.BadRequest, "DeliveryAddress not Found");
    }
    public async Task<Response<string>> AddDeliveryAddress(AddDeliveryAddressDTO deliveryAddress)
    {
        var find = await _context.DeliveryAddresses.FirstOrDefaultAsync(d => d.Id == deliveryAddress.Id);
        if (find == null)
        {
            var mapped = _mapper.Map<DeliveryAddress>(deliveryAddress);
            await _context.DeliveryAddresses.AddAsync(mapped);
            await _context.SaveChangesAsync();
            return new Response<string>("DeliveryAddress added successfully");
        }
        return new Response<string>(System.Net.HttpStatusCode.BadRequest, "DeliveryAddress already exist");
    }
    public async Task<Response<GetDeliveryAddressDTO>> UpdateDeliveryAddress(AddDeliveryAddressDTO deliveryAddress)
    {
        var find = await _context.DeliveryAddresses.FirstOrDefaultAsync(d => d.Id == deliveryAddress.Id);
        if (find != null)
        {
            var mapped = _mapper.Map<DeliveryAddress>(deliveryAddress);
            _context.DeliveryAddresses.Update(mapped);
            await _context.SaveChangesAsync();
            return new Response<GetDeliveryAddressDTO>(System.Net.HttpStatusCode.OK, "DeliveryAddress updated successfully");
        }
        return new Response<GetDeliveryAddressDTO>(System.Net.HttpStatusCode.BadRequest, "DeliveryAddress Not Found");
    }

    public async Task<Response<string>> DeleteDeliveryAddress(int deliveryAddressId)
    {
        var find = await _context.DeliveryAddresses.FirstOrDefaultAsync(d => d.Id == deliveryAddressId);
        if (find != null)
        {
            _context.DeliveryAddresses.Remove(find);
            await _context.SaveChangesAsync();
            return new Response<string>("DeliveryAddress deleted successfully");
        }
        return new Response<string>(System.Net.HttpStatusCode.NotFound, "DeliveryAddress Not Found");
    }
}
