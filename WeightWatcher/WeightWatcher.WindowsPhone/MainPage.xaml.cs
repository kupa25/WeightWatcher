using Windows.UI;
using Windows.UI.Popups;
using Syncfusion.XlsIO;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Color = Syncfusion.DocIO.DLS.Color;

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

        private async void ExportButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            #region Setting output location
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile storageFile = await local.CreateFileAsync("WeightChart.xlsx", CreationCollisionOption.ReplaceExisting);

            if (storageFile == null)
                return;
            #endregion

            #region Initializing workbook
            //Instantiate excel Engine
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;

            application.DefaultVersion = ExcelVersion.Excel2013;

            IWorkbook workbook;

            if (application.DefaultVersion != ExcelVersion.Excel97to2003)
            {
                Assembly assembly = typeof(MainPage).GetTypeInfo().Assembly;
                string resourcePath = "WeightWatcher.Template.Template.xlsx";
                Stream fileStream = assembly.GetManifestResourceStream(resourcePath);
                workbook = await application.Workbooks.OpenAsync(fileStream);
            }
            else
            {
                workbook = application.Workbooks.Create(1);
            }

            IWorksheet sheet = workbook.Worksheets[0];
            #endregion

            #region Creating chart datasource

            IWorksheet chartSheet;

            //if (application.DefaultVersion != ExcelVersion.Excel97to2003)
            //{
            //    chartSheet = workbook.Worksheets.Create("Chart Data");
            //}
            //else
            //{
                chartSheet = workbook.Worksheets[0];
            //}

            // Entering the Datas for the chart
            chartSheet.Range["A1"].Text = "Jacksonville, FL";
            chartSheet.Range["A1:D1"].Merge();
            chartSheet.Range["A1"].CellStyle.Font.Bold = true;

            chartSheet.Range["B3"].Text = "Precipitation,in.";
            chartSheet.Range["C3"].Text = "Temperature,deg.F";

            chartSheet.Range["A4"].Text = "Jan";
            chartSheet.Range["A5"].Text = "Feb";
            chartSheet.Range["A6"].Text = "March";
            chartSheet.Range["A7"].Text = "Apr";
            chartSheet.Range["A8"].Text = "May";
            chartSheet.Range["A9"].Text = "June";
            chartSheet.Range["A10"].Text = "July";
            chartSheet.Range["A11"].Text = "Aug";
            chartSheet.Range["A12"].Text = "Sept";
            chartSheet.Range["A13"].Text = "Oct";
            chartSheet.Range["A14"].Text = "Nov";
            chartSheet.Range["A15"].Text = "Dec";

            //chartSheet.Range["B4"].Number = 10.9;
            //chartSheet.Range["B5"].Number = 8.9;
            //chartSheet.Range["B6"].Number = 8.6;
            //chartSheet.Range["B7"].Number = 4.8;
            //chartSheet.Range["B8"].Number = 3.2;
            //chartSheet.Range["B9"].Number = 1.4;
            //chartSheet.Range["B10"].Number = 0.6;
            //chartSheet.Range["B11"].Number = 0.7;
            //chartSheet.Range["B12"].Number = 1.7;
            //chartSheet.Range["B13"].Number = 5.4;
            //chartSheet.Range["B14"].Number = 9.0;
            //chartSheet.Range["B15"].Number = 10.4;

            //chartSheet.Range["C4"].Number = 4.5;
            //chartSheet.Range["C5"].Number = 2.7;
            //chartSheet.Range["C6"].Number = 9.9;
            //chartSheet.Range["C7"].Number = 4.2;
            //chartSheet.Range["C8"].Number = 6.1;
            //chartSheet.Range["C9"].Number = 5.3;
            //chartSheet.Range["C10"].Number = 3.1;
            //chartSheet.Range["C11"].Number = 7;
            //chartSheet.Range["C12"].Number = 4.5;
            //chartSheet.Range["C13"].Number = 8.4;
            //chartSheet.Range["C14"].Number = 3.1;
            //chartSheet.Range["C15"].Number = 8.8;
            chartSheet.UsedRange.AutofitColumns();
            #endregion

            #region Saving workbook and disposing objects

            await workbook.SaveAsAsync(storageFile);
            workbook.Close();
            excelEngine.Dispose();

            #endregion

            #region Save acknowledgement and launching of ouput file
            MessageDialog msgDialog = new MessageDialog("Do you want to view the Document?", "File has been created successfully.");

            UICommand yesCmd = new UICommand("Yes");
            msgDialog.Commands.Add(yesCmd);
            UICommand noCmd = new UICommand("No");
            msgDialog.Commands.Add(noCmd);
            IUICommand cmd = await msgDialog.ShowAsync();
            if (cmd == yesCmd)
            {
                // Launch the saved file
                bool success = await Windows.System.Launcher.LaunchFileAsync(storageFile);
            }
            #endregion

        }

        private void AboutButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //Clicked the About button
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
