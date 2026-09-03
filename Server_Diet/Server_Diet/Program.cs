using System.Text;
using Core.Mapping;
using Core.Repository;
using Core.Services;
using Data.DataRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Resend;
using Server.date;
using Server.Service;
using Service;
using Web_Api.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Configure JWT Bearer Authentication and token validation rules for the application
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true, // בודק שהטוקן לא פג תוקף
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
//חיבור למסד
builder.Services.AddDbContext<DietContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DietDb"))
);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<DietContext>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IUserMealRepository, UserMealRepository>();
builder.Services.AddScoped<IUserMealService,UserMealService>();
builder.Services.AddScoped<IUserService,  UserService>();
builder.Services.AddScoped<IFoodService, FoodService>();
builder.Services.AddHttpClient<IFoodAnalysisService, GeminiFoodAnalysisService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddHttpContextAccessor();
// הזרקת השירות בצורה נכונה
builder.Services.AddResend(options =>
{
    options.ApiToken = builder.Configuration["ResendApiKey"];
});
builder.Services.AddSwaggerGen();
// הוספת ה-AI Service עם HttpClient מנוהל

var app = builder.Build();
app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// הוספת הגדרה מפורשת למיקום הקבצים הסטטיים

app.UseStaticFiles();
app.UseCors("AllowAngular");
app.UseHttpsRedirection();


app.UseAuthentication(); // 1. מי אתה? (בדיקת הטוקן) 
app.UseAuthorization();  // 2. האם מותר לך לעשות את זה? (הרשאות)
app.MapControllers();

app.Run();
