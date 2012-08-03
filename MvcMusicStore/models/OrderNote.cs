using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Models
{
    public class OrderNote
    {
        public OrderNote()
        {
            this.CreatedOn = DateTime.Now;
        }
        
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}