using System.Net.Http.Json;
using MauiLab1.Entities;

namespace MauiLab1.Services;

public class RateService : IRateService
{
    private readonly HttpClient _httpClient;

    public RateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Rate>> GetRates(DateTime date)
    {
        var dateString = date.ToString("yyyy-MM-dd");
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}?ondate={dateString}&periodicity=0");

        if (!response.IsSuccessStatusCode) return new List<Rate>();
        
        var rates = await response.Content.ReadFromJsonAsync<List<Rate>>();
        return rates ?? new List<Rate>();
    }
}