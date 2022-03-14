using BuchhaltungPrivat.Class;
using BuchhaltungPrivat.PagesPopup;
using Microsoft.EntityFrameworkCore;
using MigraDoc.DocumentObjectModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace BuchhaltungPrivat.Pages
{
    /// <summary>
    /// Interaktionslogik für Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        HBContext hbContext = new HBContext();

        public List<Booking> bookings { get; set; } = new List<Booking>();

        public List<Category> categories = new List<Category>();

        public List<SubCategory> subCategories = new List<SubCategory>();

        public List<Konto> konto = new List<Konto>();

        public DateTime selectedDate { get; set; }

        public Page1()
        {
            InitializeComponent();
            selectedDate = DateTime.Now;
            datePicker.Text = DateTime.Now.Date.ToString();
            ListComboboxKonto();
            ListComboboxSubCategoryEinnahme();
            ListComboboxCategorys();
            ListComboboxBookingKontoChange();
            ReadBooking();
        }

        //ComboBox Select Alle Konten
        public void ListComboboxBookingKontoChange()
        {

            konto = hbContext.Konto.ToList();
            Konto k = new Konto();
            k.KontoName = "Alle Buchungen";
            konto.Add(k);

            cb_shwoKontoHeader.SelectedIndex = konto.Count - 1;

            cb_shwoKontoHeader.ItemsSource = konto;
        }


        private void ComboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            ComboBox comboBox = (ComboBox) sender;

            Konto selectedKonto = (Konto) cb_shwoKontoHeader.SelectedItem;

            int count = 0;
            int resultIndex = -1;
            if (selectedKonto.KontoId != 0)
            {
                bookings = hbContext.Booking.Include(b => b.Konto).Include(b => b.SubCategory).OrderBy(x => x.Date).Where(x => x.Konto != null && x.Konto.KontoId == selectedKonto.KontoId).Where(x => x.Date.Month == selectedDate.Month && x.Date.Year == selectedDate.Year).ToList();
                ListReportAusgaben.ItemsSource = bookings;

                decimal sum = 0;
                foreach (Booking booking in bookings)
                {
                    sum += booking.Amount;
                }

                txb_summeMonat.Text = sum.ToString();
            }
            else
            {
                ReadBooking();
            }
        }

        //Listet alle Buchungen auf und Rechnet die Summe aus
        public void ReadBooking()
        {
            if (hbContext == null || selectedDate == null)
            {
                return;
            }

            bookings = hbContext.Booking.Where(x => x.Date.Month == selectedDate.Month && x.Date.Year == selectedDate.Year)?.Include(b => b.Konto)?.Include(b => b.SubCategory)?.OrderBy(x => x.Date).ToList();
            decimal sum = 0;
            //calculate sum
            foreach (Booking booking in bookings)
            {
                sum += booking.Amount;
            }

            txb_summeMonat.Text = sum.ToString();


            ListReportAusgaben.ItemsSource = bookings;


            ReadLastMonthTransfer();

            SumRestMoney();
        }

        private void SumRestMoney()
        {
            bookings = hbContext.Booking.Include(b => b.Konto).Include(b => b.SubCategory).OrderBy(x => x.Date).Where(x => x.Date.Month == selectedDate.Month && x.Date.Year == selectedDate.Year).ToList();
            decimal sum1 = 0;
            //calculate sum
            foreach (Booking booking in bookings)
            {
                sum1 += booking.Amount;
            }

            var firstOfThisMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
            var sum2 = hbContext.Booking.Include(b => b.Konto).Include(b => b.SubCategory).Where(x => x.Date < firstOfThisMonth).Sum(b => b.Amount);
            var summeRest = sum1 + sum2;
            txb_summeMonat2.Text = summeRest.ToString();
        }

        private void ReadLastMonthTransfer()
        {
            var firstOfThisMonth = new DateTime(selectedDate.Year, selectedDate.Month, 1);
            var sum = hbContext.Booking.Include(b => b.Konto).Include(b => b.SubCategory).Where(x => x.Date < firstOfThisMonth).Sum(b => b.Amount);
            lbl_uebertragVormonat.Content = sum;
        }



        public void ListComboboxCategorys()
        {
            categories = hbContext.Category.ToList();
            cb_kategorie.ItemsSource = categories;
            cb_kategorie.SelectedIndex = 0;
        }

        public void ListComboboxKonto()
        {
            konto = hbContext.Konto.ToList();
            cb_konten.ItemsSource = konto;
            cb_konten.SelectedIndex = 0;
        }

        public void ListComboboxSubCategoryEinnahme()
        {
            subCategories = hbContext.Category.Include(c => c.SubCategories).Where(c => c.CategoryName == "Einnahme").First().SubCategories.ToList();
            cb_rubrik.ItemsSource = subCategories;
            cb_rubrik.SelectedIndex = 0;
        }


        //Kalender Header
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

                ReadBooking();
            }
        }


        //Buchungen erstellen Einnahmen/Ausgaben
        private void bt_buchungErstellen_Click(object sender, RoutedEventArgs e)
        {
            if (checkEinnahmeIsValid())
            {
                if (cb_kategorie.SelectedIndex == 0)
                {
                    var booking = new Booking();
                    booking.Amount = Math.Abs(Convert.ToDecimal(buchungEinnahmen.Text));
                    booking.Note = notizEinnahmen.Text;
                    booking.Date = (DateTime) datumEinnahme.SelectedDate;
                    booking.SubCategory = subCategories[cb_rubrik.SelectedIndex];
                    booking.Konto = konto[cb_konten.SelectedIndex];
                    booking.Category = categories[cb_kategorie.SelectedIndex];
                    hbContext.Booking.Add(booking);
                    hbContext.SaveChanges();
                    MessageBox.Show("Buchung erfolgreich!");
                    ReadBooking();
                }
                else if (cb_kategorie.SelectedIndex == 1)
                {
                    var booking = new Booking();
                    booking.Amount = -Math.Abs(Convert.ToDecimal(buchungEinnahmen.Text));
                    booking.Note = notizEinnahmen.Text;
                    booking.Date = (DateTime) datumEinnahme.SelectedDate;
                    booking.SubCategory = subCategories[cb_rubrik.SelectedIndex];
                    booking.Konto = konto[cb_konten.SelectedIndex];
                    booking.Category = categories[cb_kategorie.SelectedIndex];
                    hbContext.Booking.Add(booking);
                    hbContext.SaveChanges();
                    MessageBox.Show("Buchung erfolgreich!");
                    ReadBooking();
                }
            }
        }


        //Abfrage korrekte Eingaben Einnahme
        private bool checkEinnahmeIsValid()
        {
            try
            {
                Convert.ToDecimal(buchungEinnahmen.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bitte einen gültigen Betrag eingeben!");
                return false;
            }
            if (cb_rubrik.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte eine Kategorie auswählen!");
                return false;
            }
            if (datumEinnahme.SelectedDate == null)
            {
                MessageBox.Show("Bitte ein Datum auswählen!");
                return false;
            }
            if (cb_konten.SelectedIndex < 0)
            {
                MessageBox.Show("Bitte eine Konto auswählen!");
                return false;
            }

            return true;
        }


        //Umbuchung Button
        private void btn_newUmbuchung_Click(object sender, RoutedEventArgs e)
        {
            Rebooking rebooking = new Rebooking(this);
            rebooking.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            rebooking.Show();
        }


        //PDF generieren und Speichern
        private void btn_pdfExport_Click(object sender, RoutedEventArgs e)
        {
            CreatePdfBooking();
        }

        private void CreatePdfBooking()
        {
            MigraDoc.DocumentObjectModel.Document doc = new MigraDoc.DocumentObjectModel.Document();
            MigraDoc.DocumentObjectModel.Section sec = doc.AddSection();
            sec.PageSetup.PageFormat = MigraDoc.DocumentObjectModel.PageFormat.A4;
            sec.PageSetup.LeftMargin = 50;
            sec.PageSetup.TopMargin = 40;
            sec.PageSetup.RightMargin = 40;
            sec.PageSetup.BottomMargin = 40;

            //Seitenzahle Automatisch gnerieren
            Paragraph paragraph1 = new Paragraph();
            paragraph1.AddText("Page ");
            paragraph1.AddPageField();
            paragraph1.AddText(" of ");
            paragraph1.AddNumPagesField();
            sec.Footers.Primary.Add(paragraph1);

            //sec.AddParagraph("List of Bookings " + selectedDate.ToString("MMMM.yyyy"));
            //sec.AddParagraph();
            Paragraph paragraph = sec.AddParagraph();
            paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);
            paragraph.Format.Font.Size = 18;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddFormattedText("Buchungen vom " + selectedDate.ToString("MMMM yyyy"));
            sec.AddParagraph();
            sec.AddParagraph();

            //Table
            MigraDoc.DocumentObjectModel.Tables.Table table = new MigraDoc.DocumentObjectModel.Tables.Table();
            table.Borders.Width = 0.2;
            table.BottomPadding = 5;
            table.TopPadding = 5;
            table.LeftPadding = 3;

            //Column
            MigraDoc.DocumentObjectModel.Tables.Column column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Left;
            column.Format.Font.Size = 12;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Left;
            column.Format.Font.Size = 12;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(4));
            column.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Left;
            column.Format.Font.Size = 12;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Right;
            column.Format.Font.Size = 12;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(4));
            column.Format.Alignment = MigraDoc.DocumentObjectModel.ParagraphAlignment.Left;
            column.Format.Font.Size = 12;

            //Header of Table
            MigraDoc.DocumentObjectModel.Tables.Row row = table.AddRow();
            MigraDoc.DocumentObjectModel.Tables.Cell cell = row.Cells[0];
            cell.AddParagraph("Datum");
            cell.Format.Font.Bold = true;

            cell = row.Cells[1];
            cell.AddParagraph("Konto");
            cell.Format.Font.Bold = true;

            cell = row.Cells[2];
            cell.AddParagraph("Kategorie");
            cell.Format.Font.Bold = true;

            cell = row.Cells[3];
            cell.AddParagraph("Betrag");
            cell.Format.Font.Bold = true;

            cell = row.Cells[4];
            cell.AddParagraph("Notiz");
            cell.Format.Font.Bold = true;


            //def Rows of Table
            foreach (Booking elem in bookings)
            {
                row = table.AddRow();

                cell = row.Cells[0];
                cell.AddParagraph(elem.Date.ToString("dd.MM.yyyy"));

                cell = row.Cells[1];
                cell.AddParagraph(elem.Konto.KontoName);


                cell = row.Cells[2];
                if (elem.SubCategory != null)
                {
                    cell.AddParagraph(elem.SubCategory.SubCategoryName);
                }

                cell = row.Cells[3];
                if (elem.Amount < 0)
                {
                    cell.Format.Font.Color = MigraDoc.DocumentObjectModel.Color.Parse("Red");
                }
                cell.AddParagraph(elem.Amount.ToString());


                cell = row.Cells[4];
                if (elem.Note != null)
                {
                    cell.AddParagraph(elem.Note);
                }


                //Jede zweite Zeile einfärben
                for (var i = 1; i < table.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        table.Rows[i].Shading.Color = MigraDoc.DocumentObjectModel.Color.FromRgb(216, 216, 216);
                    }
                }


            }

            doc.LastSection.Add(table);


            //rendering doc
            MigraDoc.Rendering.PdfDocumentRenderer docRend = new MigraDoc.Rendering.PdfDocumentRenderer(false);
            docRend.Document = doc;
            docRend.RenderDocument();

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".pdf"; // Default file extension
            dlg.Filter = "PDF documents (.pdf)|*.pdf"; // Filter files by extensio

            string fileName = "";

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results 
            if (result == true)
            {
                // Save document 
                fileName = dlg.FileName;
                docRend.PdfDocument.Save(fileName);
            }

            if (File.Exists(fileName))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(fileName)
                {
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
        }

        private void cb_kategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void btn_csvImport_Click(object sender, RoutedEventArgs e)
        {

        }
    }


}
