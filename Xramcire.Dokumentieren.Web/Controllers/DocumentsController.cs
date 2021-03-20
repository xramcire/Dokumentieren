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

//Senior Software Engineer Project
//We ask interested candidates to implement a document upload service.
//The project is an important part of our recruiting process, and it’s designed to give candidates the
//ability to showcase their skills in an extended time, low-stress environment.
//We’re interested in how you write organized, structured, tested code. There is no minimum or
//maximum project size. We’re not necessarily looking at the total quantity of functionality but rather the
//quality, style, and characteristics.
//Project Requirements:
// Build a document management service API using .NET 5
//o Supports CRUD operations
// Write a README in your project that explains:
//o What you built and the architecture decisions you made
//o Thoughts on testability, scalability, and future improvements you’d make
// Make the project available on GitHub or another Git service
//Project Constraints:
// Follow standard REST principles
// Include unit tests and integration tests
// Does not need authentication/authorization
// Should be thread-safe and handle multiple concurrent requests
//Final Thoughts:
// The code you write is owned by you – feel free to make it part of your online portfolio
// Please do not reference Free Market Music in your project as we don’t want others plagiarizing
//your work
// Once completed, let us know the URL of the Git repository
