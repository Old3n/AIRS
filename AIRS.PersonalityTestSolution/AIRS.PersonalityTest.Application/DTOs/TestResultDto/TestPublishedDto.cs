using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRS.PersonalityTest.Application.DTOs.TestResultDto;
public class TestPublishedDto
{
    
    public string? Result { get; set; }

    [Required]
    public int? UserId { get; set; }
    
    public string? TestName { get; set; } 
}

