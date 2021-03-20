using System;
using Xunit;
using Xramcire.Dokumentieren.Services;
using System.Threading.Tasks;
using System.IO;

namespace Xramcire.Dokumentieren.Tests
{
    public class FileDocumentServiceTest
    {
        [Fact]
        public async Task SaveDocumentAsync()
        {
            await this.CreateDocument();
            var content = await this.GetDocument();
            Assert.NotNull(content);
            await this.DeleteDocument();
        }

        [Fact]
        public async Task GetDocumentAsync()
        {
            await this.CreateDocument();
            var content = await this.GetDocument();
            Assert.NotNull(content);
            await this.DeleteDocument();
        }

        [Fact]
        public async Task DeleteDocumentAsync()
        {
            await this.CreateDocument();
            await this.DeleteDocument();
            var content = await this.GetDocument();
            Assert.Null(content);
        }

        private const string DOCUMENT_NAME = "helloworld.txt";
        private const string DOCUMENT_TEXT = "Hello World!";
        private FileDocumentService documentService = new FileDocumentService();

        private async Task<Stream> GetDocument()
        {
            using (var content = await documentService.GetAsync(DOCUMENT_NAME))
            {
                return content;
            }
        }

        private async Task CreateDocument()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(DOCUMENT_TEXT);
                    writer.Flush();
                    stream.Position = 0;

                    await documentService.SaveAsync(DOCUMENT_NAME, stream);
                }
            }
        }

        private async Task DeleteDocument()
        {
            await documentService.DeleteAsync(DOCUMENT_NAME);
        }
    }
}
