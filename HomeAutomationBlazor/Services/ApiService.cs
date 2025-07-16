using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using HomeAutomationBlazor.Models;

namespace HomeAutomationBlazor.Services;

public class ApiService
{
    private readonly HttpClient _http;
    public string? LastError { get; private set; }
    public string? Token { get; private set; }
    public bool IsAuthenticated => Token != null;
    public bool IsGlobalAdmin { get; private set; }
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
            ParseToken();
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

    public async Task<List<Organisation>?> GetOrganisations()
    {
        try
        {
            var response = await _http.GetAsync("api/organisations");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Organisation>>();
            }

            var content = await response.Content.ReadAsStringAsync();
            Console.Error.WriteLine($"Error fetching organisations: {response.StatusCode} {content}");
            return null;
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine($"Error fetching organisations: {ex.Message}");
            return null;
        }
    }

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

    public async Task TriggerRouterSync(string routerUniqueId)
    {
        var payload = new
        {
            routerDeviceId = routerUniqueId,
            type = "sync"
        };

        await _http.PostAsJsonAsync("api/directmessages", payload);
    }

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

    public async Task<DeviceStatus?> GetLatestDeviceStatus(string routerDeviceId) =>
        await _http.GetFromJsonAsync<DeviceStatus>($"api/devicestatuses/latest/{routerDeviceId}");

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

    public async Task<List<User>?> GetUsers() =>
        await _http.GetFromJsonAsync<List<User>>("api/users");

    public async Task<User?> CreateUser(User user)
    {
        var res = await _http.PostAsJsonAsync("api/users", user);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<User>();
    }

    public async Task UpdateUser(User user)
    {
        await _http.PutAsJsonAsync($"api/users/{user.Id}", user);
    }

    public async Task DeleteUser(int id) =>
        await _http.DeleteAsync($"api/users/{id}");

    private record TokenResponse(string token);

    private void ParseToken()
    {
        IsGlobalAdmin = false;
        if (Token == null)
            return;
        try
        {
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(Token);
            if (jwt.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value == "GlobalAdmin")
            {
                IsGlobalAdmin = true;
            }
        }
        catch
        {
            // ignore parse errors
        }
    }

    public void Logout()
    {
        Token = null;
        IsGlobalAdmin = false;
        _http.DefaultRequestHeaders.Authorization = null;
        AuthStateChanged?.Invoke();
    }
}
