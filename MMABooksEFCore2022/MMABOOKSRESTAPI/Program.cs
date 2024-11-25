using MMABooksEFClasses.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ADD CORS POLICY - IN A PRODUCTION APP LOCK THIS DOWN!

builder.Services.AddCors(options => { options.AddDefaultPolicy(
    builder =>{ builder.AllowAnyOrigin()
        .WithMethods("Post", "Put", "Delete", "Get", "Options")
        .AllowAnyHeader();
    });
});

// ADDING THE DBCONTEXT TO THE SERVICE
builder.Services.AddDbContext<MMABooksContext>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//Enables the Cors Policy
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
