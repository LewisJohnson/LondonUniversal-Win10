using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable PossibleInvalidOperationException

namespace London_Universal.Views
{
    public sealed partial class Settings
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Features_OnClick(object sender, RoutedEventArgs e)
        {
            var item = sender as CheckBox;

            switch (item?.Tag.ToString())
            {
                case "LandMarks":
                    FullMap.LandMarks = item.IsChecked.Value;
                    break;

                case "Pede":
                    FullMap.PedeFeat = item.IsChecked.Value;
                    break;

                case "Traffic":
                    FullMap.Traffic = item.IsChecked.Value;
                    break;

                case "ForceControls":
                    FullMap.ForceControls = item.IsChecked.Value;
                    break;

                case "Businesses":
                    FullMap.BusinessFeat = item.IsChecked.Value;
                    break;
            }
        }

        private void StyleCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sel = sender as ComboBox;

            switch (sel.SelectedIndex)
            {
                case 0:
                    FullMap.MapStyle = MapStyle.Road;
                    break;

                case 1:
                    FullMap.MapStyle = MapStyle.Aerial;
                    break;

                case 2:
                    FullMap.MapStyle = MapStyle.AerialWithRoads;
                    break;
            }
        }

        private void Colour_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sel = sender as ComboBox;

            switch (sel.SelectedIndex)
            {
                case 0:
                    FullMap.MapColorScheme = MapColorScheme.Light;
                    break;

                case 1:
                    FullMap.MapColorScheme = MapColorScheme.Dark;
                    break;
            }
        }

        private void Settings_OnLoading(FrameworkElement sender, object args)
        {
            if (!FullMap.Is3DEnabled)
                LandMarksCheckBox.IsEnabled = false;

            LandMarksCheckBox.IsChecked = FullMap.LandMarks;
            PedeCheckBox.IsChecked = FullMap.PedeFeat;
            TrafficCheckBox.IsChecked = FullMap.Traffic;
            ControlsCheckBox.IsChecked = FullMap.ForceControls;
            BusinessCheckBox.IsChecked = FullMap.BusinessFeat;

            switch (FullMap.MapStyle)
            {
                case MapStyle.Road:
                    StyleCombo.SelectedIndex = 0;
                    break;

                case MapStyle.Aerial:
                    StyleCombo.SelectedIndex = 1;
                    break;

                case MapStyle.AerialWithRoads:
                    StyleCombo.SelectedIndex = 2;
                    break;

            }

            switch (FullMap.MapColorScheme)
            {
                case MapColorScheme.Light:
                    ColourCombo.SelectedIndex = 0;
                    break;
                case MapColorScheme.Dark:
                    ColourCombo.SelectedIndex = 1;
                    break;
            }

        }
    }
}
