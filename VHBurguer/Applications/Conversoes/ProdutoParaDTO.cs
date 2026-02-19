using VHBurguer.Domains;
using VHBurguer.DTOs.ProdutoDto;

namespace VHBurguer.Applications.Conversoes
{
    public class ProdutoParaDTO
    {
        public static LerProdutoDto ConverterParaDto(Produto produto)
        {
            return new LerProdutoDto
            {
                ProdutoID = produto.ProdutoID,
                Nome = produto.Nome,
                Preco = produto.Preco,
                Descricao = produto.Descricao,
                StatusProduto = produto.StatusProduto,

                CategoriaIds = produto.Categoria.Select(c => c.CategoriaID).ToList(),

                Categoria = produto.Categoria.Select(c => c.Nome).ToList(),

                UsuarioID = produto.UsuarioID,
                UsuarioNome = produto.Usuario.Nome,
                UsuarioEmail = produto.Usuario.Email
            };
        }
    }
}
