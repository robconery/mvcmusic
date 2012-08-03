using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public string Processor { get; set; }
        public string Authorization { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}