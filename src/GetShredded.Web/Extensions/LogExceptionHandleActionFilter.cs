using System;
using GetShredded.Common;
using GetShredded.Data;
using GetShredded.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace GetShredded.Web.Extensions
{
    public class LogExceptionHandleActionFilter : ExceptionFilterAttribute
    {
        public LogExceptionHandleActionFilter(GetShreddedContext context)
        {
            this.Context = context;
        }

        public GetShreddedContext Context { get; set; }

        public override void OnException(ExceptionContext context)
        {
            var user = context.HttpContext.User.Identity.Name ?? GlobalConstants.Anonymous;
            var exceptionMethod = context.Exception.TargetSite;
            var trace = context.Exception.StackTrace;
            var exception = context.Exception.GetType().Name;
            var time = DateTime.UtcNow.ToLongDateString();

            string message = $"Occurence: {time}  User: {user}  Path:{exceptionMethod}  Trace: {trace}";

            var log = new DatabaseLog
            {
                Content = message,
                Handled = false,
                LogType = exception
            };

            this.Context.Logs.Add(log);
            this.Context.SaveChanges();

            context.ExceptionHandled = true;
            context.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                { "controller", "Home" },
                { "action", "Error" },
                {"area",""}
            });
        }
    }
}
