using System.Net.Http.Headers;
using System.Net.Http.Json;
using HomeAutomationBlazor.Models;

namespace HomeAutomationBlazor.Services;

public class ApiService
{
    private readonly HttpClient _http;
    public string? LastError { get; private set; }
    public string? Token { get; private set; }
    public bool IsAuthenticated => Token != null;
    public event Action? AuthStateChanged;

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> Login(string username, string password)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/users/login", new LoginDto(username, password));
            if (!response.IsSuccessStatusCode)
            {
                LastError = "Invalid username or password";
                return false;
            }
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            if (result?.token == null)
            {
                LastError = "Invalid server response";
                return false;
            }
            Token = result.token;
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            LastError = null;
            AuthStateChanged?.Invoke();
            return true;
        }
        catch (Exception ex)
        {
            LastError = ex.Message;
            return false;
        }
    }

    public async Task<List<Organisation>?> GetOrganisations() =>
        await _http.GetFromJsonAsync<List<Organisation>>("api/organisations");

    public async Task<Organisation?> CreateOrganisation(Organisation org)
    {
        var res = await _http.PostAsJsonAsync("api/organisations", org);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<Organisation>();
    }

    public async Task UpdateOrganisation(Organisation org)
    {
        await _http.PutAsJsonAsync($"api/organisations/{org.Id}", org);
    }

    public async Task DeleteOrganisation(int id) =>
        await _http.DeleteAsync($"api/organisations/{id}");

    public async Task<List<Property>?> GetProperties() =>
        await _http.GetFromJsonAsync<List<Property>>("api/properties");

    public async Task<Property?> GetProperty(int id) =>
        await _http.GetFromJsonAsync<Property>($"api/properties/{id}");

    public async Task<Property?> CreateProperty(Property prop)
    {
        var res = await _http.PostAsJsonAsync("api/properties", prop);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<Property>();
    }

    public async Task UpdateProperty(Property prop)
    {
        await _http.PutAsJsonAsync($"api/properties/{prop.Id}", prop);
    }

    public async Task DeleteProperty(int id) =>
        await _http.DeleteAsync($"api/properties/{id}");

    public async Task<List<RouterDevice>?> GetRouterDevices() =>
        await _http.GetFromJsonAsync<List<RouterDevice>>("api/routerdevices");

    public async Task<RouterDevice?> CreateRouterDevice(RouterDevice router)
    {
        var res = await _http.PostAsJsonAsync("api/routerdevices", router);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<RouterDevice>();
    }

    public async Task UpdateRouterDevice(RouterDevice router)
    {
        await _http.PutAsJsonAsync($"api/routerdevices/{router.Id}", router);
    }

    public async Task DeleteRouterDevice(int id) =>
        await _http.DeleteAsync($"api/routerdevices/{id}");

    public async Task<List<Device>?> GetDevices() =>
        await _http.GetFromJsonAsync<List<Device>>("api/devices");

    public async Task<Device?> CreateDevice(Device dev)
    {
        var res = await _http.PostAsJsonAsync("api/devices", dev);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<Device>();
    }

    public async Task UpdateDevice(Device dev)
    {
        await _http.PutAsJsonAsync($"api/devices/{dev.Id}", dev);
    }

    public async Task DeleteDevice(int id) =>
        await _http.DeleteAsync($"api/devices/{id}");

    public async Task<List<Configuration>?> GetConfigurations() =>
        await _http.GetFromJsonAsync<List<Configuration>>("api/configurations");

    public async Task<Configuration?> CreateConfiguration(Configuration cfg)
    {
        var res = await _http.PostAsJsonAsync("api/configurations", cfg);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<Configuration>();
    }

    public async Task DeleteConfiguration(int id) =>
        await _http.DeleteAsync($"api/configurations/{id}");

    private record TokenResponse(string token);

    public void Logout()
    {
        Token = null;
        _http.DefaultRequestHeaders.Authorization = null;
        AuthStateChanged?.Invoke();
    }
}
