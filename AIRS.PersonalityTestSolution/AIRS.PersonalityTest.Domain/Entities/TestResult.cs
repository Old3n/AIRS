using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRS.PersonalityTest.Domain.Entities
{
    public class TestResult
    {
        public string? TestName { get; set; }
        public int UserId { get; set; } 
        public int TotalScore { get; set; }
        public string? Result { get; set; }
        public DateTime DateTaken { get; set; }
    }
}
