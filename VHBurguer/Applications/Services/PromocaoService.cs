using System.Diagnostics;
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

        public List<LerPromocaoDto> Listar()
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

        public void Atualizar(int id, CriarPromocaoDto promocaoDto)
        {
            ValidarNome(promocaoDto.Nome);
            Promocao promocaoBanco=_repository.ObterPorId(id);

            if (promocaoBanco==null)
            {
                throw new DomainException("Promocao não encontrada");
            }

            if (_repository.NomeExiste(promocaoDto.Nome,promocaoIdAtual:id))
            {
                throw new DomainException("Já existe outra promocao com esse nome");
            }

            promocaoBanco.Nome = promocaoDto.Nome;
            promocaoBanco.DataExpiracao = promocaoDto.DataExpiracao;
            promocaoBanco.StatusPromocao= promocaoDto.StatusPromocao;

            _repository.Atualizar(promocaoBanco);
        }

        public void Remover(int id) {
            Promocao promocaoBanco = _repository.ObterPorId(id);

            if (promocaoBanco==null)
            {
                throw new DomainException("promocao nao encontrada");
            }

            _repository.Remover(id);
        }
    }
}
