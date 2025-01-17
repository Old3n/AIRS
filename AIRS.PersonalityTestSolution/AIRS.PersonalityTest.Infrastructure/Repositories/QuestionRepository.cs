using AIRS.PersonalityTest.Domain.Entities;
using AIRS.PersonalityTest.Infrastructure.Data;
using AIRS.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;

namespace AIRS.PersonalityTest.Infrastructure.Repositories;
public class QuestionRepository : IQuestionRepository
{
    private readonly PersonalityTestDbContext _context;

    public QuestionRepository(PersonalityTestDbContext context)
    {
        _context = context;
    }

    public async Task<Response<string>> AddQuestionAsync(string testCode, Question question)
    {
        var test = await _context.Tests.FirstOrDefaultAsync(t => t.TestCode == testCode);
        if (test == null)
        {
            return new Response<string>(false, "Cannot find Test"); ; 
        }

        
        question.TestId = test.TestId;

        
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();

        return new  Response<string>(true, "Questions Added Succesfully");
    }

   
}
