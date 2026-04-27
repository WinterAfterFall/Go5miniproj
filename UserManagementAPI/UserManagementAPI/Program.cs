using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
// 1. เพิ่มการตั้งค่า JWT (วางไว้ก่อน builder.Build())
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            // กุญแจลับสำหรับเข้ารหัส (ต้องยาว 32 ตัวอักษรขึ้นไป)
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecretKey12345678901234567890"))
        };
    });

// --- 1. เชื่อมต่อ Database ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- [เพิ่มส่วนนี้] ตั้งค่า CORS เพื่อให้ Angular คุยกับ API ได้ ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.AllowAnyOrigin() // หรือระบุ .WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 2. เปิดใช้งาน Swagger UI ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- [เพิ่มส่วนนี้] เรียกใช้งาน CORS (ต้องวางก่อน Authorization และ MapControllers) ---
app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();