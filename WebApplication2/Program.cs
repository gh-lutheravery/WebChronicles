using Microsoft.Data.SqlClient;
using WebApplication2.Controllers.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register Data and Business classes for DI
builder.Services.AddScoped<WebApplication2.Controllers.Data.AuthorData>();
builder.Services.AddScoped<WebApplication2.Controllers.Data.StoryData>();
builder.Services.AddScoped<WebApplication2.Controllers.Business.AuthorBusiness>();
builder.Services.AddScoped<WebApplication2.Controllers.Business.StoryBusiness>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

using (var connection = new SqlConnection("data source=DESKTOP-7T1RFUV\\SQLEXPRESS;initial catalog=WebWriter;trusted_connection=true;TrustServerCertificate=true"))
{
    connection.Open();
    // check if the database is empty
    var checkCommand = new SqlCommand("SELECT COUNT(*) FROM Authors", connection);
    int authorCount = (int)checkCommand.ExecuteScalar();
    if (authorCount == 0)
    {
        DataInit.InsertAuthors(connection);
        DataInit.InsertStories(connection);
    }
}

app.Run();
