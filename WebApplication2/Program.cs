using Microsoft.Data.SqlClient;
using WebChronicles.Controllers.Business;
using WebChronicles.Controllers.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register data and business classes 
builder.Services.AddScoped<AuthorData>();
builder.Services.AddScoped<StoryData>();
builder.Services.AddScoped<AuthorBusiness>();
builder.Services.AddScoped<StoryBusiness>();

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

//app.UseMvc(routeBuilder => {
    
//});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

// initialize the database with dummy data if its empty
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
