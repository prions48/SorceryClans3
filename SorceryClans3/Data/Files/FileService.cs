using SorceryClans3.Data.Users;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.JSInterop;
namespace SorceryClans3.Data.Files
{
    public class FileService
    {
        private readonly IFileSettings _settings;
        private readonly UserService _userService;
        private readonly BlobServiceClient _service;
        private BlobContainerClient _container;
        private readonly IJSRuntime _js;
        public FileService(IFileSettings settings, IJSRuntime js, UserService userService)
        {
            _userService = userService;
            _js = js;
            _settings = settings;
            _service = new BlobServiceClient(_settings.GetConnString());
        }
        /// <summary>
        /// Uploads file to app's default container.  Returns "Success" unless failure occurs
        /// </summary>
        /// <param name="filename">file name written in blob (recommend use a Guid instead of actual file name)</param>
        /// <param name="stream">The file memory to upload</param>
        /// <returns>the file record created, and "Success" or exception message</returns>
        public async Task<(Auth0UserFile?,string)> UploadFile(Guid userid, string filename, MemoryStream stream)
        {
            try 
            {
                _container = _service.GetBlobContainerClient(KeyChain.Container);
                stream.Position = 0;
                string blobname = Guid.NewGuid().ToString();
                await _container.UploadBlobAsync($"{userid}/{blobname}", stream);
                Auth0UserFile file = new Auth0UserFile() {
                    UserID = userid,
                    BlobName = blobname,
                    DisplayName = filename,
                    ImageFile = filename.EndsWith(".png")
                };
                _userService.CreateFileRecord(file);
                return (file,"Success");
            }
            catch (Exception ex)
            {
                return (null,ex.Message);
            }
        }
        public async Task<(MemoryStream?,string)> DownloadFile(string path, string filename)
        {
            try
            {
                _container = _service.GetBlobContainerClient(KeyChain.Container);
                BlockBlobClient block = _container.GetBlockBlobClient($"{path}/{filename}");
                MemoryStream stream = new();
                await block.DownloadToAsync(stream);
                stream.Position = 0;
                return (stream,"Success");
            }
            catch (Exception ex)
            {
                return (null,ex.Message);
            }
        }
        public async Task<string> DownloadFromBlob(Auth0UserFile file)
        {
            var results = await DownloadFile(file.UserID.ToString(),file.BlobName);
            if (results.Item2 == "Success")
            {
                await SaveFile(results.Item1!,file.DisplayName);
                return "Success";
            }
            else
                return results.Item2;
        }
        public async Task SaveFile(MemoryStream stream, string filename)
        {
            var streamRef = new DotNetStreamReference(stream);
            await _js.InvokeVoidAsync("saveAsFile",filename,streamRef);
        }
    }
}