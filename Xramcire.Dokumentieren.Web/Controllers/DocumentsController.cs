using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Xramcire.Dokumentieren.Services;

namespace Xramcire.Dokumentieren.Controllers
{
    [Route("documents")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService documentService;

        public DocumentsController(IDocumentService documentService)
        {
            this.documentService = documentService;
        }
        
        [HttpOptions]
        [ProducesResponseType(200)]
        public IActionResult Options()
        {
            Response.Headers.Add("Allow", "OPTIONS, GET, HEAD, POST, PUT");
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostAsync()
        {
            var file = this.Request.GetFirstFile();

            if (file == null)
            {
                return NotFound();
            }

            using (var stream = file.OpenReadStream())
            {
                await this.documentService.SaveAsync(file.FileName, stream);
            }

            return Ok();
        }

        [HttpDelete("{documentName}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string documentName)
        {
            await this.documentService.DeleteAsync(documentName);
            return Ok();
        }

        [HttpPut("{documentName}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PutAsync([FromRoute] string documentName)
        {
            var file = this.Request.GetFirstFile();

            if (file == null)
            {
                return NotFound();
            }

            using (var stream = file.OpenReadStream())
            {
                await this.documentService.SaveAsync(documentName, stream);
            }

            return Ok();
        }

        [HttpGet("{documentName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAsync([FromRoute] string documentName)
        {
            Stream stream = await this.documentService.GetAsync(documentName);

            if (stream == null)
                return NotFound();

            string mimeType = documentName.GetMimeType();

            return File(stream, mimeType);
        }
    }
}