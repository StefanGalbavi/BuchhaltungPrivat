using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using static BuchhaltungPrivat.Class.Database.ReportingUtils;

namespace BuchhaltungPrivat.Class.Database
{
    public class ReportingUtils
    {
        public class Report
        {
            public string SubCategoryName { get; set; }

            public decimal Amount { get; set; }

            public Brush ValueColor => GetValueColor();

            private Brush GetValueColor()
            {
                if (Amount < 0)
                    return Brushes.Red;
                else
                    return Brushes.Black;


            }

        }


        public static List<Report> getReport(HBContext hbContext, string categoryName, DateTime selectedDate)
        {
            return hbContext.SubCategory.
               Join(
               hbContext.Category,
               subCategory => subCategory.CategoryId,
               category => category.CategoryId,
               (subCategory, category) => new { categoryName = category.CategoryName, subCategoryName = subCategory.SubCategoryName, subCategoryId = subCategory.SubCategoryId })
               .Where(data => data.categoryName == categoryName)
               .Join(
               hbContext.Booking.Where(data => data.Date.Month == selectedDate.Month && data.Date.Year == selectedDate.Year),
               data => data.subCategoryId,
               booking => booking.SubCategory.SubCategoryId,
               (data, booking) => new { subCategoryId = data.subCategoryId, subCategoryName = data.subCategoryName, bookingAmount = booking.Amount, bookingDate = booking.Date }

               )
               .GroupBy(data => data.subCategoryName)
               .Select(data => new Report() { SubCategoryName = data.Key, Amount = data.Sum(tmp => tmp.bookingAmount) })
               .ToList();

            //var listB = hbContext.Booking.Where(x => x.Date.Month == selectedDate.Month && x.Date.Year == selectedDate.Year).Include(b => b.Konto)?.Include(b => b.SubCategory).OrderBy(x => x.Date);
            //listB.GroupBy(d => d.SubCategory).Select(d => new Report() { SubCategoryName = d.Key, Amount = d.Sum(c => c.bookingAmount) });

        }

    }
}
