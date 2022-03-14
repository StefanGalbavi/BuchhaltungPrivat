using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace BuchhaltungPrivat.Class
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string? Note { get; set; }




        public int? CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        public virtual SubCategory? SubCategory { get; set; }

        public virtual Konto? Konto { get; set; }

        public virtual StandingOrder? StandingOrder { get; set; }

        public Brush ValueColor => GetValueColor();


        private Brush GetValueColor()
        {
            if (Amount < 0)
                return Brushes.Red;
            else
                return Brushes.Black;


        }
    }
}
