using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiLab1.Entities;
using MauiLab1.Services;

namespace MauiLab1.Pages.TouristRoutes;

public partial class TouristRoutesPage : ContentPage
{
    private readonly IDbService _dbService;
    private List<TouristRoute> _touristRoutes;
    private int _selectedRouteId;
    
    public TouristRoutesPage(IDbService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
    }


    private void OnPageLoaded(object? sender, EventArgs e)
    {
        try
        {
            _dbService.Init();

            _touristRoutes = _dbService.GetAllTouristRoutes().ToList();
        
            RoutePicker.Items.Clear();
            foreach (var route in _touristRoutes)
            {
                RoutePicker.Items.Add(route.Name);
            }
        }
        catch (Exception exception)
        {
            DisplayAlert("Ошибка", $"Не удалось загрузить маршруты: {exception.Message}", "OK");
        }
    }


    private void OnRouteSelected(object? sender, EventArgs e)
    {
        if (RoutePicker.SelectedIndex == -1)
        {
            SelectedRouteInfo.Text = string.Empty;
            AttractionsCollection.IsVisible = false;
            AttractionsLabel.Text = string.Empty;
            return;
        }
        
        var selectedRoute = _touristRoutes[RoutePicker.SelectedIndex];
        _selectedRouteId = selectedRoute.Id;
        
        SelectedRouteInfo.Text = $"Выбранный маршрут: {selectedRoute.Name}\n" +
                                 $"Регион: {selectedRoute.Region}\n" +
                                 $"Сложность: {selectedRoute.Difficulty}\n" +
                                 $"Расстояние: {selectedRoute.Distance} км\n" +
                                 $"Время прохождения: {selectedRoute.DurationMinutes / 60} ч {selectedRoute.DurationMinutes % 60} мин";
        
        LoadAttractions(_selectedRouteId);
    }

    private void LoadAttractions(int routeId)
    {
        try
        {
            var attractions = _dbService.GetRouteAttractions(routeId).ToList();

            if (attractions.Count == 0)
            {
                AttractionsLabel.Text = "Для этого маршрута нет достопримечательностей";
                AttractionsCollection.IsVisible = false;
                return;
            }
        
            AttractionsCollection.ItemsSource = attractions;
            AttractionsCollection.IsVisible = true;
            AttractionsLabel.Text = $"Достопримечательности ({attractions.Count}):";
        }
        catch (Exception e)
        {
            DisplayAlert("Ошибка", $"Не удалось загрузить достопримечательности: {e.Message}", "OK");
        }
    }
}