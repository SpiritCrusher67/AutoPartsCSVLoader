using CsvHelper.Configuration;
using System.Text.RegularExpressions;

namespace AutoPartsCSVLoader.Data
{
    public class PriceItemMap : ClassMap<PriceItem>
    {
        public PriceItemMap(IConfigurationSection columnsConfiguration)
        {
            Map(m => m.Vendor).Name(columnsConfiguration["Vendor"]);
            Map(m => m.SearchVendor).Name(columnsConfiguration["Vendor"]);
            Map(m => m.Number).Name(columnsConfiguration["Number"]);
            Map(m => m.SearchNumber).Name(columnsConfiguration["Number"]);
            Map(m => m.Description).Name(columnsConfiguration["Description"]);
            Map(m => m.Price).Name(columnsConfiguration["Price"]);
            Map(m => m.Count).Convert(str => 
                ConvertCountString(str.Row.GetField(columnsConfiguration["Count"])));
        }

        private int ConvertCountString(string str)
        {
            var match = Regex.Match(str, "[^0-9]");
            if (match.Success)
                return int.Parse(str.Substring(match.Index + 1));
            return int.Parse(str);
        }
    }
}
