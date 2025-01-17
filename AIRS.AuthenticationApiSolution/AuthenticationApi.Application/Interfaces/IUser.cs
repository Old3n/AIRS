using AIRS.SharedLibrary.Responses;
using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.Interfaces;
public interface IUser
{
    Task<Response<UserReadDto>> Register(AppUser appUser);
    Task<Response<string>> Login(LoginDto loginDto);
    Task<UserReadDto> GetUserDetailsToPublish(string email);
    Task<UserReadDto> GetUser(int userId);
    Task<UserReadDto> GetInfo(int userId); 
}
