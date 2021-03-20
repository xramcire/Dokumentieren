using System.IO;
using System.Threading.Tasks;

namespace Xramcire.Dokumentieren.Services
{
    public interface IDocumentService
    {
        /// <summary>
        /// Creates or replaces the given document.
        /// </summary>
        /// <param name="documentName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task SaveAsync(string documentName, Stream stream);

        /// <summary>
        /// Deletes the requested document.
        /// </summary>
        /// <param name="documentName"></param>
        /// <returns></returns>        
        Task DeleteAsync(string documentName);

        /// <summary>
        /// Returns the requested document.
        /// </summary>
        /// <param name="documentName"></param>
        /// <returns></returns>
        Task<Stream> GetAsync(string documentName);
    }
}
