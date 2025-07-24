using System.Net;
using System.Net.Http.Json;
using HomeAuthomationAPI.Models;

public class UsersControllerTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public UsersControllerTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsAdminUser()
    {
        var users = await _client.GetFromJsonAsync<User[]>("/api/users");
        Assert.Contains(users, u => u.Username == "admin");
    }

    [Fact]
    public async Task Get_NonExistingUser_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/users/9999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
