using static Jobs2WinWebApi.Common;

namespace Jobs2WinWebApi.Proxy
{
    public interface IRestClient : IDisposable
    {
        public HttpResponseMessage PostAsync(ActionUrls action, string endPoint, Dictionary<string, string> headers, object content);
        public HttpResponseMessage PostAsync(ActionUrls action, string endPoint, Dictionary<string, string> headers, MultipartFormDataContent content);
    }
}
