namespace Doces.Api.Dtos;

public record class AtualizarDoceDto(
    string Nome,
    string Descricao,
    decimal Preco,
    DateOnly DataCriacao,
    bool Ativo = true
);


