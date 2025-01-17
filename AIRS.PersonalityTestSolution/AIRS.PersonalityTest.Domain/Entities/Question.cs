using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRS.PersonalityTest.Domain.Entities;

public class Question
{
    [Key]
    public int QuestionId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(250)")]
    public string? QuestionText { get; set; }

    public int TestId { get; set; }
    public Test? Test { get; set; }

    public ICollection<Answer> Answers { get; set; } = [];
}
