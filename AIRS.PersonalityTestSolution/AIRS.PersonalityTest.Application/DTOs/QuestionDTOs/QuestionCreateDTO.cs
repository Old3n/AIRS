using AIRS.PersonalityTest.Application.DTOs.AwnserDtos;
using System.ComponentModel.DataAnnotations;

namespace AIRS.PersonalityTest.Application.DTOs.QuestionDTOs;

public class QuestionCreateDTO
{
    [Required]
    [StringLength(250)]
    public string? QuestionText { get; set; }

    [Required]
    public List<AnswerDTO>? Answers { get; set; }
}