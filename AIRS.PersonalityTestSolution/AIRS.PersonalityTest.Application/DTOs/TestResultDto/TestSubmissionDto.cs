using System.ComponentModel.DataAnnotations;

namespace AIRS.PersonalityTest.Application.DTOs.TestResultDto;
public class TestSubmissionDto
{
    [Required]
    public string? TestName { get; set; }

    [Required]
    public List<int>? SelectedAnswerIds { get; set; } 
}
