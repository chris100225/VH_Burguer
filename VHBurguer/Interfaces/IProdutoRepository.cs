using Microsoft.AspNetCore.Components.Web;
using VHBurguer.Domains;

namespace VHBurguer.Interfaces
{
    public interface IProdutoRepository
    {
        List<Produto> Listar();
        Produto ObterporId(int id);
        byte[] ObterImagem(int id);
        bool NomeExiste(string nome, int? produtoIdAtual=null);
        void Adicionar (Produto produto, List<int> categoiaIds);

        void Atualizar(Produto produto, List<int> categoriaIds);

        void Remover(int id);
    }
}
