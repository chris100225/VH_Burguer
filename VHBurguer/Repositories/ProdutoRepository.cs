using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using VHBurguer.Contexts;
using VHBurguer.Domains;
using VHBurguer.Interfaces;

namespace VHBurguer.Repositoreis.UsuarioDTO
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly VH_BurguerContext _context;
        public ProdutoRepository(VH_BurguerContext context)
        {
            _context = context;
        }
        public List<Produto> Listar()
        {
            List<Produto> prdutos = _context.Produto
                //busca produto e para cada produto, traz as suas categorias
                .Include(p => p.Categoria)
                //busca produto e para cada produto, traz as suas Usuario
                .Include(p => p.Usuario)
                .ToList();
            return prdutos;
        }

        public Produto ObterPorId(int id)
        {
            Produto? produto = _context.Produto
                .Include(pDb => pDb.Categoria)
                .Include(pDb => pDb.Usuario)
                //Procura no banco (aux produtoDb) e verifica se o ID do Produto no
                //banco é igual ao id passado como paramentro no método ObterPorId
                .FirstOrDefault(pDb => pDb.ProdutoID == id);
            return produto;
        }
        public bool NomeExiste(string nome, int? produtoIdAtual = null)
        {
            //AsQueryble() -> monta a consulta para executar passo a passo
            //monta a consulta na tabela produto
            //não execulta nada no bnaco ainda
            var produtoConsultado = _context.Produto.AsQueryable();

            //se o produto atual tiver valor, então atualiza o produto

            if (produtoIdAtual.HasValue)
            {
                produtoConsultado = produtoConsultado.Where(p => p.ProdutoID !=
                produtoIdAtual.Value);
            }

            return produtoConsultado.Any(p => p.Nome == nome);
        }

        public byte[] ObterImagem(int id)
        {
            var produto = _context.Produto
                .Where(p => p.ProdutoID == id)
                .Select(p => p.Imagem)
                .FirstOrDefault();

            return produto;
        }

        public void Adicionar(Produto produto, List<int> categoriaIds)
        {
            List<Categoria> categorias = _context.Categoria
                .Where(categoria => categoriaIds.Contains(categoria.CategoriaID))
                .ToList();// Contains -> retorna true so houver o registro

            produto.Categoria = categorias; //adiciona as categorias incluidas ao produto

            _context.Produto.Add(produto);
            _context.SaveChanges();
        }

        public void Atualizar(Produto produto, List<int> categoriaIds)
        {
            Produto? produtoBanco = _context.Produto
                .Include(p => p.Categoria)
                .FirstOrDefault(pAux => pAux.ProdutoID == produto.ProdutoID);
            if (produtoBanco == null)
            {
                return;
            }

            produtoBanco.Nome = produto.Nome;
            produtoBanco.Preco = produto.Preco;
            produtoBanco.Descricao = produto.Descricao;

            if (produto.Imagem != null && produto.Imagem.Length > 0)
            {
                produtoBanco.Imagem = produto.Imagem;
            }

            if (produto.StatusProduto.HasValue)
            {
                produtoBanco.StatusProduto = produto.StatusProduto;
            }

            //Buscacategorias no banco com o id igual das categorias que 
            //vieram da requisição/front

            var categorias = _context.Categoria
                .Where(categoria => categoriaIds.Contains(categoria.CategoriaID))
                .ToList();
            // Clera() -> Remove as ligações atuais entre o produto e as categorias
            // ele não apaga a categoria do banco, só remove o vínculo com a tabela
            //ProdutoCategoria
            produtoBanco.Categoria.Clear();

            foreach (var categoria in categorias)
            {
                produtoBanco.Categoria.Add(categoria);
            }

            _context.SaveChanges();
        }

        public void Remover(int id)
        {
            Produto? produto = _context.Produto.FirstOrDefault(p => p.ProdutoID == id);

            if (produto == null)
            {
                return;
            }

            _context.Produto.Remove(produto);
            _context.SaveChanges();
        }
    }
}
