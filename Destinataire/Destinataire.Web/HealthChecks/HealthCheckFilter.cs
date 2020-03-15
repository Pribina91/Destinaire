using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Destinataire.Web.HealthChecks
{
    public class HealthChecksFilter : IDocumentFilter
    {
        public const string HealthCheckEndpoint = @"/healthcheck";

        public void Apply(OpenApiDocument openApiDocument, DocumentFilterContext context)
        {
            var pathItem = new OpenApiPathItem();

            var operation = new OpenApiOperation();
            operation.Tags.Add(new OpenApiTag { Name = "ApiStatus" });

            var properties = new Dictionary<string, OpenApiSchema>
            {
                {"status", new OpenApiSchema() {Type = "string"}}, 
                {"errors", new OpenApiSchema() {Type = "array"}}
            };

            var response = new OpenApiResponse();
            response.Content.Add("application/json", new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "object",
                    AdditionalPropertiesAllowed = true,
                    Properties = properties,
                }
            });

            operation.Responses.Add("200", response);
            pathItem.AddOperation(OperationType.Get, operation);
            openApiDocument?.Paths.Add(HealthCheckEndpoint, pathItem);
        }

    }
}
