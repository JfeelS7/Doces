namespace Doces.Api.Dtos;

public record class DoceDto(
    Guid Id,
    string Nome,
    string Descricao,
    decimal Preco,
    DateOnly DataCriacao,
    DateOnly DataAtualizacao,
    bool Ativo = true
);
