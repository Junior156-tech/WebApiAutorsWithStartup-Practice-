using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using WebApiAutoresConStartup.DTOs;
using WebApiAutoresConStartup.Middleware;
using WebApiAutoresConStartup.Services;
using WebApiAutoresConStartup.Utility;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace WebApiAutoresConStartup
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opciones =>
            {
                opciones.Conventions.Add(new SwaggerAgrupaPorVersion());
            })
            .AddJsonOptions(x =>
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();

            services.AddResponseCaching();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["LlaveJWT"])),
                    ClockSkew = TimeSpan.Zero
                });



            services.AddDbContext<ApplicationDbContext>(option =>
                 option.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiAutoresConStartup",
                    Version = "v1",
                    Description = "Este es un webApi para libros",
                    Contact = new OpenApiContact() { Email = "JuniorCarpenter@gmail.com", Name = "Junior Carpenter", Url = new Uri("https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/27148912#overview"),
                    },
                    License = new OpenApiLicense() {  Name = "MITT" }
                });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "WebApiAutoresConStartup", Version = "v2" });
                c.OperationFilter<AgregarParametroHATEOAS>();
                c.OperationFilter<AgregarParametroXVersion>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });


                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type =ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

                var archivo = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaXML = Path.Combine(AppContext.BaseDirectory, archivo);
                c.IncludeXmlComments(rutaXML);
            });


            services.AddAutoMapper(typeof(Startup));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("EsAdmin", politica => politica.RequireClaim("EsAdmin"));
            });

            services.AddDataProtection();


            services.AddCors(opciones =>
            {
                opciones.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("").AllowAnyMethod().AllowAnyHeader()
                    .WithExposedHeaders(new string[] { "cantidadTotalRegistros" });
                });
            });

            services.AddTransient<HashService>();
            services.AddTransient<generadorEnlaces>();  
            services.AddTransient<HATEOASAutorFilterAttribute>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights:ConnectionString"]);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {

            //app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
            //app.UseLoguearRespuestaHTTP();

            //app.Map("/ruta1", app =>
            //{
            //    app.Run(async contexto =>
            //    {
            //        await contexto.Response.WriteAsync("Estoy interceptando la tuberia");
            //    });
            //});


            if (env.IsDevelopment())
            {
               
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiAutoresConStartup v1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApiAutoresConStartup v2");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
