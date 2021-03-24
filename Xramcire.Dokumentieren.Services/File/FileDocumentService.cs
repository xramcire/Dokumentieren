using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Xramcire.Dokumentieren.Services
{
    /// <summary>
    /// Reads, Write, and Delete files from the local file system.
    /// </summary>
    public class FileDocumentService : IDocumentService
    {
        private readonly IDocumentLockService lockService;

        public FileDocumentService(IDocumentLockService lockService)
        {
            this.lockService = lockService;
            Directory.CreateDirectory(GetDocumentRoot());
        }

        public async Task DeleteAsync(string documentName)
        {
            //
            //  This method is not async but any method I know to make
            //  is appear so will only make performance worse. Thread.Run etc...
            //  Access to the local file system is only paritally async-able.
            //
            string blobPath = GetDocumentBlobPath(documentName);

            this.lockService.GetLock(documentName, () => {
                File.Delete(blobPath);
            });
        }

        public async Task<Stream> GetAsync(string documentName)
        {
            string blobPath = GetDocumentBlobPath(documentName);

            if (!File.Exists(blobPath))
                return null;

            return await this.lockService.GetLock(documentName, async () =>
            {
                await using (MemoryStream ret = new MemoryStream())
                {
                    await using (var fileStream = File.OpenRead(blobPath))
                    {
                        await fileStream.CopyToAsync(ret);
                        return ret;
                    }
                }
            });
        }

        public async Task SaveAsync(string documentName, Stream stream)
        {
            string blobPath = GetDocumentBlobPath(documentName);

            await this.lockService.GetLock(documentName, async () => {
                File.Delete(blobPath);

                await using (var fileStream = File.Create(blobPath))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    await stream.CopyToAsync(fileStream);
                }
            });
        }

        /// <summary>
        /// Returns the root folder where documents are stored.
        /// </summary>
        private string GetDocumentRoot()
        {
            //
            //  This should be a configuration value located not in the folder with the web site.
            //
            return $"{AppDomain.CurrentDomain.BaseDirectory}/documents/";
        }

        /// <summary>
        /// Returns the path to the given document name.
        /// </summary>
        private string GetDocumentBlobPath(string documentName)
        {
            return $"{GetDocumentRoot()}{documentName}";
        }
    }
}
