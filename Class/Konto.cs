using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace BuchhaltungPrivat.Class
{
    public class Konto
    {
        [Key]
        public int KontoId { get; set; }

        public DateTime KontoDate { get; set; }

        public string? KontoName { get; set; }

        public decimal KontoAmount { get; set; }


        //Eine Konto kann mehrere Buchungen haben
        public virtual List<Booking>? Booking { get; set; }

        public Brush ValueColor => GetValueColor();


        private Brush GetValueColor()
        {
            if (KontoAmount < 0)
                return Brushes.Red;
            else
                return Brushes.Black;


        }
    }
}
