using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoExTwo
{
    /// <summary>
    /// Логика взаимодействия для BasePage.xaml
    /// </summary>
    public partial class BasePage : Page
    {
        public BasePage()
        {
            InitializeComponent();
            UpdateList();
            FilterSetup();
        }

        private void UpdateList()
        {
            materialList.ItemsSource = BaseConnect.baseModel.Material.ToList();
        }

        private void FilterSetup()
        {
            filter.Items.Add("Все типы");
            foreach (var type in BaseConnect.baseModel.MaterialType.ToList())
            {
                filter.Items.Add(type.Title);
            }
            sorting.SelectedIndex = 0;
            filter.SelectedIndex = 0;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UnitedChange();
        }

        private void filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UnitedChange();
        }

        private void sorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UnitedChange();
        }

        private void UnitedChange()
        {
            List<Material> newList;
            if (filter.SelectedIndex == 0)
                newList = BaseConnect.baseModel.Material.Where(x => x.Title.Contains(search.Text)).ToList();
            else
                newList = BaseConnect.baseModel.Material.Where(x => x.Title.Contains(search.Text) && x.MaterialTypeID == filter.SelectedIndex).ToList();
            if (sorting.SelectedIndex == 1)
                newList = newList.OrderBy(x => x.Title).ToList();
            else if (sorting.SelectedIndex == 2)
                newList = newList.OrderByDescending(x => x.Title).ToList();
            else if (sorting.SelectedIndex == 3)
                newList = newList.OrderBy(x => x.CountInStock).ToList();
            else if (sorting.SelectedIndex == 4)
                newList = newList.OrderByDescending(x => x.CountInStock).ToList();
            else if (sorting.SelectedIndex == 5)
                newList = newList.OrderBy(x => x.Cost).ToList();
            else if (sorting.SelectedIndex == 6)
                newList = newList.OrderByDescending(x => x.Cost).ToList();
            materialList.ItemsSource = newList;
        }
    }
}
