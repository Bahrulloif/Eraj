using AutoMapper;
using Domain.DTOs.CatalogDTOs;
using Domain.Entities;
using Domain.Filters.CatalogFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Services.CatalogService;

public class CatalogService : ICatalogService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public CatalogService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Response<List<GetCatalogDTO>>> GetCatalog(GetCatalogFilter filter)
    {
        if (filter.Name != null)
        {
            var search = await _context.Catalogs.Where(n => n.CatalogName.ToLower().Contains(filter.Name.ToLower())).AsNoTracking().ToListAsync();
            var result = _mapper.Map<List<GetCatalogDTO>>(search);
            return new Response<List<GetCatalogDTO>>(result);

        }

        var catalogs = await _context.Catalogs.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

        var mapper = _mapper.Map<List<GetCatalogDTO>>(catalogs);
        return new Response<List<GetCatalogDTO>>(mapper);
    }
    public async Task<Response<GetCatalogDTO>> GetCatalogById(int catalogId)
    {
        var catalog = await _context.Catalogs.FindAsync(catalogId);
        var mapper = _mapper.Map<GetCatalogDTO>(catalog);
        return new Response<GetCatalogDTO>(mapper);
    }

    public async Task<Response<GetCatalogDTO>> AddCatalog(AddCatalogDTO catalog)
    {
        var mapper = _mapper.Map<Catalog>(catalog);
        await _context.Catalogs.AddAsync(mapper);
        await _context.SaveChangesAsync();
        var result = _mapper.Map<GetCatalogDTO>(mapper);
        return new Response<GetCatalogDTO>(result);
    }

    public async Task<Response<GetCatalogDTO>> UpdateCatalog(AddCatalogDTO catalog)
    {
        if (catalog == null)
        {
            return new Response<GetCatalogDTO>(HttpStatusCode.NotFound, "Please fill out the catalog");
        }
        var find = await _context.Catalogs.AsNoTracking().FirstOrDefaultAsync(c => c.CatalogId == catalog.CatalogId);
        if (find != null)
        {
            var mapper = _mapper.Map<Catalog>(catalog);
            _context.Catalogs.Update(mapper);
            await _context.SaveChangesAsync();
            return new Response<GetCatalogDTO>(HttpStatusCode.OK, "Update is success");
        }
        return new Response<GetCatalogDTO>(HttpStatusCode.NotFound, "Catalog not found");
    }

    public async Task<Response<GetCatalogDTO>> DeleteCatalog(int idCatalog)
    {
        var catalog = await _context.Catalogs.FirstOrDefaultAsync(c => c.CatalogId == idCatalog);
        if (catalog != null)
        {
            _context.Catalogs.Remove(catalog);
            await _context.SaveChangesAsync();
            return new Response<GetCatalogDTO>(HttpStatusCode.OK, "Catalog deleted saccessfully");
        }
        return new Response<GetCatalogDTO>(HttpStatusCode.NotFound, "Catalog not found");
    }

}
