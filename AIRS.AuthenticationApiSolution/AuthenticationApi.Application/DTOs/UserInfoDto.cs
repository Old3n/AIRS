using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.DTOs;
public class UserInfoDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
}
