using BuchhaltungPrivat.Class;
using BuchhaltungPrivat.Pages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BuchhaltungPrivat.PagesPopup
{
    /// <summary>
    /// Interaktionslogik für Rebooking.xaml
    /// </summary>
    public partial class Rebooking : Window
    {
        HBContext hbContext = new HBContext();

        public List<Konto> konto = new List<Konto>();

        private List<SubCategory> subCategories = new List<SubCategory>();

        private Page1 parent;

        public Rebooking(Page1 parent)
        {
            InitializeComponent();
            ListComboboxKontoUmbuchenAus();
            ListComboboxKontoUmbuchenEin();
            ListComboboxSubCategoryUmbuchung();
            this.parent = parent;
        }

        //Methoden Comboboxen mit Liste füllen
        public void ListComboboxKontoUmbuchenAus()
        {
            konto = hbContext.Konto.ToList();
            cb_kontoAusgehend.ItemsSource = konto;
            cb_kontoAusgehend.SelectedIndex = 0;
        }

        public void ListComboboxKontoUmbuchenEin()
        {
            konto = hbContext.Konto.ToList();
            cb_kontoEingehend.ItemsSource = konto;
            cb_kontoEingehend.SelectedIndex = 0;
        }

        public void ListComboboxSubCategoryUmbuchung()
        {
            subCategories = hbContext.Category.Include(c => c.SubCategories).First().SubCategories.ToList();
            cb_rubrikUmbuchen.ItemsSource = subCategories;
            cb_rubrikUmbuchen.SelectedIndex = 0;
        }

        //Umbuchung
        private void bt_rebooking_Click(object sender, RoutedEventArgs e)
        {
            if (checkUmbuchungIsValid())
            {
                var booking = new Booking();
                booking.Date = (DateTime) datumUmbuchung.SelectedDate;
                booking.Amount = -Math.Abs(Convert.ToDecimal(txb_betragUmbuchung.Text));
                //booking.Note = txb_notizUmbuchung.Text;
                booking.SubCategory = subCategories[cb_rubrikUmbuchen.SelectedIndex];
                booking.Konto = konto[cb_kontoAusgehend.SelectedIndex];

                hbContext.Booking.Add(booking);
                hbContext.SaveChanges();

                var bookings = new Booking();
                bookings.Date = (DateTime) datumUmbuchung.SelectedDate;
                bookings.Amount = Math.Abs(Convert.ToDecimal(txb_betragUmbuchung.Text));
                //bookings.Note = txb_notizUmbuchung.Text;
                booking.SubCategory = subCategories[cb_rubrikUmbuchen.SelectedIndex];
                bookings.Konto = konto[cb_kontoEingehend.SelectedIndex];

                hbContext.Booking.Add(bookings);
                hbContext.SaveChanges();
                //lb_bookingInfoCheck.Content = "Buchung erfolgreich";
                parent.ReadBooking();
                this.Close();
            }
        }

        private bool checkUmbuchungIsValid()
        {
            try
            {
                Convert.ToDecimal(txb_betragUmbuchung.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bitte einen gültigen Betrag eingeben!");
                return false;
            }
            if (cb_kontoAusgehend.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte Konto angeben von dem Abgehoben wird!");
                return false;
            }
            if (cb_kontoEingehend.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte Konto angeben auf das eingezahlt wird!");
                return false;
            }
            if (cb_rubrikUmbuchen.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte Rubrik angeben!");
                return false;
            }
            if (datumUmbuchung.SelectedDate == null)
            {
                MessageBox.Show("Bitte ein Datum auswählen!");
                return false;
            }

            return true;
        }


        //Button Close
        private void btn_rebookingClos_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
