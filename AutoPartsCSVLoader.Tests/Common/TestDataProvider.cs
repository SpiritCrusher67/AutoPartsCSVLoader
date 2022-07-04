using AutoPartsCSVLoader.Data;
using System.Collections.Generic;

namespace AutoPartsCSVLoader.Tests.Common
{
    public static class TestDataProvider
    {
        public static IList<PriceItem> TestPriceItems { get; private set; } = new List<PriceItem> 
        { 
            new PriceItem()
            {
                Vendor = "Автоупор",
                SearchVendor = "АВТОУПОР",
                Description = "Амортизаторы капота Volkswagen Polo V - все, 2010-..., крепеж в комплекте, сталь (new)",
                Number = "UVWPOL012", 
                SearchNumber = "UVWPOL012",
                Count = 3,
                Price = 1792.25m
            },
            new PriceItem()
            {
                Vendor = "АСОМИ",
                SearchVendor = "АСОМИ",
                Description = "Корпус стойки передней подвески авт. ВАЗ 2170 левый в сборе под картридж",
                Number = "А170.2905.581-Р",
                SearchNumber = "А1702905581Р",
                Count = 1,
                Price = 545.63m
            },
            new PriceItem()
            {
                Vendor = "LYNXauto",
                SearchVendor = "LYNXAUTO",
                Description = "Ступица колеса | зад лев |",
                Number = "WH-1503",
                SearchNumber = "WH1503",
                Count = 1,
                Price = 3808m
            },
            new PriceItem()
            {
                Vendor = "LYNXauto",
                SearchVendor = "LYNXAUTO",
                Description = "Щетка стеклоочистителя бескаркасная 330 мм\"2\"",
                Number = "XF330",
                SearchNumber = "XF330",
                Count = 1,
                Price = 218m
            }
        };
    }
}
