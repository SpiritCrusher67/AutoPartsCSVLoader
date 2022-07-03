using System.Text.RegularExpressions;

namespace AutoPartsCSVLoader.Data
{
    public class PriceItem
    {
        private string searchVendor = null!;
        private string searchNumber = null!;
        private string description = null!;

        public int Id { get; set; }
        public string Vendor { get; set; } = null!;
        public string Number { get; set; } = null!;
        public string SearchVendor 
        { 
            get => searchVendor; 
            set => searchVendor = Regex.Replace(value.ToUpper(), "[^А-ЯA-Z0-9]", string.Empty); 
        }
        public string SearchNumber 
        { 
            get => searchNumber;
            set => searchNumber = Regex.Replace(value.ToUpper(), "[^А-ЯA-Z0-9]", string.Empty); 
        }
        public string Description 
        { 
            get => description;
            set => description = value.Length > 512 ? value.Substring(0, 512) : value; 
        }
        public decimal Cost { get; set; }
        public int Count { get; set; }
    }
}
