
using Xamarin.Forms;

namespace CoordinateSender
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();

            var htmlSource = new HtmlWebViewSource();
            htmlSource.Html = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <link rel='stylesheet' href='https://unpkg.com/leaflet@1.7.1/dist/leaflet.css' />
                    <script src='https://unpkg.com/leaflet@1.7.1/dist/leaflet.js'></script>
                </head>
                <body>
                    <div id='map' style='width: 100%; height: 100vh;'></div>
                    <script>
                        var map = L.map('map').setView([55.751244, 37.618423], 13); // Москва
                        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                            attribution: 'Map data &copy; <a href=""https://www.openstreetmap.org/copyright"">OpenStreetMap</a> contributors'
                        }).addTo(map);
                    </script>
                </body>
                </html>";

            MapView.Source = htmlSource;
        }
    }
}
