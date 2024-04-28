using Domain.DTOs.TransportDTOs.MotorbikeDTOs;
using Domain.Filters.TransportFilters.GetMotorbikeFilter;
using Domain.Responses;

namespace Infrastructure.Services.TransportService.MotorbikeService;

public interface IMotorbikeService
{
    public Task<PagedResponse<List<GetMotorbikeDTO>>> GetMotorbike(GetMotorbikeFilter filter);
    public Task<Response<GetMotorbikeDTO>> GetMotorbikeById(int motorbikeId);
    public Task<Response<string>> AddMotorbike(AddMotorbikeDTO motorbike);
    public Task<Response<string>> UpdateMotorbike(AddMotorbikeDTO motorbike);
    public Task<Response<string>> DeleteMotorbike(int motorbikeId);
}
