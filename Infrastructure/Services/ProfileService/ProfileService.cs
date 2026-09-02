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

    public async Task<Response<List<GetProfileDTO>>> GetProfile(GetProfileFilter filter, string currentUserId, bool isPrivileged)
    {
        var query = _context.Profiles.AsQueryable();
        if (!isPrivileged)
        {
            query = query.Where(p => p.ApplicationUserId == currentUserId);
        }
        if (filter.Name != null)
        {
            query = query.Where(p => p.Name == filter.Name);
        }
        var profile = await query.ToListAsync();
        var result = _mapper.Map<List<GetProfileDTO>>(profile);
        return new Response<List<GetProfileDTO>>(result);
    }
    public async Task<Response<GetProfileDTO>> GetProfileById(string profileId, string currentUserId, bool isPrivileged)
    {
        if (!isPrivileged && profileId != currentUserId)
        {
            return new Response<GetProfileDTO>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this profile");
        }
        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.ApplicationUserId == profileId);
        if (profile != null)
        {
            var mapped = _mapper.Map<GetProfileDTO>(profile);
            return new Response<GetProfileDTO>(mapped);
        }
        return new Response<GetProfileDTO>(System.Net.HttpStatusCode.NotFound, "Profile Not Found");
    }
    public async Task<Response<GetProfileDTO>> AddProfile(AddProfileDTO profile, string currentUserId)
    {
        // Registration already creates a Profiles row (PK = ApplicationUserId) for every user,
        // so this always hit a duplicate-key crash before. Use PUT /profile to edit it instead.
        var exists = await _context.Profiles.AnyAsync(p => p.ApplicationUserId == currentUserId);
        if (exists)
        {
            return new Response<GetProfileDTO>(System.Net.HttpStatusCode.Conflict, "Profile already exists, use update instead");
        }
        var mapped = _mapper.Map<ProfileUser>(profile);
        mapped.ApplicationUserId = currentUserId;
        await _context.Profiles.AddAsync(mapped);
        await _context.SaveChangesAsync();
        return new Response<GetProfileDTO>(System.Net.HttpStatusCode.OK, "Profile added successfully");

    }
    public async Task<Response<GetProfileDTO>> UpdateProfile(UpdateProfileDTO profile, string currentUserId, bool isPrivileged)
    {
        if (!isPrivileged && profile.Id != currentUserId)
        {
            return new Response<GetProfileDTO>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this profile");
        }
        var find = await _context.Profiles.FirstOrDefaultAsync(p => p.ApplicationUserId == profile.Id);
        if (find != null)
        {
            _mapper.Map(profile, find);
            _context.Profiles.Update(find);
            await _context.SaveChangesAsync();
            return new Response<GetProfileDTO>(System.Net.HttpStatusCode.OK, "Profile updated successfully");
        }
        return new Response<GetProfileDTO>(System.Net.HttpStatusCode.NotFound, "Profile not found");
    }
    public async Task<Response<GetProfileDTO>> DeleteProfile(string profileId, string currentUserId, bool isPrivileged)
    {
        if (!isPrivileged && profileId != currentUserId)
        {
            return new Response<GetProfileDTO>(System.Net.HttpStatusCode.Forbidden, "You do not have access to this profile");
        }
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
