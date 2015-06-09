using Windows.Devices.Geolocation;

namespace London_Universal.ViewModels
{
    public class MapItemModel : SelectableViewModel 
    {
        private Geopoint Location { get; set; }
        private string Name { get; set; }

        public MapItemModel(Geopoint location, string name)
        {
            Location = location;
            Name = name;
        }
    }
}
