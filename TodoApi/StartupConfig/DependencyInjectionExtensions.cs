using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TodoLibrary.DataAccess;

using System.Reflection;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TodoApi.StartupConfig;

public static class DependencyInjectionExtensions
{
    public static void AddStandardServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.AddSwaggerServices();
    }

    public static void AddAuthServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(opts =>
        {
            opts.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        });

        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
                    ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(
                            builder.Configuration.GetValue<string>("Authentication:SecretKey")))
                };
            });
    }
    public static void AddHealthChecksServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddSqlServer(builder.Configuration.GetConnectionString("Default"));
    }

    public static void AddCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddSingleton<ITodoData, TodoData>();
    }
    
    private static void AddSwaggerServices(this WebApplicationBuilder builder)
    {
        var securityScheme = new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Description = "JWT Authorization header info using bearer tokens",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };

        var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                     new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearerAuth"
                        }
                    },
                    new string[] {}
                }
            };

        builder.Services.AddSwaggerGen(opts =>
        {
            // Seguridad
            opts.AddSecurityDefinition("bearerAuth", securityScheme);
            opts.AddSecurityRequirement(securityRequirement);

            // XML Comments
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));

            // Comentarios API SwaggerDoc
            opts.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Titulo de la Api",
                Description = "Descripcion de la Api",
                TermsOfService = new Uri("https://www.biodevices.com.ec"),
                Contact = new OpenApiContact()
                {
                    Name = "BioDevices Helpdesk",
                    Email = "telemercadeo@biodevices.com.ec"
                },
                License = new OpenApiLicense()
                {
                    Name = "Informacion de Licencia"
                }
            });

            opts.EnableAnnotations();
            //opts.DocumentFilter<SwaggerOrderTagsDocumentFilter>();
            opts.DocumentFilter<OrderTagsDocumentFilter>();
            //opts.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
            //opts.TagActionsBy(api => api.HttpMethod); // Agrupa por Method
            //opts.OrderActionsBy(api => $"{api.HttpMethod}"); // Agrupa por Method
            //opts.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.DisplayName}_{apiDesc.HttpMethod}");
            //opts.OrderActionsBy((apiDesc) => $"{apiDesc.HttpMethod }_{apiDesc.ActionDescriptor.DisplayName}");

        });
    }
}


public class SwaggerOrderTagsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (swaggerDoc.Tags == null) swaggerDoc.Tags = new List<OpenApiTag>();

        var swaggerList = context.ApiDescriptions.
            Select(x => x.ActionDescriptor.EndpointMetadata).
            Select(x => x.OfType<SwaggerOperationAttribute>()).
            SkipWhile(x => x == null).
            ToList();

        foreach (IEnumerable<SwaggerOperationAttribute>? item in swaggerList)
        {
            foreach (SwaggerOperationAttribute? item2 in item.Where(x => x.Tags.Length > 0))
            {
                foreach (var t in item2.Tags)
                {
                    if (!swaggerDoc.Tags.Any(x => x.Name == t))
                    {
                        swaggerDoc.Tags.Add(new OpenApiTag
                        {
                            Name = t,
                            Description = t
                        });
                    }
                }
            }
        }

        swaggerDoc.Tags = swaggerDoc.Tags.OrderBy(x => x.Description).ToList();
    }
}

public class OrderTagsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Tags = swaggerDoc.Tags.OrderBy(x => x.Name).ToList();
    }
}