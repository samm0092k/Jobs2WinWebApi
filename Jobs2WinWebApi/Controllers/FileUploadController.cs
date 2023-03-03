using Jobs2WinWebApi.Models;
using Jobs2WinWebApi.Proxy;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Jobs2WinWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileUploadController : Controller
    {
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;
        private const string EndPoint = "ImportJobs";

        public FileUploadController(IRestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
            _configuration = configuration;
        }
        [HttpPost]
        public FileDetails Post(IFormFile formFile)
        {
            var client = new HttpClient
            {
                BaseAddress = new("https://localhost:7151/")
            };

            //        using var stream = System.IO.File.OpenRead("./Test.txt");
            //        using var request = new HttpRequestMessage(HttpMethod.Post, "file");
            //        using var content = new MultipartFormDataContent
            //{
            //    { new StreamContent(stream), "file", "Test.txt" }
            //};

            //        request.Content = content;

            //        await client.SendAsync(request);
            
            var request = new HttpRequestMessage(HttpMethod.Post, "file");
            request.Content=new StreamContent(stre)
            using (var stream = new MemoryStream())
            {
                formFile.CopyTo(stream);
                using var content = new MultipartFormDataContent
    {
        { new StreamContent(stream), "file", "Test.xlsx" }
    };
                //client.SendAsync()
                var result1 = _restClient.PostAsync(Common.ActionUrls.FileUploaded, EndPoint, new Dictionary<string, string>(), content);
            }

            var result = _restClient.PostAsync(Common.ActionUrls.FileUploaded, EndPoint, new Dictionary<string, string>(), formFile);
            return JsonConvert.DeserializeObject<FileDetails>(result.Content.ToString());
        }
    }
}
