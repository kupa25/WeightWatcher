using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace WeightWatcher
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Viewmodel _items;

        public MainPage()
        {
            this.InitializeComponent();

            var items = new Viewmodel();
            items.Products.CollectionChanged += Products_CollectionChanged;

            ChartGrid.DataContext = items;

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        void Products_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //handle when dynamic refresh of the chart is required.
        }

        private void SaveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var newDailyWeight = Storage.AddValues(WeightTxt.Text);
                var item = (Viewmodel)ChartGrid.DataContext;
                item.Products.Add(newDailyWeight);
            }
            catch (Exception ex)
            {
                //handle this
            }
        }
    }
    public class Viewmodel
    {
        public Viewmodel()
        {
            this.Products = Storage.GetValuesFromCloud();
        }

        public ObservableCollection<DailyWeight> Products { get; set; }
    }
}
