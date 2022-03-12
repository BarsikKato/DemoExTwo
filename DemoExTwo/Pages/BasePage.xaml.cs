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
        PageNavigation pageNavigation = new PageNavigation();
        public BasePage()
        {
            InitializeComponent();
            UpdateList();
            FilterSetup();
            DataContext = pageNavigation;
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
            materialsCount.Text = newList.Count + " из " + BaseConnect.baseModel.Material.ToList().Count;
            pageNavigation.CountPage = 15;
            pageNavigation.Countlist = newList.Count;
            materialList.ItemsSource = newList.Skip(pageNavigation.CurrentPage * pageNavigation.CountPage - pageNavigation.CountPage).Take(pageNavigation.CountPage).ToList();
        }

        private void page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            switch (tb.Uid)
            {
                case "prev":
                    pageNavigation.CurrentPage--;
                    break;
                case "next":
                    pageNavigation.CurrentPage++;
                    break;
                default:
                    pageNavigation.CurrentPage = Convert.ToInt32(tb.Text);
                    break;
            }
            UnitedChange();
            //txtCurrentPage.Text = "Текущая страница: " + (pageNavigation.CurrentPage).ToString();
        }

        private void materialList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (materialList.SelectedItems.Count > 1)
                changeMinCount.Visibility = Visibility.Visible;
            else
                changeMinCount.Visibility = Visibility.Hidden;
        }

        private void changeMinCount_Click(object sender, RoutedEventArgs e)
        {
            List<Material> selectedMaterials = new List<Material>();
            foreach(Material item in materialList.SelectedItems)
            {
                selectedMaterials.Add(item);
            }
            ChangeMinCount changeMinCount = new ChangeMinCount(selectedMaterials);
            changeMinCount.ShowDialog();
            UnitedChange();
        }

        private void materialList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedMaterial = (Material)materialList.SelectedItem;
            ChangeMaterial addMaterial = new ChangeMaterial(selectedMaterial);
            addMaterial.ShowDialog();
            UnitedChange();
        }
    }
}
