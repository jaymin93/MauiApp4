using Microsoft.Maui.Controls;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Graphics;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Maps;

namespace MauiApp4
{
    public partial class MainPage : ContentPage
    {
        private Pin carPin; // Store the car pin
        private bool isVehicleMoving = true; // Simulate vehicle movement for demo purposes
        private Polyline vehiclePath; // To store the polyline representing the vehicle's path
        private Location previousLocation; // To store the previous location for drawing the line

        public MainPage()
        {
            InitializeComponent();
            carPin = new Pin();
            vehiclePath = new Polyline(); // Initialize the polyline
            ShowVehicleOnMap(28.7041, 77.1025, "ABC123");
            StartVehicleMovementSimulation();
            Task task = GetLocationAsync();
        }

        private void ShowVehicleOnMap(double latitude, double longitude, string vehiclePlate)
        {
            // Center the map on the specified location
            var position = new Location(latitude, longitude);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));

            // Create a custom pin with car icon
            carPin.Label = "Vehicle : " + vehiclePlate + "";
            carPin.Location = position;
            carPin.Type = PinType.Place;

            // Optionally, add a custom icon for the pin
            // carPin.MarkerIcon = new PinIcon { ResourceId = "car_icon.png" }; // Ensure "car_icon.png" is in Resources/Images

            // Add the pin to the map
            map.Pins.Add(carPin);

            // Initialize the polyline with the initial location
            previousLocation = position; // Set the initial position
            vehiclePath.StrokeColor = Colors.Red;
            vehiclePath.StrokeWidth = 1;
            vehiclePath.Geopath.Add(position);
            map.MapElements.Add(vehiclePath);
        }

        // Method to simulate vehicle movement and update pin location
        private async void StartVehicleMovementSimulation()
        {
            try
            {
                // Simulate vehicle movement every 2 seconds
                while (isVehicleMoving)
                {
                    // Simulate a new random location for the vehicle (e.g., nearby coordinates)
                    double newLatitude = 28.7041 + (new Random().NextDouble() - 0.5) * 0.01; // Random offset
                    double newLongitude = 77.1025 + (new Random().NextDouble() - 0.5) * 0.01; // Random offset

                    // Update the pin's location
                    UpdateVehicleLocation(newLatitude, newLongitude);

                    //var location = await GetLocationAsync().ConfigureAwait(false);
                    //UpdateVehicleLocation(location.latitude, location.longitude);
                    // Wait for 2 seconds before moving the vehicle again
                    await Task.Delay(2000); // Update every 2 seconds
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
        }

        // Method to update the vehicle location on the map
        private void UpdateVehicleLocation(double latitude, double longitude)
        {
            // Update the pin's location
            var newLocation = new Location(latitude, longitude);
            carPin.Location = newLocation;

            // Add the new location to the polyline to draw the path
            vehiclePath.Geopath.Add(newLocation);

            // Optionally, move the map to the new location
            map.MoveToRegion(MapSpan.FromCenterAndRadius(newLocation, Distance.FromKilometers(1)));

            // Save the current location as previous for next update
            previousLocation = newLocation;
        }

        private async Task<(double latitude, double longitude)> GetLocationAsync()
        {
            try
            {
                // Request current location
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    // If no cached location, request a fresh one
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                if (location != null)
                {
                    // Access latitude and longitude
                    double latitude = location.Latitude;
                    double longitude = location.Longitude;

                    return (latitude, longitude);
                }
                else
                {
                    await DisplayAlert("Error", "Unable to retrieve location", "OK");
                    return default;
                }
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Error", "GPS is not supported on this device", "OK");
                return default;
            }
            catch (PermissionException)
            {
                await DisplayAlert("Error", "Location permission not granted", "OK");
                return default;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                return default;
            }
        }

        // Method to stop the vehicle movement simulation
        private void StopVehicleMovement()
        {
            isVehicleMoving = false;
        }

        // Button to stop vehicle movement simulation (optional)
        private void CounterBtn_Clicked(object sender, EventArgs e)
        {
            StopVehicleMovement();
        }
    }
}
