using System.ComponentModel.DataAnnotations;

namespace Doces.Api.Dtos;

public record class NovoDoceDto(
    [Required][StringLength(60)]string Nome,
    [Required][StringLength(100)]string Descricao,
    [Required][Range(1, 500)]decimal Preco,
    bool Ativo = true
);
