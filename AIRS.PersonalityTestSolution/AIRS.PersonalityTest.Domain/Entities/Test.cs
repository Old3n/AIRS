using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRS.PersonalityTest.Domain.Entities;

public class Test
{
    [Key]
    public int TestId { get; set; } 

    [Required]
    [StringLength(100)]
    public string? TestName { get; set; }

    [Required]
    [StringLength(50)]
    public string? TestCode { get; set; } 

    public ICollection<Question> Questions { get; set; } = [];
}

