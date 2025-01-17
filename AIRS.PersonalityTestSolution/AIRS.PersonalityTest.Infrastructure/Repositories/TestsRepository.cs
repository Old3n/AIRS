using AIRS.PersonalityTest.Application.Interfaces;
using AIRS.PersonalityTest.Domain.Entities;
using AIRS.PersonalityTest.Infrastructure.Data;
using AIRS.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;


namespace AIRS.PersonalityTest.Infrastructure.Repositories;
public class TestRepository(PersonalityTestDbContext context) : ITestRepository
{
    private readonly PersonalityTestDbContext _context = context;

    public async Task<Response<Test>> GetTestByCodeAsync(string testCode)
    {
        var response =  await _context.Tests
               .Include(t => t.Questions)
                   .ThenInclude(q => q.Answers)
               .FirstOrDefaultAsync(t => t.TestCode == testCode);
        if (response == null)
        {
            return new Response<Test>(false, "Cannot find Test");
        }
        return new Response<Test>(true,"Test Found" ,response);
    }

    public Task<Response<string>> GetTestIdByNameAsync(string testName)
    {
        var response = _context.Tests.FirstOrDefaultAsync(t => t.TestName == testName);
        if(response.Result is null)
            return Task.FromResult(new Response<string>(false, "Test Not Found"));
        return Task.FromResult(new Response<string>(true, "Test Found", response.Result.TestCode));
    }

    public async Task<Response<Test>> AddTestAsync(Test test)
    {
        test.TestCode = Guid.NewGuid().ToString("N");
        var result  = await _context.Tests.AddAsync(test);
        await _context.SaveChangesAsync();
        if(result.Entity is null)
            return new Response<Test>(false, "Test Not Added");
        

        return new Response<Test>(true, "Test Added", result.Entity);
    }


}
