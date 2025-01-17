using System.ComponentModel.DataAnnotations;

namespace AIRS.UserProfileApi.Domain.Entities;

public class UserInfo
{
    [Key]
    [Required]
    public int UserId { get; set; }
    public int? Age { get; set; }
    public string? City { get; set; }
    public string? StudyField { get; set; }
    public decimal? Income { get; set; }
    public List<string>? Hobbies { get; set; }
    public List<string>? Interests { get; set; }
    public string? MbtiType { get; set; }

    public UserInfo()
    {
        Hobbies = [];
        Interests = [];
    }
}
