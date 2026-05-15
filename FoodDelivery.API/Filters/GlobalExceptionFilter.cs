using FoodDelivery.API.Exceptions;
using FoodDelivery.API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FoodDelivery.API.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is NotFoundException)
        {
            context.Result = new NotFoundObjectResult(new
            {
                message = context.Exception.Message
            });

            context.ExceptionHandled = true;
        }

        else if (context.Exception is BadRequestException)
        {
            context.Result = new BadRequestObjectResult(new
            {
                message = context.Exception.Message
            });

            context.ExceptionHandled = true;
        }

        else if (context.Exception is ConflictException)
        {
            context.Result = new ObjectResult(new
            {
                message = context.Exception.Message
            })
            {
                StatusCode = 409
            };

            context.ExceptionHandled = true;
        }

        else if (context.Exception is ForbiddenException)
        {
            context.Result = new ObjectResult(new
            {
                message = context.Exception.Message
            })
            {
                StatusCode = 403
            };

            context.ExceptionHandled = true;
        }

        else
        {
            context.Result = new ObjectResult(new
            {
                message = "Internal Server Error"
            })
            {
                StatusCode = 500
            };

            context.ExceptionHandled = true;
        }
    }
}