using AutoMapper;
using Domain.DTOs.ProfileDTO;
using Domain.Entities;
using Domain.Filters.ProfileFilter;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.ProfileService;

public class ProfileService : IProfileService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    public ProfileService(IMapper mapper, DataContext context)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Response<List<GetProfileDTO>>> GetProfile(GetProfileFilter filter)
    {
        if (filter != null)
        {
            var profile = await _context.Profiles.Where(p => p.Name == filter.Name).ToListAsync();
            var result = _mapper.Map<List<GetProfileDTO>>(profile);
            return new Response<List<GetProfileDTO>>(result);
        }
        var response = await _context.Profiles.ToListAsync();
        var mapped = _mapper.Map<List<GetProfileDTO>>(response);
        return new Response<List<GetProfileDTO>>(mapped);
    }
    public async Task<Response<GetProfileDTO>> GetProfileById(string profileId)
    {
        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.ApplicationUserId == profileId);
        if (profile != null)
        {
            var mapped = _mapper.Map<GetProfileDTO>(profile);
            return new Response<GetProfileDTO>(mapped);
        }
        return new Response<GetProfileDTO>(System.Net.HttpStatusCode.NotFound, "Profile Not Found");
    }
    public async Task<Response<GetProfileDTO>> AddProfile(AddProfileDTO profile)
    {
        var mapped = _mapper.Map<ProfileUser>(profile);
        await _context.Profiles.AddAsync(mapped);
        await _context.SaveChangesAsync();
        return new Response<GetProfileDTO>(System.Net.HttpStatusCode.OK, "Profile added successfully");

    }
    public async Task<Response<GetProfileDTO>> UpdateProfile(UpdateProfileDTO profile)
    {
        var find = await _context.Profiles.FirstOrDefaultAsync(p => p.ApplicationUserId == profile.Id);
        if (find != null)
        {
            var result = _mapper.Map<ProfileUser>(profile);
            _context.Profiles.Update(result);
            await _context.SaveChangesAsync();
        }
        return new Response<GetProfileDTO>(System.Net.HttpStatusCode.BadRequest, "Profile not found");
    }
    public async Task<Response<GetProfileDTO>> DeleteProfile(string profileId)
    {
        var find = await _context.Profiles.FirstOrDefaultAsync(p => p.ApplicationUserId == profileId);
        if (find != null)
        {
            _context.Remove(find);
            await _context.SaveChangesAsync();
            return new Response<GetProfileDTO>(System.Net.HttpStatusCode.OK, $"The Profile deleted successfully");
        }
        return new Response<GetProfileDTO>(System.Net.HttpStatusCode.BadRequest, "The profile is not exist");
    }
}
