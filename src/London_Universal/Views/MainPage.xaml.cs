using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Devices.Geolocation;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using London_Universal.DataModels;

namespace London_Universal.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Properties

        public static bool ShowBusPins;
        public static bool ShowBikePins;
        public static bool ShowOysterPins;
        public static bool ShowTubePins;
        public static bool ShowSuperHighways;
        public static bool ShowCabWise;

        public static ObservableCollection<BikePointRootObject> BikePointCollection
        { get; private set; }
        = new ObservableCollection<BikePointRootObject>();

        public static ObservableCollection<SuperCycleRootObject> SuperCycleCollection
        { get; private set; }
        = new ObservableCollection<SuperCycleRootObject>();

        public static CabWiseRootObject CabWiseCollection
        { get; private set; }
        = new CabWiseRootObject();

        private readonly List<Scenario> _scenarios = new List<Scenario>
        {
            new Scenario
            {
                Title = "Map",
                ClassType = typeof (FullMap)
            },
            new Scenario
            {
                Title = "Settings",
                ClassType = typeof (Settings)
            },
            new Scenario
            {
                Title = "About",
                ClassType = typeof (About)
            }
        };

        #endregion

        public MainPage()
        {
            InitializeComponent();

        }

        #region Click Events

        private void Hamburger_Click(object sender, RoutedEventArgs e) => Splitter.IsPaneOpen = (Splitter.IsPaneOpen != true);
        private async void Footer_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }

        private void ScenarioControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var scenarioListBox = sender as ListBox;
            var s = scenarioListBox?.SelectedItem as Scenario;
            if (s != null)
            {
                HolderFrame.Navigate(s.ClassType);
                Splitter.IsPaneOpen = false;
            }
        }

        #endregion

        private async void MainPage_OnLoading(FrameworkElement sender, object args)
        {

            ScenarioControl.ItemsSource = _scenarios;

            BikePointCollection = await DataFetch.BikePointsTask();
            SuperCycleCollection = await DataFetch.SuperHighwaysTask();
            CabWiseCollection = await DataFetch.CabWiseTask();

            var internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

            if (internetConnectionProfile == null)
            {
                var nointernetDialog = new MessageDialog(
                    "It seems that you don't have an internet connection. Please try again.",
                    "No Internet connection :(")
                {
                    Options = MessageDialogOptions.AcceptUserInputAfterDelay
                };
                await nointernetDialog.ShowAsync();
            }

            var access = await Geolocator.RequestAccessAsync();

            if (access == GeolocationAccessStatus.Denied)
            {
                var nolocationDialog = new MessageDialog(
                    "Sorry. This app relies on your location to function. Please allow you to access it via your settings",
                    "We don't have access to your location :(");
                nolocationDialog.Commands.Add(new UICommand("Okay", command => Application.Current.Exit()));
                nolocationDialog.Options = MessageDialogOptions.AcceptUserInputAfterDelay;
                await nolocationDialog.ShowAsync();
            }
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            var value = roamingSettings.Values["NotFirstRun"];

            if (value == null)
            {
                ScenarioControl.SelectedIndex = 2;
                roamingSettings.Values["NotFirstRun"] = true;
                HolderFrame.Navigate(typeof(About));
                Splitter.IsPaneOpen = false;
            }
            else if (value.ToString() == "True")
            {
                ScenarioControl.SelectedIndex = 0;
            }

        }
    }

    #region Scenario

    public class Scenario
    {
        public string Title { get; set; }
        public Type ClassType { get; set; }
    }

    public class ScenarioBindingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var s = value as Scenario;
            return s?.Title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }

    #endregion
}
