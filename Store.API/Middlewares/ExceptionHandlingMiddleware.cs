namespace Store.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.Redirect($"/Error/ErrorMessage?message={ex.Message}");
            }
        }

        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate)
        {
            this.next = requestDelegate;
        }
    }
}
