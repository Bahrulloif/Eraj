using AutoMapper;
using Domain.DTOs.KomTechDTOs.NoteBookDTOs;
using Domain.Entities.KompTech;
using Domain.Filters.KompTechFilters.NoteBookFilters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Infrastructure.Services.KompTechService.NoteBookService;

public class NoteBookService : INoteBookService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public NoteBookService(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Response<List<GetNoteBookDTO>>> GetNoteBook(GetNoteBookFilter filter)
    {
        if (filter.Name != null)
        {
            var find = await _context.NoteBooks.Where(n => n.Model == filter.Name).AsNoTracking().ToListAsync();
            var mapped = _mapper.Map<List<GetNoteBookDTO>>(find);
            return new Response<List<GetNoteBookDTO>>(mapped);
        }
        var noteBooks = await _context.NoteBooks.AsNoTracking().ToListAsync();
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
