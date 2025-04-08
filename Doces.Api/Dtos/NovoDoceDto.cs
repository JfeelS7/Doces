namespace Doces.Api.Dtos;

public record class NovoDoceDto(
    string Nome,
    string Descricao,
    decimal Preco,
    bool Ativo = true
);
