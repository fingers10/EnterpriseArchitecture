using Fingers10.EnterpriseArchitecture.API.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fingers10.EnterpriseArchitecture.API.OperationFilters
{
    public class GetBookOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.OperationId != "GetBookForAuthor")
            {
                return;
            }

            operation.Responses[StatusCodes.Status200OK.ToString()].Content.Add(
                "application/vnd.fingers10.bookwithconcatenatedauthorname.hateoas+json",
                new OpenApiMediaType()
                {
                    Schema = context.SchemaGenerator.GenerateSchema(
                        typeof(BookWithConcatenatedAuthorNameDto),
                        context.SchemaRepository)
                });
        }
    }
}
