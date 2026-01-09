#region "Declaration & Usings

using HRMS_Backend.Hubs;
using HRMS_Backend.Model;
using HRMS_Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

#endregion "Declaration & Usings


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region "Cross Origin Access"

builder.Services.AddCors(options =>
{
    options.AddPolicy("defaultcorspolicy", policy =>
    {
        //policy.WithOrigins("http://192.168.166.154:4904") // Your frontend URL for production
        policy.WithOrigins("http://localhost:53546") // Your frontend URL for development
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // SignalR needs this
    });
});

#endregion "Cross Origin Access"

#region "Newtonsoft JSON

builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

#endregion "Newtonsoft JSON

#region "SignalR Configuration"

builder.Services.AddSignalR();

#endregion "SignalR Configuration"

builder.Services.AddMvc();

#region "API Versioning"

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
});

#endregion "API Versioning"

#region "Connection b/w Interface & Class"

//builder.Services.AddTransient<IMailService, MailService>();

#endregion "Connection b/w Interface & Class"

#region "Swagger Documentation & JWT Authentication"

builder.Services.AddSwaggerGen(SwaggerGen =>
{

    #region "JWT Bearer Token in Swagger"

    SwaggerGen.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });

    SwaggerGen.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{ }
        }
    });

    #endregion "JWT Bearer Token in Swagger"

    #region "Swagger Documentation"

    SwaggerGen.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "SIUT HRMS API Project",
            Description = "SIUT HRMS API Project for Web & Android Application",
            Version = "v1",
            Contact = new Microsoft.OpenApi.Models.OpenApiContact
            {
                Name = "Abdul Kabeer Bhatti",
                Email = "akabeer919@gmail.com",
            }
        });

    //SwaggerGen.SwaggerDoc("v2",
    //    new Microsoft.OpenApi.Models.OpenApiInfo
    //    {
    //        Title = "ZulAssets Web API Project",
    //        Description = "ZulAssets Web API Project for Application",
    //        Version = "v2",
    //        Contact = new Microsoft.OpenApi.Models.OpenApiContact
    //        {
    //            Name = "Abdul Kabeer Bhatti",
    //            Email = "abdul.kabeer@zultec.com",
    //        },
    //    });

    SwaggerGen.ResolveConflictingActions(a => a.FirstOrDefault());

    SwaggerGen.OperationFilter<RemoveVersionFromParameter>();
    SwaggerGen.DocumentFilter<ReplaceVersionWithExactValueInPath>();

    var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

    //Error throughing due to no data in xml file
    SwaggerGen.IncludeXmlComments(filePath);

    #endregion "Swagger Documentation"

});

#region "Token Expiration"

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = JWTBuilder.GetValidationParameters();
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token=Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

#endregion "Token Expiration"

#endregion "Swagger Documentation & JWT Authentication"

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

}

app.UseSwagger(c =>
{
    c.SerializeAsV2 = true;
});

#region "Swagger URL Change

app.UseSwaggerUI(c =>
{
    c.DocumentTitle = "SIUT HRMS API Project";
    c.SwaggerEndpoint($"/swagger/v1/swagger.json", "SIUT HRMS API Project");
    //c.SwaggerEndpoint($"/swagger/v2/swagger.json", "ZulAssets Web API Project"); 
    c.RoutePrefix = string.Empty;
});

#endregion "Swagger URL Change

app.UseHttpsRedirection();

app.UseRouting();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/Uploads"
});

app.UseCors("defaultcorspolicy");

#region "JWT Authentication & Authorization"

app.UseAuthentication();
app.UseAuthorization();

#endregion "JWT Authentication & Authorization"

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

#region SignalR Hubs Connections

app.MapHub<DashboardHub>("/dashboardHub");
app.MapHub<ChatMessageHub>("/chatMessageHub");

#endregion

app.Run();