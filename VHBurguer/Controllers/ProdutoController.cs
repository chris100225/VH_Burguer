using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VHBurguer.Applications.Services;
using VHBurguer.DTOs.ProdutoDto;
using VHBurguer.Exceptions;

namespace VHBurguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {

        private readonly ProdutoService _service;

        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }
        private int ObterUsuarioIdLogado()
        {
            string? idTexto = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(idTexto))
            {
                throw new DomainException("Usuario não autenticado");
            }
            return int.Parse(idTexto);

        }
        [HttpGet]
        public ActionResult<List<LerProdutoDto>> Listar()
        {
            List<LerProdutoDto> produtos = _service.Listar();
            //retur StatusCode(200,produto)
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public ActionResult<LerProdutoDto> ObterPorId(int id)
        {
            LerProdutoDto produto = _service.ObterPorId(id);

            if (produto == null)
            {
                return NotFound();
            }
            return Ok(produto);
        }

        [HttpGet("{id}/imagem")]

        public IActionResult ObterImagem(int id)
        {
            try
            {
                var imagem = _service.ObterImagem(id);
                return File(imagem, "imagem/jpeg");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

