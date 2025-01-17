using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AIRS.PersonalityTest.Application.DTOs.TestDto;
using AIRS.PersonalityTest.Domain.Entities;
using AIRS.PersonalityTest.Application.Interfaces;
using AIRS.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;

namespace AIRS.PersonalityTest.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestsController(ITestRepository testRepository, IMapper mapper) : ControllerBase
{
    private readonly ITestRepository _testRepository = testRepository;
    private readonly IMapper _mapper = mapper;

    // POST: api/Tests
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ActionResult<Response<TestDTO>>> AddTest([FromBody] TestDTO testDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var test = _mapper.Map<Test>(testDto);

        var addedTest = await _testRepository.AddTestAsync(test);

        if (addedTest.Data == null)
            return BadRequest(addedTest);
        var createdTestDto = _mapper.Map<TestDTO>(addedTest.Data);

        var response = new Response<TestDTO>(addedTest.Flag, addedTest.Message, createdTestDto);

        return Ok(response);
    }

    // GET: api/Tests/{testCode}
    [HttpGet("by-code/{testCode}")]
    public async Task<ActionResult<TestDTO>> GetTest(string testCode)
    {
        var test = await _testRepository.GetTestByCodeAsync(testCode);

        if (test == null)
            return NotFound();

        var testDto = _mapper.Map<TestDTO>(test);

        return Ok(testDto);
    }

    // GET: api/Tests/by-name/{testName}
    [HttpGet("by-name/{testName}")]
    public async Task<ActionResult<Response<string>>> GetTestIdByName(string testName)
    {
        var response = await _testRepository.GetTestIdByNameAsync(testName);
        if (response.Data == null)
            return NotFound();
        return Ok(response);
    }
}
