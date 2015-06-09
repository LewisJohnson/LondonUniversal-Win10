using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Devices.Geolocation;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using London_Universal.Annotations;

namespace London_Universal.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public RelayCommand PaneToggleCommand { get; private set; }

        public MainViewModel()
        {

            PaneToggleCommand = new RelayCommand(PaneToggle);
            MapItemSource = new ObservableCollection<MapItemModel>
            {
                new MapItemModel(new Geopoint(new BasicGeoposition
                {
                    Latitude = 51.502993,
                    Longitude = -0.118833
                }), "London Eye")
            };
        }

        private ObservableCollection<MapItemModel> _mapItemSource;
        public ObservableCollection<MapItemModel> MapItemSource
        {
            get { return _mapItemSource; }
            set
            {
                if (Equals(value, _mapItemSource)) return;
                _mapItemSource = value;
                OnPropertyChanged();
            }
        }


        private bool _isPaneOpen;


        public bool IsPaneOpen
        {
            get { return _isPaneOpen; }
            private set
            {
                if (_isPaneOpen == value)
                {
                    return;
                }
                _isPaneOpen = value;
                OnPropertyChanged();
            }
        }

        private void PaneToggle()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

