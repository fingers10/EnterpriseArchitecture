using Fingers10.EnterpriseArchitecture.API.Dtos;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fingers10.EnterpriseArchitecture.API.OperationFilters
{
    public class CreateAuthorOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.OperationId != "CreateAuthor")
            {
                return;
            }

            operation.RequestBody.Content.Add(
                "application/vnd.fingers10.authorforcreationwithdateofdeath+json",
                new OpenApiMediaType()
                {
                    Schema = context.SchemaGenerator.GenerateSchema(
                        typeof(AuthorForCreationWithDateOfDeathDto),
                        context.SchemaRepository)
                });
        }
    }
}
