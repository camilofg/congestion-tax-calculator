using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CongestionTaxApi.Filters
{
    public class AddHeaderOperationFilter : IOperationFilter
    {
        private readonly string _parameterName;
        private readonly string _description;
        private readonly string _controllerName;

        public AddHeaderOperationFilter(string parameterName, string description, string controllerName)
        {
            this._parameterName = parameterName;
            this._description = description;
            this._controllerName = controllerName;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            if (context.MethodInfo.DeclaringType.Name.Equals(_controllerName))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = _parameterName,
                    In = ParameterLocation.Header,
                    Description = _description,
                    Required = context.MethodInfo.DeclaringType.Name.Equals(_controllerName),
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                });
            }
        }
    }
}
