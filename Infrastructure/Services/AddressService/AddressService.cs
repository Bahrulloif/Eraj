using AutoMapper;
using Domain.DTOs.AddressDTO;
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
        var find = await _context.Addresses.FirstOrDefaultAsync(a => a.Country == filter.Country);
        if (find != null)
        {
            var mapped = _mapper.Map<List<GetAddressDTO>>(find);
            return new Response<List<GetAddressDTO>>(mapped);
        }
        var result = await _context.Addresses.ToListAsync();
        var response = _mapper.Map<List<GetAddressDTO>>(result);
        return new Response<List<GetAddressDTO>>(response);
    }
    public async Task<Response<GetAddressDTO>> GetAddressById(int addressId)
    {
        var find = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == addressId);
        if (find != null)
        {
            var mapped = _mapper.Map<GetAddressDTO>(find);
            return new Response<GetAddressDTO>(mapped);
        }
        return new Response<GetAddressDTO>(System.Net.HttpStatusCode.NotFound, "Address Not Found");
    }
    // public async Task<Response<GetAddressDTO>> AddAddress(AddAddressDTO address)
    // {
    //     var 
    // }
    // Task<Response<GetAddressDTO>> UpdateAddress(AddAddressDTO address);
    // Task<Response<GetAddressDTO>> DeleteAddress(int addressId);
}
