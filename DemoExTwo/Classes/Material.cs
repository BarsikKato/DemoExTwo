using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DemoExTwo
{
    public partial class Material
    {
        public string MinMaterial
        {
            get
            {
                return "Минимальное количество: " + MinCount + " шт.";
            }
        }

        public string Suppliers
        {
            get
            {
                var suppliers = Supplier.ToList();
                string suppString = "Поставщик: ";
                foreach(var supplier in suppliers)
                {
                    suppString += supplier.Title + ", ";
                }
                return suppString;
            }
        }

        public string CountLeft
        {
            get
            {
                return "Осталось: " + CountInStock + " шт.";
            }
        }

        public BitmapImage materialImage
        {
            get
            {
                if (Image != "")
                    return new BitmapImage(new Uri(Image, UriKind.Relative));
                else
                    return new BitmapImage(new Uri("\\materials\\material_null.png", UriKind.Relative));
            }
        }
    }
}
