using MauiLab1.Entities;

namespace MauiLab1.Services;

public interface IRateService
{
    Task<IEnumerable<Rate>> GetRates(DateTime date);
}