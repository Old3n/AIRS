using AIRS.SharedLibrary.Responses;
using AIRS.UserProfileApi.Application.Dtos;
using AIRS.UserProfileApi.Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AIRS.UserProfileApi.Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserProfileController(IUserInfoRepository userInfoRepository, IMapper mapper) : ControllerBase
{
    private readonly IUserInfoRepository _userInfoRepository = userInfoRepository;
    private readonly IMapper _mapper = mapper;

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserProfileReadDto>> GetUserProfile()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userInfoResponse = await _userInfoRepository.GetUserProfileInfo(userId);
        if (userInfoResponse.Data == null)
        {
            return NotFound();
        }

        var userProfileReadDto = _mapper.Map<UserProfileReadDto>(userInfoResponse.Data);
        return Ok(userProfileReadDto);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Response<string>>> UpdateUserProfile(UserProfleUpdateDto userProfleUpdateDto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);



        var response = await _userInfoRepository.UpdateUserInfo(userId, userProfleUpdateDto);

        if (!response.Flag)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);
    }
}
