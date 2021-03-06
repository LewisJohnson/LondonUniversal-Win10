﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using London_Universal.DataModels;
using Newtonsoft.Json.Linq;

namespace London_Universal.Views
{
    public partial class FullMap
    {
        #region Properties

        public static bool LandMarks;
        public static bool Traffic;
        public static bool BusinessFeat;
        public static bool PedeFeat;
        public static bool Is3DEnabled;
        public static bool ForceControls;
        public static MapStyle MapStyle;
        public static MapColorScheme MapColorScheme;

        private readonly MapIcon _userLocationIcon = new MapIcon
        {
            Title = "Me",
            NormalizedAnchorPoint = new Point(0.5, 1.0),
            CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible
        };

        private MapRouteView _directionsMapRoute;
        private readonly string _emptyListHint = "Hint: You can search for all by just pressing the search icon";

        #endregion

        #region Constructor

        public FullMap()
        {
            InitializeComponent();
            Is3DEnabled = MapControl.Is3DSupported;
        }

        #endregion

        #region Map Funcs

        private async Task UsersLocation(bool movemap)
        {
            var geolocator = new Geolocator();
            var geoposition = await geolocator.GetGeopositionAsync();
            if (movemap)
            {
                MapControl.ZoomLevel = 17;
                await MapControl.TrySetViewAsync(geoposition.Coordinate.Point);
            }
            _userLocationIcon.Location = new Geopoint(new BasicGeoposition
            {
                Latitude = geoposition.Coordinate.Point.Position.Latitude,
                Longitude = geoposition.Coordinate.Point.Position.Longitude
            });

            MapControl.MapElements.Add(_userLocationIcon);
        }

        private async void UpdateMapView()
        {
            MapControl?.MapElements.Clear();
            MapControl?.Routes.Clear();

            if (MainPage.ShowBikePins)
            {
                foreach (var mapIcon in MainPage.BikePointCollection.Select(item => new MapIcon
                {
                    Image =
                        RandomAccessStreamReference.CreateFromUri(
                            new Uri("ms-appx:///Assets/cycle-hire-pushpin-icon.png")),
                    Location = new Geopoint(new BasicGeoposition
                    {
                        Latitude = item.lat,
                        Longitude = item.lon
                    }),
                    Title = "BikePoint",
                    NormalizedAnchorPoint = new Point(0.5, 1.0)
                }))
                {
                    MapControl?.MapElements.Add(mapIcon);
                }
            }

            if (MainPage.ShowSuperHighways)
            {
                    foreach (var cycleRoutes in MainPage.SuperCycleCollection)
                    {
                        if (cycleRoutes.Geography.Type == SuperCyleHighwayType.LineString.ToString())
                        {
                            var line = new MapPolyline();
                            var route = cycleRoutes.Geography.Coordinates.Select(loc => new BasicGeoposition
                            {
                                Latitude = double.Parse(loc[1].ToString()),
                                Longitude = double.Parse(loc[0].ToString())
                            }).ToList();

                            line.Path = new Geopath(route);
                            line.StrokeColor = Colors.Chartreuse;
                            line.StrokeThickness = 5;

                            MapControl?.MapElements.Add(line);

                        }
                        else if (cycleRoutes.Geography.Type == SuperCyleHighwayType.MultiLineString.ToString())
                        {
                            var line = new MapPolyline();

                            var route = (from subItem in cycleRoutes.Geography.Coordinates
                                from loc in subItem.Cast<JArray>()
                                select new BasicGeoposition
                                {
                                    Latitude = double.Parse(loc[1].ToString()),
                                    Longitude = double.Parse(loc[0].ToString())
                                }).ToList();

                            line.Path = new Geopath(route);

                            var rand = new Random();

                            line.StrokeColor = Color.FromArgb(255,
                                byte.Parse(rand.Next(0, 255).ToString()),
                                byte.Parse(rand.Next(0, 255).ToString()),
                                byte.Parse(rand.Next(0, 255).ToString()));

                            line.StrokeDashed = true;
                            line.StrokeThickness = 5;
                            MapControl?.MapElements.Add(line);
                        }
                    }
                
            }

            if (MainPage.ShowCabWise)
            {
                    foreach (var item in MainPage.CabWiseCollection.operators.operatorList.Select(item => new MapIcon
                    {
                        Image =
        RandomAccessStreamReference.CreateFromUri(
            new Uri("ms-appx:///Assets/cabwise-pushpin-icon.png")),
                        Location = new Geopoint(new BasicGeoposition
                        {
                            Latitude = item.latitude,
                            Longitude = item.longitude
                        }),
                        Title = "CabWise",
                        NormalizedAnchorPoint = new Point(0.5, 1.0)
                    }))
                    {
                        MapControl?.MapElements.Add(item);
                    }
            }

            //Add user icon
            await UsersLocation(false);
        }

