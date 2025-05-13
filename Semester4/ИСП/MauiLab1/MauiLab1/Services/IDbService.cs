using MauiLab1.Entities;

namespace MauiLab1.Services;

public interface IDbService
{
    IEnumerable<TouristRoute> GetAllTouristRoutes();
    IEnumerable<Attraction> GetRouteAttractions(int routeId);
    void Init();
}