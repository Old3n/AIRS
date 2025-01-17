using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRS.PersonalityTest.Domain.Entities
{
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(250)")]
        public string? AnswerText { get; set; }

        public int Points { get; set; } 

        public int QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}
