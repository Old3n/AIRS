using AIRS.PersonalityTest.Application.DTOs.TestResultDto;
using AIRS.SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRS.PersonalityTest.Application.Interfaces;
public interface ITestResultRepository
{
   Task<Response<TestPublishedDto>> CalculateTestResultAsync(string testName, TestSubmissionDto submissionDto, int userId);
}
