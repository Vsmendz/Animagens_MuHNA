using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=x

namespace Animagens_MuHNA.BackEnd.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class imgController : ControllerBase
    {
        // GET:
        [HttpGet]
        public List<String> Get()
        {
            var PastaFotos = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
            var Fotos = Directory.GetFiles(PastaFotos);
            var ListaDeFotos = new List<String>();

            foreach (var foto in Fotos)
            {
                var NomeFoto = Path.GetFileName(foto);
                var url = $"/img/{NomeFoto}";
                ListaDeFotos.Add(url);
            }

            return ListaDeFotos;
        }

        // POST: /api/img/upload-lote
        [HttpPost("upload-lote")]
        public async Task<IActionResult> ReceberLoteDeImagens(List<IFormFile> fotos)
        {
            if (fotos == null || fotos.Count == 0)
                return BadRequest("Nenhuma imagem enviada.");

            var pastaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
            if (!Directory.Exists(pastaDestino))
                Directory.CreateDirectory(pastaDestino);

            var urlsSalvas = new List<string>();

            foreach (var foto in fotos)
            {
                if (foto.Length > 0)
                {
                    var extensao = Path.GetExtension(foto.FileName);
                    var nomeUnico = $"{Guid.NewGuid()}{extensao}";
                    var caminhoCompleto = Path.Combine(pastaDestino, nomeUnico);

                    using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                    {
                        await foto.CopyToAsync(stream);
                    }

                    urlsSalvas.Add($"/img/{nomeUnico}");
                }
            }

            return Ok(new
            {
                TotalSalvas = urlsSalvas.Count,
                Imagens = urlsSalvas
            });
        }
    }
}
