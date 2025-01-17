using AIRS.PersonalityTest.Application.DTOs.TestResultDto;
using AIRS.PersonalityTest.Application.Interfaces;
using AIRS.PersonalityTest.Domain.Entities;
using AIRS.PersonalityTest.Infrastructure.Data;
using AIRS.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRS.PersonalityTest.Infrastructure.Repositories;
public class TestResultRepository(PersonalityTestDbContext context) : ITestResultRepository
{
    private readonly PersonalityTestDbContext _context = context;

    public async Task<Response<TestPublishedDto>> CalculateTestResultAsync(string testName, TestSubmissionDto submissionDto, int userId)
    {
        // Fetch the test by name
        var test = await _context.Tests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.TestName == testName);

        if (test == null)
        {
            return new Response<TestPublishedDto>(false, "Test not found.");
        }

        var answers = await _context.Answers
            .Where(a => submissionDto.SelectedAnswerIds!.Contains(a.AnswerId))
            .ToListAsync();

        if (answers.Count != submissionDto.SelectedAnswerIds!.Count)
        {
            return new Response<TestPublishedDto>(false, "Some answers were not found.");
        }
        string result; 
       
        int totalScore = answers.Sum(a => a.Points);

        switch(testName)
        {
            case "MBTI":
                result= DetermineMbtiType(totalScore);
                break;
            default:
                return new Response<TestPublishedDto>(false, "Test not found.");
        }
        
        // Create TestResultReadDto
        var resultDto = new TestPublishedDto
        {
            UserId = userId,
            Result = result,
            TestName = testName 
        };

        return new Response<TestPublishedDto>(true, "Test result calculated successfully.", resultDto);
    }

    private static string DetermineMbtiType(int totalScore)
    {
        // Your logic here
        if (totalScore < 50)
            return "Introvert";
        else if (totalScore < 100)
            return "Ambivert";
        else
            return "Extrovert";
    }
}
