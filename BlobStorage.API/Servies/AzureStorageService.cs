using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using BlobStorage.API.Models;
using BlobStorage.API.StorageExtension;

namespace BlobStorage.API.Servies
{
    public class AzureStorageService : IAzureStorageService
    {

        private BlobServiceClient _blobServiceClient;
        public AzureStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<byte[]> DownloadAsync(string fileName, string container)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(container);

            var blobClient = containerClient.GetBlobClient(fileName);

            var downloadContent = await blobClient.DownloadAsync();

            using (var ms = new MemoryStream())
            {
                await downloadContent.Value.Content.CopyToAsync(ms);

                return ms.ToArray();
            }

        }

        public async Task<AzureUploadResponseModel> UploadAsync(string filePath, string container)
        {
            if (!File.Exists(filePath)) return new AzureUploadResponseModel();

            var containerClient = _blobServiceClient.GetBlobContainerClient(container);

            await containerClient.CreateIfNotExistsAsync();

            var fileName = Path.GetFileName(filePath);

            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(filePath, new BlobHttpHeaders
            {
                ContentType = filePath.GetContentType()
            });

            return new AzureUploadResponseModel
            {
                fileExtention = Path.GetExtension(fileName),
                fileType = filePath.GetContentType(),
                fileUri = blobClient.Uri.AbsoluteUri
            };

        }

        public async Task<List<AzureUploadResponseModel>> UploadAsync(List<IFormFile> files, string container)
        {
            if (!files.Any()) return new List<AzureUploadResponseModel>();

            var containerClient = _blobServiceClient.GetBlobContainerClient(container);

            await containerClient.CreateIfNotExistsAsync();

            var responseList = new List<AzureUploadResponseModel>();

            foreach (var file in files)
            {
                var blobClient = containerClient.GetBlobClient(file.FileName);

                await blobClient.UploadAsync(file.OpenReadStream(), new BlobHttpHeaders
                {
                    ContentType = file.FileName.GetContentType()
                });

                responseList.Add(new AzureUploadResponseModel
                {
                    fileExtention = Path.GetExtension(file.FileName),
                    fileType = file.FileName.GetContentType(),
                    fileUri = blobClient.Uri.AbsoluteUri
                });
            }


            return responseList;
        }

        public async Task<AzureUploadResponseModel> UploadAsync(IFormFile file, string container)
        {
            if (file.Length == 0) return new AzureUploadResponseModel();

            var containerClient = _blobServiceClient.GetBlobContainerClient(container);

            await containerClient.CreateIfNotExistsAsync();

            var fileName = Path.GetFileName(file.FileName);

            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(file.OpenReadStream(), new BlobHttpHeaders
            {
                ContentType = file.FileName.GetContentType()
            });

            return new AzureUploadResponseModel
            {
                fileExtention = Path.GetExtension(file.FileName),
                fileType = file.FileName.GetContentType(),
                fileUri = blobClient.Uri.AbsoluteUri
            };
        }
    
    }
}
