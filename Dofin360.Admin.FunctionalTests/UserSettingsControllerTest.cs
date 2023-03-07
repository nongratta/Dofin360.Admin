using Dofin360.Admin.ApiClient;
using Dofin360.Admin.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Dofin360.Admin.FunctionalTests;

/// <summary>
/// Тестирование
/// </summary>
public class UserSettingsControllerTest
{
    private readonly Dofin360AdminApiClient _client;
    private readonly ITestOutputHelper _output;

    public static IConfiguration InitConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
        return config;
    }

    public UserSettingsControllerTest(ITestOutputHelper output)
    {
        _output = output;
        
        var configuration = InitConfiguration();

        var apiUrl = configuration["ApiUrl"];
        var httpClient = new HttpClient { BaseAddress = new Uri(apiUrl) };
        _client = new Dofin360AdminApiClient(httpClient);       
    }

    [Fact]
    public async Task CreateUserSettingsTest()
    {
        /*var id = */await _client.Post(new D360UserSettingsSetViewModel { FirstName = "Виктор", SecondName = "Виноградов" }); 
        Assert.True(true);
    }    
}
