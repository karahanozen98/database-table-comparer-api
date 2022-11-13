using DatabaseOperations.DBContext;
using DatabaseOperations.Services.GenerateDDL;
using DatabaseOperations.Services.TableCompare;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DAContext>();
builder.Services.AddSingleton<TableCompareService>();
builder.Services.AddSingleton <GenerateDDLService>();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
