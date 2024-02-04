using AutoMapper;
using Domain.DTOs.KomTechDTOs.NoteBookDTOs;
using Domain.DTOs.PictureDTO;
using Domain.Entities;
using Domain.Entities.KompTech;
using Domain.Filters.KompTechFilters.NoteBookFilters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Services.KompTechService.NoteBookService;

public class NoteBookService : INoteBookService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    public NoteBookService(DataContext context, IMapper mapper, IFileService fileService)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
    }
    public async Task<PagedResponse<List<GetNoteBookDTO>>> GetNoteBook(GetNoteBookFilter filter)
    {
        var query = _context.NoteBooks.AsQueryable();
        if (filter.Name != null)
        {
            query = query.Where(x => x.Model.ToLower().Contains(filter.Name.ToLower()));
        };
        var mapped = await (from n in query
                            select new GetNoteBookDTO
                            {
                                Id = n.Id,
                                Color = n.Color,
                                Diagonal = n.Diagonal,
                                Model = n.Model,
                                Core = n.Core,
                                RAM = n.RAM,
                                ROM = n.ROM,
                                Price = n.Price,
                                DiscountPrice = n.DiscountPrice,
                                Images = _context.Pictures
                                    .Where(p => p.ProductId == n.Id && p.SubCategoryId == n.SubCategoryId)
                                    .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                    .ToList()
                            }).Skip((filter.PageNumber - 1) * filter.PageSize)
                            .Take(filter.PageSize).ToListAsync();
        var totalCount = await query.CountAsync();

        return new PagedResponse<List<GetNoteBookDTO>>(mapped, filter.PageNumber, filter.PageSize, totalCount);
    }

    public async Task<Response<GetNoteBookDTO>> GetNoteBookById(int noteBookId)
    {
        var query = _context.NoteBooks.AsQueryable();
        query = query.Where(n => n.Id == noteBookId);
        if (query == null)
        {
            return new Response<GetNoteBookDTO>(HttpStatusCode.NotFound, "NoteBook not found");
        }
        var mapped = await (from n in query
                            select new GetNoteBookDTO
                            {
                                Id = n.Id,
                                Color = n.Color,
                                Diagonal = n.Diagonal,
                                Model = n.Model,
                                Core = n.Core,
                                RAM = n.RAM,
                                ROM = n.ROM,
                                Price = n.Price,
                                DiscountPrice = n.DiscountPrice,
                                Images = _context.Pictures
                                    .Where(p => p.ProductId == n.Id && p.SubCategoryId == n.SubCategoryId)
                                    .Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName })
                                    .ToList()
                            }).FirstOrDefaultAsync();
        return new Response<GetNoteBookDTO>(mapped);
    }
    public async Task<Response<string>> AddNoteBook(AddNoteBookDTO noteBook)
    {
        if (noteBook == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Please fill the parameters");
        }
        var mapped = _mapper.Map<NoteBook>(noteBook);
        await _context.NoteBooks.AddAsync(mapped);
        await _context.SaveChangesAsync();
        foreach (var item in noteBook.Images)
        {
            var imageName = _fileService.CreateFile(item);
            var image = new Picture
            {
                ImageName = imageName.Data!,
                ProductId = mapped.Id,
                SubCategoryId = mapped.SubCategoryId
            };
            await _context.Pictures.AddAsync(image);
            await _context.SaveChangesAsync();
        }
        return new Response<string>("NoteBook added successfully");
    }
    public async Task<Response<string>> UpdateNoteBook(AddNoteBookDTO noteBook)
    {
        if (noteBook == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "Please fill the parameters");
        }
        var find = await _context.NoteBooks.Where(n => n.Id == noteBook.Id).AsNoTracking().FirstOrDefaultAsync();
        if (find != null)
        {
            var mapped = _mapper.Map<NoteBook>(noteBook);
            _context.NoteBooks.Update(mapped);
            if (noteBook.Images != null)
            {
                var images = await _context.Pictures.Where(n => n.ProductId == noteBook.Id
                             && n.SubCategoryId == noteBook.SubCategoryId).
                             ToListAsync();
                foreach (var item in images)
                {
                    _fileService.DeleteFile(item.ImageName);
                    await _context.SaveChangesAsync();               // нужен ли это?
                }
                _context.Pictures.RemoveRange(images);
                await _context.SaveChangesAsync();
                
                foreach (var item in noteBook.Images)
                {
                    var imageName = _fileService.CreateFile(item);
                    var image = new Picture
                    {
                        ImageName = imageName.Data!,
                        ProductId = mapped.Id,
                        SubCategoryId = mapped.SubCategoryId
                    };
                    await _context.Pictures.AddAsync(image);
                    await _context.SaveChangesAsync();
                }
            }
            return new Response<string>("The notebook is updated successfully");
        }
        return new Response<string>(HttpStatusCode.NotFound, "NoteBook not found");
    }
    public async Task<Response<string>> DeleteNoteBook(int noteBookId)
    {
        var find = await _context.NoteBooks.FirstOrDefaultAsync(n => n.Id == noteBookId);
        if (find == null)
        {
            return new Response<string>(HttpStatusCode.NotFound, "The NoteBook is not found");
        }
        var images = await _context.Pictures.Where(p => p.ProductId == find.Id &&
                            p.SubCategoryId == find.SubCategoryId).
                            ToListAsync();
        foreach (var item in images)
        {
            _fileService.DeleteFile(item.ImageName);
        }
        _context.Pictures.RemoveRange(images);
        _context.NoteBooks.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<string>("NoteBook deleted successfully");
    }
}