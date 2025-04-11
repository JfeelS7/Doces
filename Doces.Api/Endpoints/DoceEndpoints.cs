using Doces.Api.Dtos;

namespace Doces.Api.Endpoints
{
    public static class DoceEndpoints
    {
        const string GetDoceById = "GetDoceById";

        private static readonly List<DoceDto> doces = [
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
         
        public static RouteGroupBuilder WebDoceEnpoints(this WebApplication app)
        {
            var group = app.MapGroup("doces")
                           .WithParameterValidation();

            group.MapGet("/doces", () => doces)
                 .WithName("GetAllDoces")
                 .WithOpenApi()
                 .Produces<List<DoceDto>>(StatusCodes.Status200OK)
                 .Produces(StatusCodes.Status500InternalServerError);

            group.MapGet("/doces/{id:guid}", (Guid id) => doces.Find(d => d.Id == id))
                 .WithName(GetDoceById)
                 .WithOpenApi()
                 .Produces<DoceDto>(StatusCodes.Status200OK)
                 .Produces(StatusCodes.Status404NotFound)
                 .Produces(StatusCodes.Status500InternalServerError);

            group.MapPost("/doces", (NovoDoceDto novoDoce) => 
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

            group.MapPut("/doces/{id:guid}", (Guid id, AtualizarDoceDto atualizarDoce) =>
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
                doces[index].DataCriacao,
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

            group.MapDelete("/doces/{id:guid}", (Guid id) =>
            {
                doces.RemoveAll(d => d.Id == id);

                return Results.NoContent();
            }).WithName("DeleteDoce")
              .WithOpenApi()
              .Produces(StatusCodes.Status204NoContent)
              .Produces(StatusCodes.Status404NotFound)
              .Produces(StatusCodes.Status500InternalServerError);

            return group; 
        }
    }
}