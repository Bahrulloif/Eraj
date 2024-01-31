using AutoMapper;
using Domain.DTOs.KomTechDTOs.TabletDTOs;
using Domain.Entities.KompTech;
using Domain.Filters.KompTechFilters.TabletFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Services.KompTechService.TabletService;

public class TabletService : ITabletService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public TabletService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Response<List<GetTabletDTO>>> GetTablet(GetTabletFilter filter)
    {
        if (filter.Name != null)
        {
            var find = _context.Tablets.Where(t => t.Model.ToLower().Contains(filter.Name.ToLower())).ToListAsync();
            var result = _mapper.Map<List<GetTabletDTO>>(find);
            return new Response<List<GetTabletDTO>>(result);
        }
        var tablets = await _context.Tablets.ToListAsync();
        var mapped = _mapper.Map<List<GetTabletDTO>>(tablets);
        return new Response<List<GetTabletDTO>>(mapped);
    }

    public async Task<Response<GetTabletDTO>> GetTabletById(int tabletId)
    {
        var find = await _context.Tablets.FirstOrDefaultAsync(t => t.Id == tabletId);
        if (find == null)
        {
            return new Response<GetTabletDTO>(HttpStatusCode.NotFound, "Tablet not found");
        }
        var mapped = _mapper.Map<GetTabletDTO>(find);
        return new Response<GetTabletDTO>(mapped);
    }
    public async Task<Response<GetTabletDTO>> AddTablet(AddTabletDTO tablet)
    {
        if (tablet == null)
        {
            return new Response<GetTabletDTO>(HttpStatusCode.NotFound, "Please fill the parameter");
        }
        var mapped = _mapper.Map<Tablet>(tablet);
        await _context.Tablets.AddAsync(mapped);
        await _context.SaveChangesAsync();
        return new Response<GetTabletDTO>(HttpStatusCode.OK, "Tablet added successfully");
    }
    public async Task<Response<GetTabletDTO>> UpdateTablet(AddTabletDTO tablet)
    {
        if (tablet == null)
        {
            return new Response<GetTabletDTO>(HttpStatusCode.NotFound, "Please fill parameter");
        }
        var find = await _context.Tablets.AsNoTracking().FirstOrDefaultAsync(t => t.Id == tablet.Id);
        if (find != null)
        {
            var mapped = _mapper.Map<Tablet>(find);
            _context.Tablets.Update(mapped);
            await _context.SaveChangesAsync();
            return new Response<GetTabletDTO>(HttpStatusCode.OK, "Tablet updated successfully");
        }
        return new Response<GetTabletDTO>(HttpStatusCode.NotFound, "Tablet not found");
    }
    public async Task<Response<GetTabletDTO>> DeleteTablet(int tabletId)
    {
        var find = await _context.Tablets.FirstOrDefaultAsync(t => t.Id == tabletId);
        if (find == null)
        {
            return new Response<GetTabletDTO>(HttpStatusCode.NotFound, "Tablet not found");
        }
        _context.Tablets.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<GetTabletDTO>(HttpStatusCode.OK, "Tablet deleted successfully");
    }
}
