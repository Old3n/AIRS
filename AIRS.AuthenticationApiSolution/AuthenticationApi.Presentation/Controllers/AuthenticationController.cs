using AIRS.SharedLibrary.Responses;
using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AuthenticationApi.Domain.Entities;
using Serilog;
using AIRS.SharedLibrary.Logs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AuthenticationApi.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(IUser userRepository, IMapper mapper, IMessageBus messageBus) : ControllerBase
{
    private readonly IUser _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly IMessageBus _messageBus = messageBus;
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    [HttpPost("register")]
    public async Task<ActionResult<Response<UserReadDto>>> Register(UserCreateDto userCreateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new Response<UserReadDto>(false, "Invalid Input", null));

        var appUser = _mapper.Map<AppUser>(userCreateDto);
        var result = await _userRepository.Register(appUser);
        var userReadDto = _mapper.Map<UserReadDto>(appUser);

        if (result.Flag)
        {
            var userReadDtoToPublish = await _userRepository.GetUserDetailsToPublish(appUser.Email!);

            var userPublishedDto = _mapper.Map<UserPublishedDto>(userReadDtoToPublish);
            // trying to send message to other services
            userPublishedDto.Event = "User_Registered";
            try
            {
                await _messageBus.PublishAsync(
                    exchange: "trigger",
                    routingKey: "",
                    message: userPublishedDto
                );
            }
            catch (Exception ex)
            {
                LogExceptions.LogException(ex);
            }


            return Ok(userReadDto);
        }
        else
        {
            return BadRequest(result);
        }
    }


    [HttpPost("login")]
    public async Task<ActionResult<Response<string>>> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new Response<string>(false, "Invalid Input", null));

        var result = await _userRepository.Login(loginDto);

        if (result.Flag)
        {
            return Ok(result);
        }
        else
        {
            return BadRequest(result);
        }
    }


    [Authorize(Roles ="admin")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserReadDto>> GetUser(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid Input");
        
        
        var user = await _userRepository.GetUser(id);


        return user != null ? Ok(user) : NotFound();
    }


    [Authorize]     
    [HttpGet]
    public async Task<ActionResult<UserInfoDto>> GetInfo()
    {
        var userIdentity = User.Identity as ClaimsIdentity;
        if (userIdentity == null || !userIdentity.IsAuthenticated)
        {
            return Unauthorized();
        }

        var userIdClaim = userIdentity.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized();
        }

        var userReadDto = await _userRepository.GetInfo(userId);
        if (userReadDto == null)
        {
            return NotFound();
        }

        var userInfoDto = _mapper.Map<UserInfoDto>(userReadDto);

        return Ok(userInfoDto);
    }
}

