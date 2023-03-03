using Jobs2WinWebApi.Models;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using static Jobs2WinWebApi.Common;

namespace Jobs2WinWebApi.Proxy
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        Dictionary<ActionUrls, string> BaseUrlCollections;

        public RestClient(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
            CustomUrls customUrls = new();
            _configuration.GetSection("CustomUrls").Bind(customUrls);
            BaseUrlCollections = new()
            {
                { Common.ActionUrls.FileUploaded, customUrls.FileUploaderUrl }
            };
        }
        public HttpResponseMessage PostAsync(ActionUrls action, string url, Dictionary<string, string> headers, object content)
        {
            _httpClient.BaseAddress = new Uri(BaseUrlCollections[action]);
            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(url, UriKind.Relative),
                Method = HttpMethod.Post,
            };

            if (headers != null)
            {
                foreach (var requestHeader in headers)
                    requestMessage.Headers.Add(requestHeader.Key, requestHeader.Value);
            }

            if (content != null)
                requestMessage.Content = CreateHttpContentAsJson(content);
            return _httpClient.SendAsync(requestMessage).Result;
        }
        public HttpResponseMessage PostAsync(ActionUrls action, string url, Dictionary<string, string> headers, MultipartFormDataContent content)
        {
            _httpClient.BaseAddress = new Uri(BaseUrlCollections[action]);
            var requestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(url, UriKind.Relative),
                Method = HttpMethod.Post,
            };

            if (headers != null)
            {
                foreach (var requestHeader in headers)
                    requestMessage.Headers.Add(requestHeader.Key, requestHeader.Value);
            }

            if (content != null)
                requestMessage.Content = content;
            return _httpClient.SendAsync(requestMessage).Result;
        }
        private HttpContent CreateHttpContentAsJson(object content)
        {
            // This code is necessary to handle the serialization of abstract classes in the CSG models.
            // Setting this to Auto tells the serializer to check the objects and create the $type key if the types do not match (abstract classes with multiple derivations). 
            // The default is None which does not read or write any type names.

            // In our case, this will force the serializer to set the $type key for our abstract classes like Communications etc.
            // By doing this, we are giving DPRestApi the information it needs to deserialize the CSG abstract classes. If we remove this code,
            // DPRestApi will not be able to deserialize the CSG models when using JSON.
            var formatterSetings = new JsonMediaTypeFormatter
            {
                SerializerSettings = { TypeNameHandling = TypeNameHandling.Auto }
            };

            MediaTypeFormatter jsonFormatter = formatterSetings;
            return new ObjectContent<object>(content, jsonFormatter);
        }

        public void Dispose() => _httpClient.Dispose();

    }
}
