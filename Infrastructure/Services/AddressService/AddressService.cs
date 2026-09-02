using AutoMapper;
using Domain.DTOs.AddressDTO;
using Domain.Entities;
using Domain.Filters.AddressFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.AddressService;

public class AddressService : IAddressService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    public AddressService(IMapper mapper, DataContext context)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Response<List<GetAddressDTO>>> GetAddress(AddressFilter filter)
    {
        if (filter.Country != null)
        {
            var find = await _context.Addresses.Where(a => a.Country == filter.Country).AsNoTracking().ToListAsync();
            var mapped = _mapper.Map<List<GetAddressDTO>>(find);
            return new Response<List<GetAddressDTO>>(mapped);
        }
        var result = await _context.Addresses.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).AsNoTracking().ToListAsync();
        var response = _mapper.Map<List<GetAddressDTO>>(result);
        return new Response<List<GetAddressDTO>>(response);
    }
    public async Task<Response<GetAddressDTO>> GetAddressById(int addressId)
    {
        var find = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.Id == addressId);
        if (find != null)
        {
            var mapped = _mapper.Map<GetAddressDTO>(find);
            return new Response<GetAddressDTO>(mapped);
        }
        return new Response<GetAddressDTO>(System.Net.HttpStatusCode.NotFound, "Address Not Found");
    }
    public async Task<Response<GetAddressDTO>> AddAddress(AddAddressDTO address, string currentUserId)
    {
        var mapped = _mapper.Map<Address>(address);
        mapped.OwnerId = currentUserId;
        await _context.Addresses.AddAsync(mapped);
        await _context.SaveChangesAsync();
        return new Response<GetAddressDTO>(System.Net.HttpStatusCode.OK, "Address added successfully");
    }

    /// <summary>
    /// Address is shared reference data (also pointed at by ProfileUser.AddressId and
    /// DeliveryAddress.AddressId once linked), so ownership isn't only OwnerId: a non-privileged
    /// caller may manage one they created, or one actually referenced by their own profile or
    /// one of their own delivery addresses.
    /// </summary>
    private async Task<bool> OwnsAddress(Address address, string currentUserId)
    {
        if (address.OwnerId == currentUserId)
        {
            return true;
        }
        var ownsViaProfile = await _context.Profiles
            .AnyAsync(p => p.ApplicationUserId == currentUserId && p.AddressId == address.Id);
        if (ownsViaProfile)
        {
            return true;
        }
        return await _context.DeliveryAddresses
            .AnyAsync(d => d.ApplicationUserId == currentUserId && d.AddressId == address.Id);
    }

    public async Task<Response<string>> UpdateAddress(UpdateAddressDTO address, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == address.Id);
        if (find == null)
        {
            return new Response<string>(System.Net.HttpStatusCode.NotFound, "Address Not Found");
        }
        if (!isPrivileged && !await OwnsAddress(find, currentUserId))
        {
            return new Response<string>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this address");
        }
        _mapper.Map(address, find);
        _context.Addresses.Update(find);
        await _context.SaveChangesAsync();
        return new Response<string>("Address updated successfully");
    }
    public async Task<Response<GetAddressDTO>> DeleteAddress(int addressId, string currentUserId, bool isPrivileged)
    {
        var find = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == addressId);
        if (find == null)
        {
            return new Response<GetAddressDTO>(System.Net.HttpStatusCode.NotFound, "Address Not Found");
        }
        if (!isPrivileged && !await OwnsAddress(find, currentUserId))
        {
            return new Response<GetAddressDTO>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this address");
        }
        _context.Addresses.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<GetAddressDTO>(System.Net.HttpStatusCode.OK, "Address deleted successfully");
    }
}
