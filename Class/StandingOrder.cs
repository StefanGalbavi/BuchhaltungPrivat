using System;
using System.ComponentModel.DataAnnotations;

namespace BuchhaltungPrivat.Class
{
    public class StandingOrder
    {
        [Key]
        public int StandingOrderId { get; set; }

        [MaxLength(30), MinLength(3)]
        public string? OderName { get; set; }

        public decimal Amount { get; set; }

        public string? Note { get; set; }

        public DateTime StandingOrderDateStart { get; set; }

        public DateTime StandingOrderDateEnd { get; set; }

        public string? Repetition { get; set; }


        public virtual Konto? Konto { get; set; }

        public virtual Category? Category { get; set; }

        public virtual SubCategory? SubCategory { get; set; }
    }
}
