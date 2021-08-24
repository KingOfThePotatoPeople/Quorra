using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Quorra.Utilities.Filters
{
    public class GuardNullAttribute : ActionFilterAttribute
    {

        private readonly Func<Dictionary<string, object>, bool> _validate;
 
        public GuardNullAttribute() : this(arguments => 
            arguments.ContainsValue(null))
        { }
 
        public GuardNullAttribute(Func<Dictionary<string, object>, bool> checkCondition)
        {
            _validate = checkCondition;
        }
 
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (_validate((Dictionary<string, object>) context.ActionArguments))
            {
                context.Result = new BadRequestObjectResult("The data model is null.");
            }
        }

    }
}