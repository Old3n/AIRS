
using AIRS.PersonalityTest.Application.DTOs.QuestionDTOs;

namespace AIRS.PersonalityTest.Application.DTOs.TestDto;
public class TestDTO
{
    public string? TestName { get; set; }
    public string? TestCode { get; set; }
    public List<QuestionCreateDTO>? Questions { get; set; }
}