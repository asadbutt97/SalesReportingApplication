using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
      builder.Services.AddSession(options =>
      {
          options.Cookie.Name = ".SalesReportingApplication.Session";
          options.IdleTimeout = TimeSpan.FromMinutes(30);
          options.Cookie.HttpOnly = true;
          options.Cookie.IsEssential = true;
      });
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddRazorPages().AddRazorPagesOptions(options => options.Conventions.AddPageRoute("/Login",""));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapRazorPages();
app.UseSession();

app.MapControllers();

app.Run();
