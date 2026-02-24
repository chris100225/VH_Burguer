using VHBurguer.Applications.Regras;
using VHBurguer.Domains;
using VHBurguer.DTOs.PromocaoDto;
using VHBurguer.Exceptions;
using VHBurguer.Interfaces;

namespace VHBurguer.Applications.Services
{
    public class PromocaoService
    {

        private readonly IPromocaoRepository _repository;

        public PromocaoService(IPromocaoRepository repository)
        {
            _repository = repository;
        }

        public List<Promocao> Listar()
        {
            List<Promocao> promocoes = _repository.Listar();

            List<LerPromocaoDto> promocaoDtos = promocoes.Select(promocao => new LerPromocaoDto
            {
                PromocaoID = promocao.PromocaoID,
                Nome = promocao.Nome,
                DataExpiracao = promocao.DataExpiracao,
                StatusPromocao = promocao.StatusPromocao,
            }).ToList();
            return promocaoDtos;
        }
        public LerPromocaoDto ObterPorId(int id)
        {
            Promocao promocao = _repository.ObterPorId(id);

            if (promocao == null)
            {
                throw new DomainException("Promocao nao encontrada");
            }

            LerPromocaoDto promocaoDto = new LerPromocaoDto
            {
                PromocaoID = promocao.PromocaoID,
                Nome = promocao.Nome,
                DataExpiracao = promocao.DataExpiracao,
                StatusPromocao = promocao.StatusPromocao,
            };
            return promocaoDto;
        }

        private static void ValidarNome(string Nome)
        {
            if (string.IsNullOrWhiteSpace(Nome))
            {
                throw new DomainException("Nome é obrigatorio");
            }
        }

        public void Adicionar(CriarPromocaoDto promocaoDto)
        {
            ValidarNome(promocaoDto.Nome);
            ValidarDataExpiracaoPromocao.ValidarDataExpiracao(promocaoDto.DataExpiracao);
            if (_repository.NomeExiste(promocaoDto.Nome))
            {
                throw new DomainException("promocao existente");
            }

            Promocao promocao = new Promocao
            {
                Nome = promocaoDto.Nome,
                DataExpiracao = promocaoDto.DataExpiracao,
                StatusPromocao = promocaoDto.StatusPromocao,
            };

            _repository.Adicionar(promocao);
        }
    }
}
