namespace Kymeta.Cloud.Services.EnterpriseBroker.HttpClients
{
    public interface IFileStorageClient
    {
        Task<bool> UploadBlobFile(Stream fileContent, string fileName, string storageAccount, string container, string path);
    }
    public class FileStorageClient : IFileStorageClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private readonly ILogger<FileStorageClient> _logger;

        public FileStorageClient(HttpClient client, IConfiguration config, ILogger<FileStorageClient> logger)
        {
            client.BaseAddress = new Uri(config["Api:FileStorage"]);
            client.DefaultRequestHeaders.Add("sharedKey", config["SharedKey"]);

            _client = client;
            _config = config;
            _logger = logger;
        }

        public async Task<bool> UploadBlobFile(Stream fileContent, string fileName, string storageAccount, string container, string path)
        {
            // convert stream to byte array
            using var memoryStream = new MemoryStream();
            fileContent.CopyTo(memoryStream);
            var fileContentBytes = memoryStream.ToArray();

            // init the form data post body
            MultipartFormDataContent form = new()
            {
                { new StringContent(storageAccount), "storageaccount" },
                { new StringContent(container), "containername" },
                { new StringContent(path), "path" },
                { new ByteArrayContent(fileContentBytes, 0, fileContentBytes.Length), "files",  fileName }
            };

            // send the file to FileStorage service to upload to blob storage (CDN)
            var response = await _client.PostAsync("/v1/blobs", form);

            // return upload result
            return response.IsSuccessStatusCode;
        }
    }
}
