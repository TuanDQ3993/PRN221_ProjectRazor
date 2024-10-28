
﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

using ProjectPRN221_LIBManagement.Models;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(); // Thêm session
builder.Services.AddDbContext<PRN221_LibContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("value"));
});
builder.Services.AddRazorPages();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 209715200; // Hạn chế kích thước file tối đa nếu cần (đây là khoảng 200MB)
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // Kích hoạt session

app.UseAuthorization();

app.MapRazorPages();

app.Run();