        private void FullMap_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MapControl.ZoomInteractionMode = ForceControls
                ? MapInteractionMode.GestureAndControl
                : MapInteractionMode.Auto;
        }

        #endregion

        #region Loading Funcs

        private async void MapPage_Loading(FrameworkElement sender, object args)
        {
            MapControl.MapServiceToken = "Ahs_1OPV3NCxO6Q66lk7w94wXTyRLPGEG6YrIPVDgZfFH47jbzkS7BbXV5Dshoj7";

            await UsersLocation(true);
        }

        private void MapPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            MapControl.MapServiceToken = "Ahs_1OPV3NCxO6Q66lk7w94wXTyRLPGEG6YrIPVDgZfFH47jbzkS7BbXV5Dshoj7";
            MapControl.TrafficFlowVisible = Traffic;
            MapControl.BusinessLandmarksVisible = BusinessFeat;
            MapControl.PedestrianFeaturesVisible = PedeFeat;
            MapControl.ZoomInteractionMode = ForceControls
                ? MapInteractionMode.GestureAndControl
                : MapInteractionMode.Auto;
            MapControl.LandmarksVisible = LandMarks;
            MapControl.Style = MapStyle;
            MapControl.ColorScheme = MapColorScheme;

            SearchListView.Items.Add(_emptyListHint);
        }

        #endregion

        #region OnClick Events

        private async void FindYou_OnClick(object sender, RoutedEventArgs e) => await UsersLocation(true);

        private void MenuFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            // ReSharper disable PossibleNullReferenceException
            var selectedItem = sender as ToggleMenuFlyoutItem;

            switch (selectedItem?.Tag.ToString())
            {
                case "Bike":
                    MainPage.ShowBikePins = selectedItem.IsChecked;
                    break;
                case "Oyster":
                    MainPage.ShowOysterPins = selectedItem.IsChecked;
                    break;
                case "Tube":
                    MainPage.ShowTubePins = selectedItem.IsChecked;
                    break;
                case "Bus":
                    MainPage.ShowBusPins = selectedItem.IsChecked;
                    break;
                case "CycleSuper":
                    MainPage.ShowSuperHighways = selectedItem.IsChecked;
                    break;
                case "CabWise":
                    MainPage.ShowCabWise = selectedItem.IsChecked;

                    break;
            }
            // ReSharper restore PossibleNullReferenceException
            UpdateMapView();
        }

        private async void MapControl_OnMapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            var element = args.MapElements[0] as MapIcon;

            switch (element?.Title)
            {
                case "BikePoint":
                    foreach (var item in MainPage.BikePointCollection.Where(item => element != null && item.lat == element.Location.Position.Latitude))
                    {
                        await BikeInfoBox(item, element);
                    }
                    break;

                case "CabWise":
                    foreach (var item in MainPage.CabWiseCollection.operators.operatorList.Where(item => element != null && item.latitude == element.Location.Position.Latitude))
                    {
                        CabWiseInfoBox(item, element);
                    }
                    break;

                case "Me":
                    await new MessageDialog("You are here").ShowAsync();
                    break;
            }

        }

        #endregion

        #region InfoBox Funcs

        private async void BikeInfoBoxOnTapped(object sender, TappedRoutedEventArgs e, BikePointRootObject item, double lat, double lon)
        {
            var stack = sender as StackPanel;
            var directions = stack?.Children.First() as ToggleButton;
            var expandInfoToggle = stack?.Children.Last() as ToggleButton;


            switch (e.OriginalSource.ToString())
            {
                case "Windows.UI.Xaml.Controls.Primitives.ToggleButton":
                case "Windows.UI.Xaml.Controls.Grid":

                    if (directions != null && directions.IsChecked == true)
                    {
                        await NewDirections(lat, lon);
                    }
                    if (expandInfoToggle != null && expandInfoToggle.IsChecked == true)
                    {
                        expandInfoToggle.Visibility = Visibility.Collapsed;
                        expandInfoToggle.Height = 0;
                        expandInfoToggle.IsChecked = false;
                        BikeExtendedInfoBox(item, stack);
                    }
                    break;

                case "Windows.UI.Xaml.Controls.StackPanel":
                case "Windows.UI.Xaml.Controls.TextBlock":
                    {
                        var opacityAnim = new DoubleAnimation
                        {
                            Duration = new Duration(TimeSpan.FromSeconds(0.1)),
                            To = 0,
                            EasingFunction = new ExponentialEase()
                        };
                        var sb2 = new Storyboard
                        {
                            Duration = new Duration(TimeSpan.FromSeconds(0.1))
                        };
                        sb2.Children.Add(opacityAnim);
                        Storyboard.SetTarget(opacityAnim, stack);
                        Storyboard.SetTargetProperty(opacityAnim, "Opacity");
                        sb2.Begin();
                        sb2.Completed += (o, o1) => MapControl.Children.Remove(stack);
                        if (_directionsMapRoute != null)
                        {
                            MapControl.Routes.Remove(_directionsMapRoute);
                        }
                    }
                    break;
            }
        }

        private async void CabWiseBoxOnTapped(object sender, TappedRoutedEventArgs e, CabWiseOperatorList item, double lat, double lon)
        {
            var stack = sender as StackPanel;
            var directions = stack?.Children.First() as ToggleButton;
            var expandInfoToggle = stack?.Children.Last() as ToggleButton;


            switch (e.OriginalSource.ToString())
            {
                case "Windows.UI.Xaml.Controls.Primitives.ToggleButton":
                case "Windows.UI.Xaml.Controls.Grid":

                    if (directions != null && directions.IsChecked == true)
                    {
                        await NewDirections(lat, lon);
                    }
                    if (expandInfoToggle != null && expandInfoToggle.IsChecked == true)
                    {
                        expandInfoToggle.Visibility = Visibility.Collapsed;
                        expandInfoToggle.Height = 0;
                        expandInfoToggle.IsChecked = false;
                        CabWiseExtendedInfoBox(item, stack);
                    }
                    break;

                case "Windows.UI.Xaml.Controls.StackPanel":
                case "Windows.UI.Xaml.Controls.TextBlock":
                    {
                        var opacityAnim = new DoubleAnimation
                        {
                            Duration = new Duration(TimeSpan.FromSeconds(0.1)),
                            To = 0,
                            EasingFunction = new ExponentialEase()
                        };
                        var sb2 = new Storyboard
                        {
                            Duration = new Duration(TimeSpan.FromSeconds(0.1))
                        };
                        sb2.Children.Add(opacityAnim);
                        Storyboard.SetTarget(opacityAnim, stack);
                        Storyboard.SetTargetProperty(opacityAnim, "Opacity");
                        sb2.Begin();
                        sb2.Completed += (o, o1) => MapControl.Children.Remove(stack);
                        if (_directionsMapRoute != null)
                        {
                            MapControl.Routes.Remove(_directionsMapRoute);
                        }
                    }
                    break;
            }
        }

        private async Task NewDirections(double lat, double lon)
        {
            // Start at user location

            var startPoint = new Geopoint(new BasicGeoposition
            {
                Latitude = _userLocationIcon.Location.Position.Latitude,
                Longitude = _userLocationIcon.Location.Position.Longitude
            });

            // End at the bikepoint.
            var endPoint = new Geopoint(new BasicGeoposition
            {
                Latitude = lat,
                Longitude = lon
            });

            // Get the route between the points.
            var routeResult =
                await MapRouteFinder.GetDrivingRouteAsync(
                    startPoint,
                    endPoint,
                    MapRouteOptimization.Distance,
                    MapRouteRestrictions.Highways);

            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                if (_directionsMapRoute != null)
                {
                    MapControl.Routes.Remove(_directionsMapRoute);
                }
                // Use the route to initialize a MapRouteView.
                _directionsMapRoute = new MapRouteView(routeResult.Route)
                {
                    RouteColor = Colors.DodgerBlue,
                    OutlineColor = Colors.AntiqueWhite
                };

                // Add the new MapRouteView to the Routes collection
                // of the MapControl.
                MapControl.Routes.Add(_directionsMapRoute);
            }
            else
            {
                await
                    new MessageDialog($"We got an error: {routeResult.Status}. Sorry.", "Uh-oh.").ShowAsync();
            }
        }

        private async Task BikeInfoBox(BikePointRootObject item, MapIcon bkeElement)
        {
            var infoBox = new StackPanel
            {
                Opacity = 0,
                Style = (Style)Application.Current.Resources["InfoBox"]
            };
            var name = new TextBlock
            {
                Text = "Name: " + item.commonName,
                Foreground = new SolidColorBrush(Colors.White),
                Margin = new Thickness(5)
            };
            var hire = new TextBlock
            {
                Text = "Bikes: " + item.additionalProperties[6].value,
                Foreground = new SolidColorBrush(Colors.White),
                Margin = new Thickness(5)
            };
            var emptyDocks = new TextBlock
            {
                Text = "Empty Docks: " + item.additionalProperties[7].value,
                Foreground = new SolidColorBrush(Colors.White),
                Margin = new Thickness(5)
            };

            var expand = new ToggleButton
            {
                Content = "▼",
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 8,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            var dirBtn = new ToggleButton
            {
                Content = "Directions",
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 10,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            infoBox.Children.Add(dirBtn);
            infoBox.Children.Add(name);
            infoBox.Children.Add(hire);
            infoBox.Children.Add(emptyDocks);
            infoBox.Children.Add(expand);

            infoBox.Background = new SolidColorBrush(Color.FromArgb(255, 116, 116, 116));
            MapControl.SetLocation(infoBox, new Geopoint(new BasicGeoposition
            {
                Latitude = bkeElement.Location.Position.Latitude,
                Longitude = bkeElement.Location.Position.Longitude
            }));

            MapControl.SetNormalizedAnchorPoint(infoBox, new Point(0, 0));
            MapControl.Children.Add(infoBox);

            var opacityAnim = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                To = 0.85,
                EasingFunction = new ExponentialEase()
            };
            var sb = new Storyboard
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };

            sb.Children.Add(opacityAnim);
            Storyboard.SetTarget(opacityAnim, infoBox);
            Storyboard.SetTargetProperty(opacityAnim, "Opacity");

            sb.Begin();
            infoBox.Tapped +=
                (o, eventArgs) =>
                    BikeInfoBoxOnTapped(o, eventArgs, item, bkeElement.Location.Position.Latitude,
                        bkeElement.Location.Position.Longitude);
            //Checks if bikepoint is installed
            if (item.additionalProperties[1].value != "true")
                await new MessageDialog("This bikepoint is not installed yet.", "Alert").ShowAsync();

            //Checks if bikepoint is locked
            if (item.additionalProperties[2].value != "false")
                await new MessageDialog("This bikepoint is locked.", "Alert").ShowAsync();

            //Checks if bikepoint is going to be removed
            if (item.additionalProperties[4].value != "")
                await
                    new MessageDialog(
                        $"This bikepoint is going to be removed on {item.additionalProperties[4].value}",
                        "Woah.").ShowAsync();

            //Checks if bikepoint is temporary
            if (item.additionalProperties[5].value != "false")
                await
                    new MessageDialog(
                        "This bikepoint is only temporary. Be sure to check the status before arriving.",
                        "Woah.").ShowAsync();
        }
        private static void BikeExtendedInfoBox(BikePointRootObject item, StackPanel l)
        {
            var property0 = new TextBlock
            {
                Text = "TerminalName: " + item.additionalProperties[0].value,
                Style = (Style) Application.Current.Resources["InfoBoxText"]
            };
            var property1 = new TextBlock
            {
                Text = "Installed: " + item.additionalProperties[1].value,
                Style = (Style)Application.Current.Resources["InfoBoxText"]
            };
            var property2 = new TextBlock
            {
                Text = "Locked: " + item.additionalProperties[2].value,
                Style = (Style)Application.Current.Resources["InfoBoxText"]
            };
            var property3 = new TextBlock
            {
                Text = "InstallDate: " + item.additionalProperties[3].value,
                Style = (Style)Application.Current.Resources["InfoBoxText"]
            };
            var property4 = new TextBlock
            {
                Text = "RemovalDate: " + item.additionalProperties[4].value,
                Style = (Style)Application.Current.Resources["InfoBoxText"]
            };
            var property5 = new TextBlock
            {
                Text = "Temporary: " + item.additionalProperties[5].value,
                Style = (Style)Application.Current.Resources["InfoBoxText"]
            };
            var property8 = new TextBlock
            {
                Text = "NbDocks: " + item.additionalProperties[8].value,
                Style = (Style)Application.Current.Resources["InfoBoxText"]
            };

            l.Children.Add(property8);
            l.Children.Add(property0);
            l.Children.Add(property1);
            l.Children.Add(property2);
            l.Children.Add(property3);
            l.Children.Add(property5);

            if (property4.Text != "RemovalDate: ")
            {
                l.Children.Add(property4);
            }
        }

        private void CabWiseInfoBox(CabWiseOperatorList item, MapIcon cabElement)
        {
            var infoBox = new StackPanel
            {
                Style = (Style)Application.Current.Resources["InfoBox"],
                Opacity = 0
            };
            var name = new TextBlock
            {
                Text = "Name: " + item.tradingName,
                Style = (Style)Application.Current.Resources["InfoBoxText"]
            };
            var address = new TextBlock
            {
                Text = "Address: " + item.postcode + " " + item.addressLine1,
                Style = (Style)Application.Current.Resources["InfoBoxText"]
            };

            var address2 = new TextBlock
            {
                Text =  item.addressLine2,
                Style = (Style)Application.Current.Resources["InfoBoxText"]
            };

            var address3 = new TextBlock
            {
                Text =  item.addressLine2,
                Style = (Style)Application.Current.Resources["InfoBoxText"]
            };

            var emptyDocks = new TextBlock
            {
                Text = "Number: " + item.bookingsPhoneNumber,
                Style = (Style)Application.Current.Resources["InfoBoxText"]
            };

            var expand = new ToggleButton
            {
                Content = "▼",
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 8,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            var dirBtn = new ToggleButton
            {
                Content = "Directions",
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 10,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            infoBox.Children.Add(dirBtn);
            infoBox.Children.Add(name);
            infoBox.Children.Add(address);
            if (item.addressLine2 != ",")
            {
                infoBox.Children.Add(address2);
            }
            if (item.addressLine3 != ",")
            {
                infoBox.Children.Add(address3);
            }

            infoBox.Children.Add(emptyDocks);
            infoBox.Children.Add(expand);


            MapControl.SetLocation(infoBox, new Geopoint(new BasicGeoposition
            {
                Latitude = cabElement.Location.Position.Latitude,
                Longitude = cabElement.Location.Position.Longitude
            }));

            MapControl.SetNormalizedAnchorPoint(infoBox, new Point(0, 0));
            MapControl.Children.Add(infoBox);

            var opacityAnim = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                To = 0.85,
                EasingFunction = new ExponentialEase()
            };
            var sb = new Storyboard
            {
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };
            
            sb.Children.Add(opacityAnim);
            Storyboard.SetTarget(opacityAnim, infoBox);
            Storyboard.SetTargetProperty(opacityAnim, "Opacity");
            
            sb.Begin();

            infoBox.Tapped +=
                (o, eventArgs) =>
                    CabWiseBoxOnTapped(o, eventArgs, item, item.latitude,
                        item.longitude);
        }
        private static void CabWiseExtendedInfoBox(CabWiseOperatorList item, StackPanel l)
        {

            var monThu = new TextBlock
            {
                Text = "Mon-Thu: " + item.startTimeMonThu + "-" + item.endTimeMonThu,
                Style = (Style) Application.Current.Resources["InfoBoxText"]
            };
            var fri = new TextBlock
            {
                Text = "Fri: " + item.startTimeFri + "-" + item.endTimeFri,
                Style = (Style) Application.Current.Resources["InfoBoxText"]
            };
            var sat = new TextBlock
            {
                Text = "Sat: " + item.startTimeSat + "-" + item.endTimeSat,
                Style = (Style) Application.Current.Resources["InfoBoxText"]
            };
            var sun = new TextBlock
            {
                Text = "Sun: " + item.startTimeSun + "-" +  item.endTimeSun,
                Style = (Style) Application.Current.Resources["InfoBoxText"]
            };

            if (item.bookingsEmail != string.Empty)
            {
                l.Children.Add(new TextBlock
                {
                    Text = "Email: " + item.bookingsEmail,
                    Style = (Style)Application.Current.Resources["InfoBoxText"]
                });
            }

            if (item.hoursOfOperation24X7)
            {
                l.Children.Add(new TextBlock
                {
                    Text = "Open 24/7 ",
                    Style = (Style) Application.Current.Resources["InfoBoxText"]
                }
                    );
            }
            else
            {
                l.Children.Add(monThu);
                l.Children.Add(fri);
                l.Children.Add(sat);
                l.Children.Add(sun);
            }

            if (item.creditDebitCard)
            {
                l.Children.Add(new TextBlock
                {
                    Text = "Accepts cards",
                    Style = (Style) Application.Current.Resources["InfoBoxText"]
                }
                    );
            }
            else
            {
                l.Children.Add(new TextBlock
                {
                    Text = "Does not accept cards",
                    Style = (Style) Application.Current.Resources["InfoBoxText"]
                }
                    );
            }

            if (item.wheelchairAccessible)
            {
                l.Children.Add(new TextBlock
                {
                    Text = "Wheelchair accessible",
                    Style = (Style) Application.Current.Resources["InfoBoxText"]
                }
                    );
            }
            else
            {
                l.Children.Add(new TextBlock
                {
                    Text = "Not Wheelchair accessible",
                    Style = (Style) Application.Current.Resources["InfoBoxText"]
                }
                    );
            }

            if (item.publicWaitingRoom)
            {
                l.Children.Add(new TextBlock
                {
                    Text = "Has waiting room",
                    Style = (Style) Application.Current.Resources["InfoBoxText"]
                }
                );
            }
        }

        #endregion

        #region SearchBox Funcs

        private void Search_OnClick(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            Splitter.IsPaneOpen = (Splitter.IsPaneOpen != true);

            SearchIcon.FontSize = 20;
            SearchIcon.Glyph = "";
            SearchIcon.Foreground = new SolidColorBrush(Colors.Black);

            SearchBorder.CornerRadius = new CornerRadius(0);
            SearchBorder.Margin = new Thickness(0, 2, 0, 0);
            SearchBorder.Background = new SolidColorBrush(Colors.Transparent);
            SearchBorder.Height = 50;
        }

        private void SplitView_OnPaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            SearchIcon.FontSize = 30;
            SearchIcon.Glyph = "";
            SearchIcon.Foreground = new SolidColorBrush(Colors.White);

            SearchBorder.CornerRadius = new CornerRadius(90);
            SearchBorder.Margin = new Thickness(15);
            SearchBorder.Background = (Brush)Application.Current.Resources["TransDodgerBlue"];
            SearchBorder.Height = 54;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            SearchListView?.Items.Clear();
            SearchListView?.Items.Add(_emptyListHint);
        }

        private void Search_OnGotFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Splitter.IsPaneOpen == false)
            {
                Splitter.IsPaneOpen = true;
            }
        }

        private void SearchList_OnSelectionChanged(object sender, ItemClickEventArgs e)
        {
            var selectedCombo = SearchCombo.SelectedItem as ComboBoxItem;

            switch (selectedCombo?.Content.ToString())
            {
                case "Bike":
                    BikeSearch(e.ClickedItem.ToString(), true);
                    break;

                case "Superhighways":
                    SuperHighwaySearch(e.ClickedItem.ToString(), true);
                    break;

                case "CabWise":
                    CabWiseSearch(e.ClickedItem.ToString(), true);
                    break;
            }
        }

        private void SearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            SearchListView.Items.Clear();

            var selectedCombo = SearchCombo.SelectedItem as ComboBoxItem;
            switch (selectedCombo?.Content.ToString())
            {
                case "Bike":
                    BikeSearch(SearchBox.QueryText, false);
                    break;

                case "Superhighways":
                    SuperHighwaySearch(SearchBox.QueryText, false);
                    break;

                case "CabWise":
                    CabWiseSearch(SearchBox.QueryText, false);
                    break;
            }

            UpdateMapView();
        }

        private async void BikeSearch(string query, bool moveTo)
        {
            //CommandBar Layers Flyout Item (BIKE)
            var layerbikebtn = LayersMenuFlyout.Items[0] as ToggleMenuFlyoutItem;
            if (MainPage.ShowBikePins == false)
            {
                MainPage.ShowBikePins = true;
            }

            if (layerbikebtn?.IsChecked == false)
            {
                layerbikebtn.IsChecked = true;
            }

            foreach (
                var item in
                    MainPage.BikePointCollection.Where(item => item.commonName.ToLower().Contains(query.ToLower())))
            {
                if (moveTo)
                {
                    await MapControl.TrySetViewAsync(new Geopoint(new BasicGeoposition
                    {
                        Latitude = item.lat,
                        Longitude = item.lon
                    }));
                }
                else
                {
                    SearchListView.Items.Add(item.commonName);
                }
            }
        }

        private async void SuperHighwaySearch(string query, bool moveTo)
        {
            //CommandBar Layers Flyout Item (SUPER CYCLE)
            var layerbikebtn = LayersMenuFlyout.Items[1] as ToggleMenuFlyoutItem;
            if (MainPage.ShowSuperHighways == false)
            {
                MainPage.ShowSuperHighways = true;
            }

            if (layerbikebtn?.IsChecked == false)
            {
                layerbikebtn.IsChecked = true;
            }

            foreach (
                var item in MainPage.SuperCycleCollection.Where(item => item.Label.ToLower().Contains(query.ToLower())))
            {
                if (moveTo)
                {
                    if (item.Geography.Type == SuperCyleHighwayType.LineString.ToString())
                    {
                        var route = item.Geography.Coordinates
                            .Select(point => new BasicGeoposition
                            {
                                Longitude = double.Parse(point[0].ToString()),
                                Latitude = double.Parse(point[1].ToString())
                            }).ToList();

                        await
                            MapControl.TrySetViewBoundsAsync(GeoboundingBox.TryCompute(route), new Thickness(2),
                                MapAnimationKind.Bow);
                    }
                    else if (item.Geography.Type == SuperCyleHighwayType.MultiLineString.ToString())
                    {
                        var route = (from subItem in item.Geography.Coordinates
                                     from JArray loc in subItem
                                     select new BasicGeoposition
                                     {
                                         Latitude = double.Parse(loc[1].ToString()),
                                         Longitude = double.Parse(loc[0].ToString())
                                     }).ToList();

                        await
                            MapControl.TrySetViewBoundsAsync(GeoboundingBox.TryCompute(route), new Thickness(10),
                                MapAnimationKind.Bow);
                    }

                }
                else
                {
                    SearchListView.Items.Add(item.Label);
                }



            }

        }

        private async void CabWiseSearch(string query, bool moveTo)
        {
            //CommandBar Layers Flyout Item (CABWISE)
            var layerbikebtn = LayersMenuFlyout.Items[2] as ToggleMenuFlyoutItem;
            if (MainPage.ShowCabWise == false)
            {
                MainPage.ShowCabWise = true;
            }

            if (layerbikebtn?.IsChecked == false)
            {
                layerbikebtn.IsChecked = true;
            }

            foreach (
                var item in
                    MainPage.CabWiseCollection.operators.operatorList.Where(
                        item => item.tradingName.ToLower().Contains(query.ToLower())))
            {
                if (moveTo)
                {
                    await MapControl.TrySetViewAsync(new Geopoint(new BasicGeoposition
                    {
                        Latitude = item.latitude,
                        Longitude = item.longitude
                    }));
                }
                else
                {
                    SearchListView.Items.Add(item.tradingName);
                }
            }

        }

        #endregion


    }
}