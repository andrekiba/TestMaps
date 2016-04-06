using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using Android.OS;

namespace TestMaps
{
	[Activity (Label = "TestMaps", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity, IOnMapReadyCallback
	{
	    private GoogleMap map;
        private MapFragment mapFragment;

	    private static readonly LatLng Wharariki = new LatLng(-40.50, 172.67);
	    private static readonly LatLng Hobbiton = new LatLng(-37.85, 175.67);
	    private static readonly LatLng MilfordSound = new LatLng(-44.66, 167.92);
	    private static readonly LatLng Home = new LatLng(44.91, 9.78);

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Find the map fragment
            mapFragment = FragmentManager.FindFragmentById(Resource.Id.map) as MapFragment;

            mapFragment?.GetMapAsync(this);
        }

        public void OnMapReady(GoogleMap googleMap)
	    {
            map = googleMap;

			map.MapType = GoogleMap.MapTypeSatellite;
            map.MyLocationEnabled = true;
            map.UiSettings.CompassEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true;
            map.UiSettings.ZoomControlsEnabled = true;

            var homeMarker = map.AddMarker(new MarkerOptions()
                .SetPosition(Home));

            var whararikiMarker =map.AddMarker(new MarkerOptions()
                .SetPosition(Wharariki)
                .SetTitle("Wharariki")
                .SetSnippet("I left my heart here!")
                .SetIcon(BitmapDescriptorFactory.FromResource(Resource.Mipmap.Icon)));

            var milfordMarker = map.AddMarker(new MarkerOptions()
                .SetPosition(MilfordSound)
                .SetTitle("Milford Sound")
                .Draggable(true)
                .SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueYellow)));

            var hobbitonMarker = map.AddMarker(new MarkerOptions()
                .SetPosition(Hobbiton)
                .SetTitle("Hobbiton")
                .InfoWindowAnchor(1, 0)
                .SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueBlue)));

            map.MarkerClick += delegate (object sender, GoogleMap.MarkerClickEventArgs e) {
                if (e.Marker.Equals(milfordMarker))
                {
                    milfordMarker.Flat = !milfordMarker.Flat;
                    milfordMarker.ShowInfoWindow();
                }
                else {
                    // Execute default behavior for other markers.
                    e.Handled = false;
                }
            };

            map.InfoWindowClick += (sender, e) =>
            {
                if (e.Marker.Id == hobbitonMarker.Id)
                {
                    e.Marker.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueRose));
                }
            };

            map.MapClick += (sender, e) =>
            {
                if (!hobbitonMarker.IsInfoWindowShown)
                {
                    hobbitonMarker.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueYellow));
                }
            };

            map.MapLongClick += (sender, e) =>
                map.AnimateCamera(CameraUpdateFactory.ZoomOut(), 1000, null);

            // Center on Wharariki
            var update = CameraUpdateFactory.NewLatLngZoom(Wharariki, map.MaxZoomLevel);
            map.MoveCamera(update);
        }
	}
}
