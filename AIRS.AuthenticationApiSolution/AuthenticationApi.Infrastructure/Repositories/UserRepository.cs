using AIRS.SharedLibrary.Responses;
using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Infrastructure.Repositories;

public class UserRepository(AuthenticationDbContext context,IConfiguration configuration) : IUser
{
    private async Task<AppUser> GetUserByEmail(string email)
    {
        var user =  await context.Users.FirstOrDefaultAsync(x => x.Email == email);
        return user is null ? null! : user;
    }
    public async Task<UserReadDto> GetUserDetailsToPublish(string email)
    {
        var user = await GetUserByEmail(email);
        UserReadDto result;
        if (user == null) {
            throw new Exception(email + " does not exist");

        }
        result = new UserReadDto { Id = user.Id, Name = user.Name, Telephone = user.Telephone!, Email = user.Email! };
        return result;
    }
    public async Task<UserReadDto> GetUser(int userId)
    {
        var user = await context.Users.FindAsync(userId);
        return user is not null ? new UserReadDto { Id = user.Id,Name = user.Name, Telephone = user.Telephone!, Email = user.Email! } : null!;
    }
    public async Task<UserReadDto> GetInfo(int userId)
    {
        var user = await context.Users.FindAsync(userId);
        
        return user is not null ? new UserReadDto { Id = user.Id, Name = user.Name, Telephone = user.Telephone!, Email = user.Email! } : null!;
    }
    public async Task<Response<string>> Login(LoginDto loginDto)
    {
        var user = await GetUserByEmail(loginDto.Email);
        if (user == null)
        {
            return new Response<string>(false, "Invalid Credentials", null);
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
        if (!isPasswordValid)
        {
            return new Response<string>(false, "Invalid Credentials", null);
        }

        string token = GenerateToken(user);
        return new Response<string>(true, "Login successful", token);
    }

    private string GenerateToken(AppUser user)
    {
        var key = Encoding.UTF8.GetBytes(configuration["Authentication:Key"]!);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.Name!)
        };
        if (!string.IsNullOrEmpty(user.Role))
        {
            claims.Add(new Claim(ClaimTypes.Role, user.Role));
        }
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(60),
            SigningCredentials = credentials,
            Issuer = configuration["Authentication:Issuer"],
            Audience = configuration["Authentication:Audience"]

        };
        var handler = new JsonWebTokenHandler(); 
        return handler.CreateToken(tokenDescriptor);
    } 

    public async Task<Response<UserReadDto>> Register(AppUser appUser)
    {
        var existingUser = await GetUserByEmail(appUser.Email!);
        if (existingUser != null)
        {
            return new Response<UserReadDto>(false, "User already exists", null);
        }

        appUser.Password = BCrypt.Net.BCrypt.HashPassword(appUser.Password);
        appUser.Role = "User";
        appUser.DateRegistered = DateTime.UtcNow;

        var result = await context.Users.AddAsync(appUser);
        await context.SaveChangesAsync();

        var userReadDto = new UserReadDto
        {
            Id = appUser.Id,
            Name = appUser.Name,
            Email = appUser.Email,
            Telephone = appUser.Telephone
        };

        return result.Entity.Id > 0
            ? new Response<UserReadDto>(true, "User registered successfully", userReadDto)
            : new Response<UserReadDto>(false, "Failed to register user", null);
    }
}
