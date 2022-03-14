using BuchhaltungPrivat.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace BuchhaltungPrivat.Pages
{
    /// <summary>
    /// Interaktionslogik für Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        public readonly HBContext hbContext = new HBContext();

        public List<StandingOrder> standingOrders { get; set; }

        public List<Booking> bookings { get; set; }

        public List<Konto> kontos { get; set; }

        public List<Category> category { get; set; }

        public List<SubCategory> subCategories { get; set; }

        private DateTime selectedDate;

        public Page2()
        {
            InitializeComponent();
            selectedDate = DateTime.Now;
            ReadStandingOrderList();
            ListComboboxCategorys();
            ListComboboxKonto();
        }

        public void ReadStandingOrderList()
        {

            standingOrders = hbContext.StandingOrder.Include(b => b.Konto).Include(b => b.SubCategory).ToList();

            //List<ListViewItem> list = new List<ListViewItem>();

            //for (int i = 0; i < standingOrders.Count; i++)
            //{
            //    ListViewItem item = new ListViewItem();
            //    item.Content = standingOrders[i];
            //    if (standingOrders[i].Amount < 0)
            //    {
            //        item.Foreground = Brushes.Red;

            //    }
            //    list.Add(item);
            //}

            ListStandingOrders.ItemsSource = standingOrders;
        }

        public void ListComboboxCategorys()
        {
            category = hbContext.Category.ToList();
            cb_category.ItemsSource = category;
            cb_category.SelectedIndex = 0;
        }

        public void ListComboboxKonto()
        {
            kontos = hbContext.Konto.ToList();
            cb_kategorie.ItemsSource = kontos;
            cb_kategorie.SelectedIndex = 0;
        }

        private void bt_orderMaking_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
