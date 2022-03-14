using BuchhaltungPrivat.Class;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace BuchhaltungPrivat.Pages
{
    /// <summary>
    /// Interaktionslogik für Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        public readonly HBContext hbContext = new HBContext();

        public List<Category> categories { get; set; }

        public List<SubCategory> subCategories { get; set; }

        public Category Category { get; private set; }

        public Page3()
        {
            InitializeComponent();
            ReadDataCategoryEinahmen();
            ListComboboxCategorys();
        }

        public void ReadDataCategoryEinahmen()
        {
            subCategories = hbContext.SubCategory.Include(c => c.Category).OrderByDescending(c => c.Category.CategoryName).ThenBy(c => c.SubCategoryName).ToList();
            ListCategorys.ItemsSource = subCategories;
        }

        public void ListComboboxCategorys()
        {
            categories = hbContext.Category.ToList();
            cb_categoryNew.ItemsSource = categories;
            cb_categoryNew.SelectedIndex = 0;
        }

        private void bt_categoryMaking_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            SubCategory subCategory = new SubCategory()
            {
                SubCategoryName = txb_kategorieBezeichnung.Text,
                Category = categories[cb_categoryNew.SelectedIndex]
            };

            hbContext.SubCategory.Add(subCategory);
            hbContext.SaveChanges();
            ReadDataCategoryEinahmen();
        }



    }
}
