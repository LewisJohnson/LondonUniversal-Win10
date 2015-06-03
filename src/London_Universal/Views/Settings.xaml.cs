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
                    MainPage.LandMarks = item.IsChecked.Value;
                    break;

                case "Pede":
                    MainPage.PedeFeat = item.IsChecked.Value;
                    break;

                case "Traffic":
                    MainPage.Traffic = item.IsChecked.Value;
                    break;

                case "ForceControls":
                    MainPage.ForceControls = item.IsChecked.Value;
                    break;

                case "Businesses":
                    MainPage.BusinessFeat = item.IsChecked.Value;
                    break;
            }
        }

        private void StyleCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sel = sender as ComboBox;

            switch (sel.SelectedIndex)
            {
                case 0:
                    MainPage.MapStyle = MapStyle.Road;
                    break;

                case 1:
                    MainPage.MapStyle = MapStyle.Aerial;
                    break;

                case 2:
                    MainPage.MapStyle = MapStyle.AerialWithRoads;
                    break;
            }
        }

        private void Colour_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sel = sender as ComboBox;

            switch (sel.SelectedIndex)
            {
                case 0:
                    MainPage.MapColorScheme = MapColorScheme.Light;
                    break;

                case 1:
                    MainPage.MapColorScheme = MapColorScheme.Dark;
                    break;
            }
        }

        private void Settings_OnLoading(FrameworkElement sender, object args)
        {
            if (!MainPage.Is3DEnabled)
                LandMarksCheckBox.IsEnabled = false;

            LandMarksCheckBox.IsChecked = MainPage.LandMarks;
            PedeCheckBox.IsChecked = MainPage.PedeFeat;
            TrafficCheckBox.IsChecked = MainPage.Traffic;
            ControlsCheckBox.IsChecked = MainPage.ForceControls;
            BusinessCheckBox.IsChecked = MainPage.BusinessFeat;

            switch (MainPage.MapStyle)
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

            switch (MainPage.MapColorScheme)
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
