using AIRS.PersonalityTest.Application.DTOs.TestResultDto;
using AIRS.PersonalityTest.Application.Interfaces;
using AIRS.SharedLibrary.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AIRS.PersonalityTest.Presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResultController(ITestResultRepository resultRepo, ITestRepository testRepo, IMessageBus messageBus) : ControllerBase
{
    private readonly ITestResultRepository _resultRepo = resultRepo;
    private readonly ITestRepository _testRepo = testRepo;
    private readonly IMessageBus _messageBus = messageBus; 

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Response<string>>> Submit([FromBody] TestSubmissionDto submissionDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Get the user ID from the JWT token
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        

        // Find the test by TestCode
        var test = await _testRepo.GetTestIdByNameAsync(submissionDto.TestName!);
        if (test == null)
            return NotFound($"Test with Name '{submissionDto.TestName}' not found.");

        // Calculate the test result
        var result = await _resultRepo.CalculateTestResultAsync(test.Data!, submissionDto, userId);
        if (!result.Flag)
            return BadRequest(new Response<string>(false, result.Message));

        // Publish the result to RabbitMQ
        await _messageBus.PublishAsync("trigger", "test_result", result.Data);

        return Ok(new Response<string>(true, "Test Submitted successfully"));
    }
}
