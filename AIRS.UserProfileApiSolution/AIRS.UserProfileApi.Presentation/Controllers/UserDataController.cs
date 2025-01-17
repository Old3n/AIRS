using AIRS.SharedLibrary.Responses;
using AIRS.UserProfileApi.Application.Interfaces;
using AIRS.UserProfileApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AIRS.UserProfileApi.Presentation.Controllers;
[Route("api/Data")]
[ApiController]
public class UserDataController(IUserInfoRepository userInfoRepository) : ControllerBase
{
    private readonly IUserInfoRepository _userInfoRepository = userInfoRepository;

    [Authorize(Roles = "admin")]
    [HttpGet("export")]
    public async Task<ActionResult<Response<string>>> ExportUserData()
    {
        var userInfos = await _userInfoRepository.GetAllUsers();
        if (userInfos == null || !userInfos.Any())
        {
            return NotFound(new Response<string>(false, "No user data found"));
        }

        var csv = ConvertToCsv(userInfos);
        var bytes = Encoding.UTF8.GetBytes(csv);
        _ = new FileContentResult(bytes, "text/csv")
        {
            FileDownloadName = "UserData.csv"
        };

        return File(bytes, "text/csv", "UserData.csv");
    }

    private static string ConvertToCsv(IEnumerable<UserInfo> userInfos)
    {
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("UserId,Age,City,StudyField,Income,Hobbies,Interests,MbtiType");

        foreach (var userInfo in userInfos)
        {
            var hobbies = string.Join(";", userInfo.Hobbies!);
            var interests = string.Join(";", userInfo.Interests!);
            csvBuilder.AppendLine($"{userInfo.UserId},{userInfo.Age},{userInfo.City},{userInfo.StudyField},{userInfo.Income.ToString()},{hobbies},{interests},{userInfo.MbtiType}");
        }

        return csvBuilder.ToString();
    }
}
