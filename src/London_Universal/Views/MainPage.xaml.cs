using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.System;
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

        public static ObservableCollection<BikePointRootObject> BikePointCollection
        { get; private set; } = new ObservableCollection<BikePointRootObject>();

        public static ObservableCollection<SuperCycleRootObject> SuperCycleCollection
        { get; private set; } = new ObservableCollection<SuperCycleRootObject>();

        private readonly List<Scenario> _scenarios = new List<Scenario>
        {
            new Scenario {Title = "Map", ClassType = typeof (FullMap)},
            new Scenario {Title = "Settings", ClassType = typeof (Settings)}
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
            var s = scenarioListBox.SelectedItem as Scenario;
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
            ScenarioControl.SelectedIndex = 0;

            BikePointCollection = await DataFetch.BikePointsTask();
            SuperCycleCollection = await DataFetch.SuperHighwaysTask();
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
            return s.Title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return true;
        }
    }

    #endregion
}