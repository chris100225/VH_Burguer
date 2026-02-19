using Microsoft.IdentityModel.Tokens;
using VHBurguer.Contexts;
using VHBurguer.Domains;
using VHBurguer.Interfaces;

namespace VHBurguer.Repositoreis
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public readonly VH_BurguerContext _context;

        public UsuarioRepository(VH_BurguerContext context)
        {
            _context = context;
        }

        public List<Usuario> Listar()
        {
            return _context.Usuario.ToList();
        }

        public Usuario? ObterPorId(int id)
        {
            //Find é melhor performaticamente para chaves primária
            return _context.Usuario.Find(id);
        }

        public Usuario? ObterPorEmail(string email)
        {
            // FirstOrDefault -> retorna nosso usuário dobanco
            return _context.Usuario.FirstOrDefault(u => u.Email == email);
        }

        public bool EmailExiste(string email)
        {
            //Any retorna true ou false para validar a exis
            //tencia de um usuario com o e-mail
            return _context.Usuario.Any(u => u.Email == email);
        }

        public void Adicionar(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            _context.SaveChanges();
        }

        public void Atualizar(Usuario usuario)
        {
            Usuario? usuarioBanco = _context.Usuario.FirstOrDefault(u => u.UsuarioID == usuario.UsuarioID);
            if (usuarioBanco == null)
            {
                return;
            }
            usuarioBanco.Nome = usuario.Nome;
            usuarioBanco.Email = usuario.Email;
            usuarioBanco.Senha = usuario.Senha;

            _context.SaveChanges();
        }

        public void Remover(int id)
        {
            Usuario? usuario = _context.Usuario.FirstOrDefault(u => u.UsuarioID == id);

            if (usuario == null)
            {
                return;
            }

            _context.Usuario.Remove(usuario);
            _context.SaveChanges();
        }

    }
}
