using Dofin360.Admin.Model;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Dofin360.Admin.ApiClient;

public interface IDofin360AdminApiClient
{
    Task<object> Get(int skip = 0, int take = 20);
    Task<object> Get(Guid id);
    Task Post(D360UserSettingsSetViewModel model);
    Task Put(Guid id, D360UserSettingsSetViewModel model);
    Task Delete(Guid id);
}
/// <summary>
/// Клиент к ресурсу Dofin360.Admin 
/// </summary>
public class Dofin360AdminApiClient : IDofin360AdminApiClient
{
    private readonly HttpClient _httpClient;
    public delegate void ErrorMessage(string message);
    public ErrorMessage WriteLine { get; set; } = x => Console.WriteLine(x);

    public Dofin360AdminApiClient(HttpClient client) => _httpClient = client;

    public async Task<object> Get(int skip = 0, int take = 20) => await ExecuteGetAsync<IEnumerable<D360UserSettings>>($"usersettings?skip={skip}&take={take}");

    public async Task<object> Get(Guid id) => await ExecuteGetAsync<D360UserSettings>($"usersettings/{id}");

    public async Task Post(D360UserSettingsSetViewModel model) => await ExecutePostAsync<IEnumerable<D360UserSettings>>($"usersettings", model);

    public async Task Put(Guid id, D360UserSettingsSetViewModel model) => await ExecutePutAsync<IEnumerable<D360UserSettings>>($"usersettings/{id}", model);

    public async Task Delete(Guid id) => await ExecuteDeleteAsync<IEnumerable<D360UserSettings>>($"usersettings/{id}");

    private async Task<T> ExecuteGetAsync<T>(string url) where T : class
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        using HttpContent content = response.Content;
        var str = await content.ReadAsStringAsync();
        try
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
        catch (WebException ex)
        {
            await WebException(ex);
            throw;
        }
    }

    private async Task ExecuteDeleteAsync<T>(string url)
    {
        using HttpResponseMessage response = await _httpClient.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
        using HttpContent content = response.Content;
        try
        {
            var str = await content.ReadAsStringAsync();
            return;
        }
        catch (WebException ex)
        {
            await WebException(ex);
            throw;
        }
    }

    private async Task ExecutePostAsync<T>(string url, object json)
    {
        var attach = new StringContent(JsonConvert.SerializeObject(json));
        using HttpResponseMessage response = await _httpClient.PostAsync(url, attach);
        response.EnsureSuccessStatusCode();
        using HttpContent content = response.Content;
        try
        {
            await content.ReadAsStringAsync();            
        }
        catch (WebException ex)
        {
            await WebException(ex);
            throw;
        }
    }

    private async Task ExecutePutAsync<T>(string url, object json)
    {
        var attach = new StringContent(JsonConvert.SerializeObject(json));
        using HttpResponseMessage response = await _httpClient.PutAsync(url, attach);
        response.EnsureSuccessStatusCode();
        using HttpContent content = response.Content;
        try
        {
            await content.ReadAsStringAsync();
        }
        catch (WebException ex)
        {
            await WebException(ex);
            throw;
        }
    }

    private async Task WebException(WebException ex)
    {
        var text = string.Empty;

        if (ex.Response != null) using (var s = ex.Response?.GetResponseStream())
            {
                using var tw = new StreamReader(s);
                text = await tw.ReadToEndAsync();
            }

        WriteLine($"WebException: {ex.Message} | Status {ex.Status} | Response: {text}");
    }    
}
