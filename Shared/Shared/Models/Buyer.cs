using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Buyer
    {
        public int Id { get; set; }
        public string TaxId { get; set; } 
        public string Name { get; set; } 
        public string ContactEmail { get; set; } 
        public string Address { get; set; } // Adres
        public ICollection<Supplier> Suppliers { get; set; } 

        public Buyer()
        {
            Suppliers = new List<Supplier>();
        }
    }
}
