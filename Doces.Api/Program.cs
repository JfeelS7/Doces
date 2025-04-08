using Doces.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
    
}

const string GetDoceById = "GetDoceById";

List<DoceDto> doces = [
    new(Guid.NewGuid(),
        "Doce de Leite",
        "Doce de leite caseiro",
        10.00m,
        new DateOnly(2025, 1, 1),
        new DateOnly(2025, 1, 1)
        ),

    new(Guid.NewGuid(),
        "Bolo de Cenoura",
        "Bolo de cenoura com cobertura de chocolate",
        15.00m,
        new DateOnly(2025, 1, 1),
        new DateOnly(2025, 1, 1)
        ),

    new(Guid.NewGuid(),
        "Pudim",
        "Pudim de leite condensado",
        12.00m,
        new DateOnly(2025, 1, 1),
        new DateOnly(2025, 1, 1)
        )
];

app.MapGet("/doces", () => doces)
    .WithName("GetAllDoces")
    .WithOpenApi()
    .Produces<List<DoceDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapGet("/doces/{id:guid}", (Guid id) => doces.Find(d => d.Id == id))
    .WithName(GetDoceById)
    .WithOpenApi()
    .Produces<DoceDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapPost("/doces", (NovoDoceDto novoDoce) => 
{
    DoceDto doce = new(
        Guid.NewGuid(),
        novoDoce.Nome,
        novoDoce.Descricao,
        novoDoce.Preco,
        DateOnly.FromDateTime(DateTime.Now),
        DateOnly.FromDateTime(DateTime.Now),
        novoDoce.Ativo
    );
    
    doces.Add(doce);
    
    return Results.CreatedAtRoute(GetDoceById,new {id = doce.Id}, doce);
}).WithName("CreateDoce")
    .WithOpenApi()
    .Produces<DoceDto>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status500InternalServerError);

app.MapPut("/doces/{id:guid}", (Guid id, AtualizarDoceDto atualizarDoce) =>
{
    var index = doces.FindIndex(d => d.Id == id);
    
    if (index == -1)
    {
        return Results.NotFound();
    }

    doces[index] = new DoceDto(
        id,
        atualizarDoce.Nome,
        atualizarDoce.Descricao,
        atualizarDoce.Preco,
        atualizarDoce.DataCriacao,
        DateOnly.FromDateTime(DateTime.Now),
        atualizarDoce.Ativo
    );

    return Results.NoContent();
}).WithName("UpdateDoce")
    .WithOpenApi()
    .Produces<DoceDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status500InternalServerError);

app.UseHttpsRedirection();

app.Run();