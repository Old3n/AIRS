namespace AIRS.UserProfileApi.Application.Dtos;
public class UserProfleUpdateDto
{
    public int? Age { get; set; }
    public string? City { get; set; }
    public string? StudyField { get; set; }
    public decimal? Income { get; set; }
    public List<string>? Hobbies { get; set; }
    public List<string>? Interests { get; set; }
}
