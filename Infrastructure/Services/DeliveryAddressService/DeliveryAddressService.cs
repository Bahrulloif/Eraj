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
    public async Task<Response<List<GetDeliveryAddressDTO>>> GetDeliveryAddress(DeliveryAddressFilter filter, string currentUserId, bool isPrivileged)
    {
        var query = _context.DeliveryAddresses.AsQueryable();
        if (!isPrivileged)
        {
            query = query.Where(d => d.ApplicationUserId == currentUserId);
        }
        if (filter.Id != null)
        {
            query = query.Where(d => d.Id == filter.Id);
            var find = await query.ToListAsync();
            var mapped = _mapper.Map<List<GetDeliveryAddressDTO>>(find);
            return new Response<List<GetDeliveryAddressDTO>>(mapped);
        }
        // Skip/Take needs a deterministic order to paginate correctly - without it SQL doesn't
        // guarantee row order, so results can drift or duplicate across pages.
        var result = await query.OrderBy(d => d.Id).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
        var response = _mapper.Map<List<GetDeliveryAddressDTO>>(result);
        return new Response<List<GetDeliveryAddressDTO>>(response);

    }
    public async Task<Response<GetDeliveryAddressDTO>> GetDeliveryAddressById(int deliveryAddressId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.DeliveryAddresses.FirstOrDefaultAsync(d => d.Id == deliveryAddressId);
        if (find == null)
        {
            return new Response<GetDeliveryAddressDTO>(System.Net.HttpStatusCode.NotFound, "DeliveryAddress not Found");
        }
        if (!isPrivileged && find.ApplicationUserId != currentUserId)
        {
            return new Response<GetDeliveryAddressDTO>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this delivery address");
        }
        var mapped = _mapper.Map<GetDeliveryAddressDTO>(find);
        return new Response<GetDeliveryAddressDTO>(mapped);
    }
    public async Task<Response<string>> AddDeliveryAddress(AddDeliveryAddressDTO deliveryAddress, string currentUserId)
    {
        // Always the caller's own delivery address — never trust a client-supplied ApplicationUserId.
        deliveryAddress.ApplicationUserId = currentUserId;
        var find = await _context.DeliveryAddresses.FirstOrDefaultAsync(d => d.Id == deliveryAddress.Id);
        if (find == null)
        {
            var mapped = _mapper.Map<DeliveryAddress>(deliveryAddress);
            await _context.DeliveryAddresses.AddAsync(mapped);
            await _context.SaveChangesAsync();
            return new Response<string>("DeliveryAddress added successfully");
        }
        return new Response<string>(System.Net.HttpStatusCode.Conflict, "DeliveryAddress already exist");
    }
    public async Task<Response<GetDeliveryAddressDTO>> UpdateDeliveryAddress(AddDeliveryAddressDTO deliveryAddress, string currentUserId, bool isPrivileged)
    {
        var find = await _context.DeliveryAddresses.FirstOrDefaultAsync(d => d.Id == deliveryAddress.Id);
        if (find == null)
        {
            return new Response<GetDeliveryAddressDTO>(System.Net.HttpStatusCode.NotFound, "DeliveryAddress Not Found");
        }
        if (!isPrivileged && find.ApplicationUserId != currentUserId)
        {
            return new Response<GetDeliveryAddressDTO>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this delivery address");
        }
        if (!isPrivileged)
        {
            // Never let a non-privileged caller reassign the delivery address to someone else.
            deliveryAddress.ApplicationUserId = currentUserId;
        }
        _mapper.Map(deliveryAddress, find);
        _context.DeliveryAddresses.Update(find);
        await _context.SaveChangesAsync();
        return new Response<GetDeliveryAddressDTO>(System.Net.HttpStatusCode.OK, "DeliveryAddress updated successfully");
    }

    public async Task<Response<string>> DeleteDeliveryAddress(int deliveryAddressId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.DeliveryAddresses.FirstOrDefaultAsync(d => d.Id == deliveryAddressId);
        if (find == null)
        {
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "DeliveryAddress Not Found");
        }
        if (!isPrivileged && find.ApplicationUserId != currentUserId)
        {
            return new Response<string>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this delivery address");
        }
        _context.DeliveryAddresses.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<string>("DeliveryAddress deleted successfully");
    }
}
