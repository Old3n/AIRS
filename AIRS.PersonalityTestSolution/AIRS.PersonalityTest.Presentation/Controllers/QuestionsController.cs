using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AIRS.PersonalityTest.Domain.Entities;
using AIRS.PersonalityTest.Infrastructure.Data;
using AIRS.PersonalityTest.Infrastructure.Repositories;
using AIRS.SharedLibrary.Responses;
using Microsoft.AspNetCore.Authorization;
using AIRS.PersonalityTest.Application.DTOs.QuestionDTOs;

namespace AIRS.PersonalityTest.Presentation.Controllers;

[ApiController]
[Route("api/tests/{testCode}/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IMapper _mapper;

    public QuestionsController(IQuestionRepository questionRepository, IMapper mapper)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;
    }

    // POST: api/tests/{testCode}/questions
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Response<String>>> AddQuestion(string testCode, [FromBody] QuestionCreateDTO questionCreateDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Map the DTO to the domain model
        var question = _mapper.Map<Question>(questionCreateDTO);

        
        var addedQuestion = await _questionRepository.AddQuestionAsync(testCode, question);
        if (addedQuestion == null)
            return NotFound($"Test with code '{testCode}' not found.");

        return Ok(addedQuestion);
    }
}
