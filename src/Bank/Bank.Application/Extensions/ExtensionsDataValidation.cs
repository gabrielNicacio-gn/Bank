using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Bank.Bank.Application.Extensions;

public static class ExtensionsDataValidation
{
    public static RouteHandlerBuilder Validate<T>(this RouteHandlerBuilder builder)
    {
        builder.AddEndpointFilter(async (context, @delegate) =>
        {
            var argument = context.Arguments.OfType<T>().FirstOrDefault();
            var validationContext = new ValidationContext(argument!);
            var listErrors = new List<ValidationResult>();
            
            if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(argument!, 
                    validationContext, listErrors, true))
            {
                return Results.BadRequest(new 
                {
                    Errors = listErrors
                });
            }
            return await @delegate(context);
        });
        return builder;
    }
}