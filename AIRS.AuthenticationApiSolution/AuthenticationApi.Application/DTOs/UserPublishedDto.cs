using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.DTOs;
public class UserPublishedDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Event { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
}
