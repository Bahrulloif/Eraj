using AutoMapper;
using Domain.DTOs.KomTechDTOs.SmartPhoneDTOs;
using Domain.Entities.KompTech;
using Domain.Filters.KompTechFilters.SmartPhoneFilters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Services.KompTechService.SmartPhoneService;

public class SmartPhoneService : ISmartPhoneService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public SmartPhoneService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Response<List<GetSmartPhoneDTO>>> GetSmartPhone(GetSmartPhoneFilter filter)
    {
        if (filter.Name != null)
        {
            var find = _context.SmartPhones.Where(s => s.Model == filter.Name);
            var result = _mapper.Map<List<GetSmartPhoneDTO>>(find);
            return new Response<List<GetSmartPhoneDTO>>(result);
        }
        var smartPhone = await _context.SmartPhones.ToListAsync();
        var mapped = _mapper.Map<List<GetSmartPhoneDTO>>(smartPhone);
        return new Response<List<GetSmartPhoneDTO>>(mapped);
    }
    public async Task<Response<GetSmartPhoneDTO>> GetSmartPhoneById(int smartPhoneId)
    {
        var find = await _context.SmartPhones.FirstOrDefaultAsync(s => s.Id == smartPhoneId);
        if (find == null)
        {
            return new Response<GetSmartPhoneDTO>(HttpStatusCode.NotFound, "SmartPhone not found");
        }
        var mapped = _mapper.Map<GetSmartPhoneDTO>(find);
        return new Response<GetSmartPhoneDTO>(mapped);
    }
    public async Task<Response<GetSmartPhoneDTO>> AddSmartPhone(AddSmartPhoneDTO smartPhone)
    {
        if (smartPhone == null)
        {
            return new Response<GetSmartPhoneDTO>(HttpStatusCode.OK, "Please fill the SmartPhone");
        }
        var result = _mapper.Map<SmartPhone>(smartPhone);
        await _context.SmartPhones.AddAsync(result);
        await _context.SaveChangesAsync();
        return new Response<GetSmartPhoneDTO>(HttpStatusCode.OK, $"{result.Model} SmartPhone was added successfully");
    }
    public async Task<Response<GetSmartPhoneDTO>> UpdateSmartPhone(AddSmartPhoneDTO smartPhone)
    {
        var find = await _context.SmartPhones.Where(s => s.Id == smartPhone.Id).AsNoTracking().FirstOrDefaultAsync();
        if (find != null)
        {
            var mapped = _mapper.Map<SmartPhone>(smartPhone);
            _context.SmartPhones.Update(mapped);
            await _context.SaveChangesAsync();
            return new Response<GetSmartPhoneDTO>(HttpStatusCode.OK, "SmartPhone updated successfully");
        }
        return new Response<GetSmartPhoneDTO>(HttpStatusCode.NotFound, "SmartPhone not found");
    }
    public async Task<Response<GetSmartPhoneDTO>> DeleteSmartPhone(int smartPhoneId)
    {
        var find = await _context.SmartPhones.FirstOrDefaultAsync(s => s.Id == smartPhoneId);
        if (find == null)
        {
            return new Response<GetSmartPhoneDTO>(HttpStatusCode.NotFound, "SmartPhone not found");
        }
        _context.SmartPhones.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<GetSmartPhoneDTO>(HttpStatusCode.OK, "SmartPhone deleted succesfully");
    }
}