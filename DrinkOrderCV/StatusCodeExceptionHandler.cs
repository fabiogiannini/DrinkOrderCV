using Newtonsoft.Json;

namespace DrinkOrderCV.Web
{
    public class StatusCodeExceptionHandler
    {
        private readonly RequestDelegate request;

        public StatusCodeExceptionHandler(RequestDelegate pipeline)
        {
            this.request = pipeline;
        }

        public Task Invoke(HttpContext context) => this.InvokeAsync(context);

        async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.request(context);
            }
            catch (HttpStatusException exception)
            {
                context.Response.StatusCode = (int)exception.Status; 
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { Message = exception.Message }));
            }
        }
    }
}