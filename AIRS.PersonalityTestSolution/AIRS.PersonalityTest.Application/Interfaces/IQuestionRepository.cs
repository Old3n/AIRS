using AIRS.PersonalityTest.Domain.Entities;
using AIRS.SharedLibrary.Responses;

namespace AIRS.PersonalityTest.Infrastructure.Repositories;

public interface IQuestionRepository
{
    public Task<Response<string>> AddQuestionAsync(string testCode, Question question);
}