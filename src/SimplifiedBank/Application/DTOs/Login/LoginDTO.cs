using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplifiedBank.Application.DTOs.Login
{
    public record struct LoginDTO(string Email, string Password);
}