using Microsoft.AspNetCore.StaticFiles;

namespace BlobStorage.API.StorageExtension
{
    public static class StorageHelpers
    {
        private static readonly FileExtensionContentTypeProvider Provider = new FileExtensionContentTypeProvider();

        public static string GetContentType(this string fileName)
        {
            if (!Provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }


        public static bool IsImage(this string fileName)
        {

            return Provider.TryGetContentType(fileName, out var contentType)
                && contentType.StartsWith("image/");
        }
    }
}
