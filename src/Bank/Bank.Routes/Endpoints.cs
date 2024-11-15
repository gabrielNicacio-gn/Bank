using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Bank.Application.DTOs.Request;
using Bank.Bank.Domain.Interfaces;
using Bank.Bank.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Bank.Routes
{
    public static class Endpoints
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapPost("/", ([FromServices]ITokenService tokenService,[FromBody]UserLogin user) =>
            {
                var result = tokenService.GenerateToken(new User
                {
                    Email = user.Email
                });
                return Results.Ok(new{token=result});
            });
        }
    }
}