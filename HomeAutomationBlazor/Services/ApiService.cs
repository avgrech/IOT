using System.Net.Http.Headers;
using System.Net.Http.Json;
using HomeAutomationBlazor.Models;

namespace HomeAutomationBlazor.Services;

public class ApiService
{
    private readonly HttpClient _http;
    public string? Token { get; private set; }
    public bool IsAuthenticated => Token != null;
    public event Action? AuthStateChanged;

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> Login(string username, string password)
    {
        var response = await _http.PostAsJsonAsync("api/users/login", new LoginDto(username, password));
        if (!response.IsSuccessStatusCode) return false;
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
        if (result?.token == null) return false;
        Token = result.token;
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        AuthStateChanged?.Invoke();
        return true;
    }

    public async Task<List<Organisation>?> GetOrganisations() =>
        await _http.GetFromJsonAsync<List<Organisation>>("api/organisations");

    public async Task<Organisation?> CreateOrganisation(Organisation org)
    {
        var res = await _http.PostAsJsonAsync("api/organisations", org);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<Organisation>();
    }

    public async Task DeleteOrganisation(int id) =>
        await _http.DeleteAsync($"api/organisations/{id}");

    public async Task<List<Property>?> GetProperties() =>
        await _http.GetFromJsonAsync<List<Property>>("api/properties");

    public async Task<Property?> CreateProperty(Property prop)
    {
        var res = await _http.PostAsJsonAsync("api/properties", prop);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<Property>();
    }

    public async Task DeleteProperty(int id) =>
        await _http.DeleteAsync($"api/properties/{id}");

    private record TokenResponse(string token);

    public void Logout()
    {
        Token = null;
        _http.DefaultRequestHeaders.Authorization = null;
        AuthStateChanged?.Invoke();
    }
}
