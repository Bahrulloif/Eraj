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
    public async Task<Response<List<GetNoteBookDTO>>> GetNoteBook(GetNoteBookFilter filter)
    {
        if (filter.Name != null)
        {
            // var find = await _context.NoteBooks.Where(n => n.Model == filter.Name).AsNoTracking().ToListAsync();
            var mapped = await (from n in _context.NoteBooks
                                join p in _context.Pictures on n.Id equals p.ProductId
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
                                    Images = _context.Pictures.Where(p => p.ProductId == n.Id).Select(s => new PictureDto { Id = s.Id, ImageName = s.ImageName }).ToList()
                                }).ToListAsync();
            // var Picture =
            // var mapped = _mapper.Map<List<GetNoteBookDTO>>(find);
            return new Response<List<GetNoteBookDTO>>(mapped);
        }
        var noteBooks = await _context.NoteBooks.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).AsNoTracking().ToListAsync();
        var result = _mapper.Map<List<GetNoteBookDTO>>(noteBooks);
        return new Response<List<GetNoteBookDTO>>(result);
    }
    public async Task<Response<GetNoteBookDTO>> GetNoteBookById(int noteBookId)
    {
        var find = await _context.NoteBooks.FirstOrDefaultAsync(n => n.Id == noteBookId);
        if (find == null)
        {
            return new Response<GetNoteBookDTO>(HttpStatusCode.NotFound, "NoteBook not found");
        }
        var mapped = _mapper.Map<GetNoteBookDTO>(find);
        return new Response<GetNoteBookDTO>(mapped);
    }
    public async Task<Response<GetNoteBookDTO>> AddNoteBook(AddNoteBookDTO noteBook)
    {
        if (noteBook == null)
        {
            return new Response<GetNoteBookDTO>(HttpStatusCode.NotFound, "Please fill the parameters");
        }
        var mapped = _mapper.Map<NoteBook>(noteBook);
        await _context.NoteBooks.AddAsync(mapped);
        await _context.SaveChangesAsync();
        foreach (var item in noteBook.Images)
        {
            // var imageName = await _fileService.Create(item);
            var imageName = _fileService.CreateFile(item);
            var image = new Picture
            {
                ImageName = imageName.Data!,
                ProductId = mapped.Id,
                SubCategoryId = mapped.SubCategoryId
            };

        }
        return new Response<GetNoteBookDTO>(HttpStatusCode.OK, "NoteBook added successfully");
    }
    public async Task<Response<GetNoteBookDTO>> UpdateNoteBook(AddNoteBookDTO noteBook)
    {
        if (noteBook == null)
        {
            return new Response<GetNoteBookDTO>(HttpStatusCode.NotFound, "Please fill the parameters");
        }
        var find = await _context.NoteBooks.Where(n => n.Id == noteBook.Id).AsNoTracking().FirstOrDefaultAsync();
        if (find != null)
        {
            var mapped = _mapper.Map<NoteBook>(noteBook);
            _context.NoteBooks.Update(mapped);
            await _context.SaveChangesAsync();
            return new Response<GetNoteBookDTO>(HttpStatusCode.OK, "The notebook is updated successfully");
        }
        return new Response<GetNoteBookDTO>(HttpStatusCode.NotFound, "NoteBook not found");
    }
    public async Task<Response<GetNoteBookDTO>> DeleteNoteBook(int noteBookId)
    {
        var find = await _context.NoteBooks.FirstOrDefaultAsync(n => n.Id == noteBookId);
        if (find == null)
        {
            return new Response<GetNoteBookDTO>(HttpStatusCode.NotFound, "The NoteBook is not found");
        }
        _context.NoteBooks.Remove(find);
        await _context.SaveChangesAsync();
        return new Response<GetNoteBookDTO>(HttpStatusCode.OK, "NoteBook deleted successfuly");
    }
}
