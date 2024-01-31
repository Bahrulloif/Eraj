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
    public async Task<Response<GetAddressDTO>> AddAddress(AddAddressDTO address)
    {
        var mapped = _mapper.Map<Address>(address);
        await _context.Addresses.AddAsync(mapped);
        await _context.SaveChangesAsync();
        return new Response<GetAddressDTO>(System.Net.HttpStatusCode.OK, "Address added successfully");
    }
    public async Task<Response<string>> UpdateAddress(UpdateAddressDTO address)
    {
        var find = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.Id == address.Id);
        if (find != null)
        {
            var mapped = _mapper.Map<Address>(address);
            _context.Addresses.Update(mapped);
            await _context.SaveChangesAsync();
            return new Response<string>("Address updated successfully");
        }
        return new Response<string>(System.Net.HttpStatusCode.NotFound, "Address Not Found");

    }
    public async Task<Response<GetAddressDTO>> DeleteAddress(int addressId)
    {
        var find = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(a => a.Id == addressId);
        if (find != null)
        {
            _context.Addresses.Remove(find);
            await _context.SaveChangesAsync();
            return new Response<GetAddressDTO>(System.Net.HttpStatusCode.OK, "Address deleted successfully");
        }
        return new Response<GetAddressDTO>(System.Net.HttpStatusCode.NotFound, "Address Not Found");
    }
}
