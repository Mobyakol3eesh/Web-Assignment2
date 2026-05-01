internal static class WebApplicationExtensions
{
    internal static WebApplication UseApplicationMiddleware(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tuna League API v1");
        });

        app.UseCors("AllowSwagger");
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }

    
}