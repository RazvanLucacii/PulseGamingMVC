using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MvcCoreAzureStorage.Services
{
    public class ServiceStorageBlobs
    {
        private BlobServiceClient client;

        public ServiceStorageBlobs(BlobServiceClient client)
        {
            this.client = client;
        }

        public async Task<List<string>> GetContainersAsync()
        {
            List<string> containers = new List<string>();

            await foreach (BlobContainerItem item in this.client.GetBlobContainersAsync())
            {
                containers.Add(item.Name);
            }
            return containers;
        }

        public async Task CreateContainerAsync(string containerNombre)
        {
            await this.client.CreateBlobContainerAsync(containerNombre, PublicAccessType.Blob);
        }

        public async Task DeleteContainerAsync(string containerNombre)
        {
            await this.client.DeleteBlobContainerAsync(containerNombre);
        }

        public async Task<string> GetBlobUrlAsync(string containerNombre, string blobName)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerNombre);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            string urlBlob = blobClient.Uri.OriginalString;
            return urlBlob;
        }

        public async Task DeleteBlobAsync(string containerNombre, string blobNombre)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerNombre);
            await containerClient.DeleteBlobAsync(blobNombre);
        }

        public async Task UploadBlobAsync(string containerNombre, string blobNombre, Stream stream)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerNombre);
            await containerClient.UploadBlobAsync(blobNombre, stream);
        }
    }
}
