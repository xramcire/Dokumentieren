using System;
using System.IO;
using System.Threading.Tasks;

namespace Xramcire.Dokumentieren.Services
{
    /// <summary>
    /// Reads, Write, and Delete files from a remote bucket store.
    /// </summary>
    public class BucketDocumentService : IDocumentService
    {
        //
        //  This implementaion will need a client lib (AWS, Azure, Google)
        //  or raw HTTP requests. Maybe one for each...
        //  Authentication will need configuration and or a key vault.
        //  Fun for another day.
        //
        public Task DeleteAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> GetAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(string name, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
