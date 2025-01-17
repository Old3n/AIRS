
using AIRS.PersonalityTest.Application.DTOs.TestResultDto;
using AIRS.PersonalityTest.Domain.Entities;
using AIRS.SharedLibrary.Responses;

namespace AIRS.PersonalityTest.Application.Interfaces;
public interface ITestRepository
{
    Task<Response<Test>> AddTestAsync(Test test);
    Task<Response<Test>> GetTestByCodeAsync(string testCode);
    Task<Response<string>> GetTestIdByNameAsync(string testName);
}
