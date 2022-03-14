using BuchhaltungPrivat.Class;
using System;
using System.Linq;
using System.Windows;

namespace BuchhaltungPrivat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public IQueryable<Booking> BookingList { get; set; }

        public decimal Einnahmen { get; set; }

        public decimal Ausgaben { get; set; }

        public decimal Sum { get; set; }

        public DateTime selectedDate { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            CenterWindowOnScreen();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            frame.Navigate(new Uri("Page1.xaml", UriKind.RelativeOrAbsolute));

            DataContext = this;

            CurrentMonthSummary();

        }

        public void CurrentMonthSummary()
        {
            DataContext = this;

            HBContext hbContext = new HBContext();

            selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            BookingList = hbContext.Booking.Where(c => c.Date >= selectedDate);

            var Liste = BookingList.Where(c => c.CategoryId == 1);

            Einnahmen = BookingList.Where(c => c.CategoryId == 1).Sum(c => c.Amount);

            Ausgaben = BookingList.Where(c => c.CategoryId == 2).Sum(c => c.Amount);

            Sum = Einnahmen + Ausgaben;

        }

        private void SideMenuControl_SelectionChanged(object sender, EventArgs e)
        {
            CurrentMonthSummary();

            switch (MenuList.SelectedIndex)
            {
                case 0:
                    frame.Navigate(new Uri("Page1.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case 1:
                    frame.Navigate(new Uri("Page2.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case 2:
                    frame.Navigate(new Uri("Page3.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case 3:
                    frame.Navigate(new Uri("Page4.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case 4:
                    frame.Navigate(new Uri("Page5.xaml", UriKind.RelativeOrAbsolute));
                    break;
                case 5:
                    Close();
                    break;
            }
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        private void btn_nextSide_Click(object sender, RoutedEventArgs e)
        {
            if (frame.NavigationService.CanGoForward)
                frame.NavigationService.GoForward();
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            if (frame.NavigationService.CanGoBack)
                frame.NavigationService.GoBack();
        }

    }
}
