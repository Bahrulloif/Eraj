using Domain.DTOs.LoginDTOs;
using Domain.DTOs.RegisterDTO;
using Domain.Responses;

namespace Infrastructure.AccountService;

public interface IAccountService
{
    public Task<Response<string>> Login(LoginDTO login);
    public Task<Response<string>> Register(RegisterDTO user);
}
