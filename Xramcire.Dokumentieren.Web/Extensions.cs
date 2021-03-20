using Microsoft.AspNetCore.Http;
using MimeTypes;

namespace Xramcire.Dokumentieren
{
    internal static class Extensions
    {
        internal static IFormFile GetFirstFile(this HttpRequest httpRequest)
        {
            if (httpRequest.Form == null || httpRequest.Form.Files.Count == 0)
                return null;

            return httpRequest.Form.Files[0];
        }

        internal static string GetMimeType(this string documentName)
        {
            if (!string.IsNullOrWhiteSpace(documentName) && documentName.Contains('.'))
            {
                //  Something.docx
                var parts = documentName.Split('.');
                //  Something docx
                string ext = parts[parts.Length - 1];
                //  docx
                return MimeTypeMap.GetMimeType(ext);
                //  application/docx
            }

            return null;
        }
    }
}

