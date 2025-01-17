using System.ComponentModel.DataAnnotations;

namespace AIRS.UserProfileApi.Application.Dtos;
public class TestPublishedDto
{
    [Required]
    public string? Result { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public string? TestName { get; set; }
}

