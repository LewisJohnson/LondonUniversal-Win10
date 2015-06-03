
using Windows.UI.Xaml;

namespace London_Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class About
    {
        public About()
        {
            InitializeComponent();
        }


        private void About_OnLoaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer.
            MainPage._AboutItem.Visibility = Visibility.Collapsed;
            AboutPage.Height = Window.Current.Bounds.Height;
        }
    }
}
