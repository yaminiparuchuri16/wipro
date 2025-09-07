using CarRental.Models;
using CarRental.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ------------------- AUTHENTICATION -------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "GitHub";
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddGitHub("GitHub", options =>
{
    options.ClientId = "Ov23liNfXIMM0tj6Fzva";
    options.ClientSecret = "2fadc854a418cb78f9077fe60455981143309b5d";
    options.Scope.Add("user:email");

    options.Events.OnTicketReceived = async ctx =>
    {
        var claimsPrincipal = ctx.Principal!;
        var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value ?? "no-email@github.com";
        var name = claimsPrincipal.Identity?.Name ?? "GitHub User";

        var jwtSettings = ctx.HttpContext.RequestServices
            .GetRequiredService<IConfiguration>()
            .GetSection("JwtSettings");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: new[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email)
            },
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        // Return JWT as JSON instead of doing normal cookie redirect
        ctx.Response.ContentType = "application/json";
        await ctx.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
        {
            access_token = token,
            token_type = "Bearer",
            expires_in = 3600
        }));

        // Stop cookie handling, because we re doing our own response
        ctx.HandleResponse();
    };
})
.AddJwtBearer("Bearer", options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
});

// ------------------- SWAGGER -------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[]{}
        }
    });
});

// ------------------- DATABASE -------------------
builder.Services.AddDbContext<CarDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarRentalConnection"));
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// ------------------- MIDDLEWARE -------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseStaticFiles();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

// ------------------- ENDPOINTS -------------------
app.MapGet("/login/github", async (HttpContext context) =>
{
    return Results.Challenge(
        new AuthenticationProperties { RedirectUri = "/" },
        new[] { "GitHub" });
});


app.MapControllers();

app.Run();
