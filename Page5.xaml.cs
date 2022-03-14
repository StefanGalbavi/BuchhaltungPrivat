using BuchhaltungPrivat.Class;
using BuchhaltungPrivat.Class.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BuchhaltungPrivat.Pages
{
    /// <summary>
    /// Interaktionslogik für Page5.xaml
    /// </summary>
    public partial class Page5 : Page
    {
        public readonly HBContext hbContext = new HBContext();

        public List<Category> categories { get; set; }

        public List<SubCategory> subCategories { get; set; }

        public DateTime selectedDate { get; set; }

        public Page5()
        {
            InitializeComponent();
            selectedDate = DateTime.Now;
            datePicker.Text = DateTime.Now.Date.ToString();
            ReadDataCategoryEinahmen();
            ReadDataCategoryAusgabe();
        }

        public void ReadDataCategoryAusgabe()
        {

            var tmp = ReportingUtils.getReport(hbContext, "Ausgabe", selectedDate);

            var text = "";

            foreach (var category in tmp)
            {
                text += category.SubCategoryName + category.Amount + "\n";
            }


            ListReportAusgaben.ItemsSource = tmp;
        }

        public void ReadDataCategoryEinahmen()
        {

            var tmp = ReportingUtils.getReport(hbContext, "Einnahme", selectedDate);

            var text = "";

            foreach (var category in tmp)
            {
                text += category.SubCategoryName + category.Amount + "\n";
            }

            ListReportEinahmen.ItemsSource = tmp;
        }

        private void DatePicker_Opened(object? sender, RoutedEventArgs e)
        {
            if (sender == null)
                return;
            DatePicker datepicker = (DatePicker) sender;
            Popup popup = (Popup) datepicker.Template.FindName("PART_Popup", datepicker);
            Calendar cal = (Calendar) popup.Child;
            cal.DisplayModeChanged += Calender_DisplayModeChanged;
            cal.DisplayMode = CalendarMode.Decade;
        }

        private void Calender_DisplayModeChanged(object? sender, CalendarModeChangedEventArgs e)
        {
            if (sender == null)
                return;
            Calendar calendar = (Calendar) sender;

            if (calendar.DisplayMode == CalendarMode.Month)
            {
                calendar.SelectedDate = calendar.DisplayDate;

                datePicker.IsDropDownOpen = false;

                selectedDate = calendar.DisplayDate;

                ReadDataCategoryEinahmen();
                ReadDataCategoryAusgabe();
            }
        }

    }
}
