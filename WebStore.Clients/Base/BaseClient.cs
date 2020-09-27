using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected string ServiceAddress;
        protected HttpClient Client;

        protected BaseClient(IConfiguration configuration, string serviceAddress)
        {
            this.ServiceAddress = serviceAddress;

            Client = new HttpClient
            {
                BaseAddress = new Uri(configuration["WebApiURL"]),
                DefaultRequestHeaders =
                {
                    Accept = {new MediaTypeWithQualityHeaderValue("application/json")}
                }
            };
        }

        protected T Get<T>(string url) => GetAsync<T>(url).Result;

        protected async Task<T> GetAsync<T>(string url, CancellationToken token = default)
        {
            var response = await Client.GetAsync(url, token);
            return await response
                .EnsureSuccessStatusCode() // Убеждаемся, что в ответ получен 200-ый статусный код.
                .Content // В ответе есть содержимое с которым можно работать
                .ReadAsAsync<T>(token); // Десериализуем данные в нужный тип объекта
        }

        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync<T>(url, item).Result;

        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken token = default)
        {
            var response = await Client.PostAsJsonAsync(url, item, token);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync<T>(url, item).Result;

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken token = default)
        {
            var response = await Client.PutAsJsonAsync(url, item, token);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;

        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken token = default)
        {
            var response = await Client.DeleteAsync(url, token);
            return response.EnsureSuccessStatusCode();
        }

        #region IDisposable

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            // Здесь можно выполнить освобождение неуправляемых ресурсов
            if (disposing)
            {
                // Здесь можно выполнить освобождение управляемых ресурсов
                Client.Dispose();
            }
        }

        #endregion
    }
}