using System.Security.Cryptography;
using System.Text;
using VHBurguer.Domains;
using VHBurguer.DTOs;
using VHBurguer.Exceptions;
using VHBurguer.Interfaces;

namespace VHBurguer.Applications.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository= repository;
        }
        private static LerUsuarioDto LerDto (Usuario usuario)
        {
            LerUsuarioDto lerUsuario = new LerUsuarioDto
            {
                UsuarioID = usuario.UsuarioID,
                Nome = usuario.Nome,
                Email = usuario.Email,
                StatusUsuario = usuario.StatusUsuario ?? true //se não tiver status no banco retorna true para garantir
            };
            return lerUsuario;
            
        }
        public List<LerUsuarioDto> Listar()
        {
            List<Usuario> usuarios = _repository.Listar();
            List<LerUsuarioDto> usuariosDto=usuarios.Select(usuarioBanco=>LerDto(usuarioBanco))
                .ToList();
            return usuariosDto;
        }
        private static void ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new DomainException("Email inválido.");
            }
        }

        private static byte[] HashSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
            {
                throw new DomainException("Senha é obrigatoria");
            }

            using var sha256 =SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        }

        public LerUsuarioDto ObterPorId(int id)
        {
            Usuario usuario = _repository.ObterPorId(id);

            if (usuario == null)
            {

                throw new DomainException("Usuário não existe.");
            }
            return LerDto(usuario);
        }
        public LerUsuarioDto ObterPorEmail(int email)
        {
            Usuario usuario = _repository.ObterPorId(email);

            if (usuario == null)
            {
                throw new DomainException("Usuário não existe.");
            }
            return LerDto(usuario);
        }
    }
}
