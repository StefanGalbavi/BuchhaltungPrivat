using BuchhaltungPrivat.Class;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace BuchhaltungPrivat.Pages
{
    /// <summary>
    /// Interaktionslogik für Page4.xaml
    /// </summary>
    public partial class Page4 : Page
    {
        public readonly HBContext hbContext = new HBContext();
        private ComboBox sender;

        public List<SubCategory> subCategories { get; set; }

        public List<Category> categories { get; set; }

        public List<Konto> myKontos { get; set; }

        public Page4()
        {
            InitializeComponent();
            ListComboboxCategorys();
            ListComboboxSubCategory();
            ReadKonto();
        }

        public void ReadKonto()
        {
            myKontos = hbContext.Konto.ToList();
            ListKonten.ItemsSource = myKontos;
        }

        public void ListComboboxCategorys()
        {
            categories = hbContext.Category.ToList();
            cb_kategorie.ItemsSource = categories;
            cb_kategorie.SelectedIndex = 0;
        }

        public void ListComboboxSubCategory()
        {
            cb_rubrik.ItemsSource = subCategories;
            cb_rubrik.SelectedIndex = 0;
        }

        private void bt_kontoErstellen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateNewKonto();
        }

        private void CreateNewKonto()
        {
            var kontos = new Konto();
            if (cb_kategorie.SelectedIndex == 0)
            {
                kontos.KontoName = txb_kontoName.Text;
                kontos.KontoAmount = Math.Abs(Convert.ToDecimal(txb_kontoAmount.Text));
                kontos.KontoDate = DateTime.Now;
                hbContext.Konto?.Add(kontos);
                hbContext.SaveChanges();
                ReadKonto();
            }
            if (cb_kategorie.SelectedIndex == 1)
            {
                kontos.KontoName = txb_kontoName.Text;
                kontos.KontoAmount = -Math.Abs(Convert.ToDecimal(txb_kontoAmount.Text));
                kontos.KontoDate = DateTime.Now;
                hbContext.Konto?.Add(kontos);
                hbContext.SaveChanges();
                ReadKonto();
            }

            if (cb_kategorie.SelectedIndex == 0)
            {
                var booking = new Booking();
                booking.Amount = Math.Abs(Convert.ToDecimal(txb_kontoAmount.Text));
                booking.Date = DateTime.Now;
                booking.Konto = kontos;
                booking.SubCategory = subCategories[cb_rubrik.SelectedIndex];
                booking.Category = categories[cb_kategorie.SelectedIndex];
                hbContext.Booking.Add(booking);
                hbContext.SaveChanges();
            }
            if (cb_kategorie.SelectedIndex == 1)
            {
                var booking = new Booking();
                booking.Amount = -Math.Abs(Convert.ToDecimal(txb_kontoAmount.Text));
                booking.Date = DateTime.Now;
                booking.Konto = kontos;
                booking.SubCategory = subCategories[cb_rubrik.SelectedIndex];
                booking.Category = categories[cb_kategorie.SelectedIndex];
                hbContext.Booking.Add(booking);
                hbContext.SaveChanges();
            }
        }

        private void cb_rubrik_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox) sender;

            Category selctedCategory = (Category) cb_kategorie.SelectedItem;

            if (selctedCategory.CategoryId == 1)
            {
                subCategories = hbContext.Category.Include(c => c.SubCategories).Where(c => c.CategoryId == selctedCategory.CategoryId).First().SubCategories.ToList();
                cb_rubrik.ItemsSource = subCategories;
                cb_rubrik.SelectedIndex = 0;
            }

            if (selctedCategory.CategoryId == 2)
            {
                subCategories = hbContext.Category.Include(c => c.SubCategories).Where(c => c.CategoryId == selctedCategory.CategoryId).First().SubCategories.ToList();
                cb_rubrik.ItemsSource = subCategories;
                cb_rubrik.SelectedIndex = 0;
            }
        }
    }
}
