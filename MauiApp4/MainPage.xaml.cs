using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Graphics;

namespace MauiApp4
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            ShowVehicleOnMap(28.7041, 77.1025);

        }

        private void ShowVehicleOnMap(double latitude, double longitude)
        {
            // Center the map on the specified location
            var position = new Location(latitude, longitude);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));

            // Create a custom pin with car icon
            var carPin = new Pin
            {
                Label = "Vehicle Location",
                Location = position,
                Type = PinType.Place
            };

            // Add custom icon for the pin
            //carPin.MarkerIcon = new PinIcon { ResourceId = "car_icon.png" }; // Ensure "car_icon.png" is in Resources/Images

            //// Event handler to show latitude and longitude on pin click
            //carPin.Clicked += (s, e) =>
            //{
            //    DisplayAlert("Vehicle Location", $"Latitude: {latitude}\nLongitude: {longitude}", "OK");
            //};

            // Add the pin to the map
            map.Pins.Add(carPin);
        }

        private void CounterBtn_Clicked(object sender, EventArgs e)
        {
            //ShowVehicleOnMap("28.7041", "77.1025", "Vehicle Plate: ABC123");

        }
    }

}
