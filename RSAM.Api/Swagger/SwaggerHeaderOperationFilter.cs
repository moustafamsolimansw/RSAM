using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RSAM.Api.Swagger;

public class SwaggerHeaderOperationFilter
{ }
    /*: IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters.Add(new OpenApiParameter // Accept-Language-Header
        {
            Name = "Accept-Language",
            In = ParameterLocation.Header,
            Description = "Language preference (en , ar)",
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Default = new OpenApiString("ar")
            }
        });
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "Content-Encoding",
            In = ParameterLocation.Header,
            Description = "Content Encoding",
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string",
                Default = new OpenApiString("gzip")
            }
        });
    }
}*/

