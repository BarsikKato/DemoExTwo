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
using System.Windows.Shapes;

namespace DemoExTwo
{
    /// <summary>
    /// Логика взаимодействия для AddMaterial.xaml
    /// </summary>
    public partial class AddMaterial : Window
    {
        Material selectedMaterial;
        public AddMaterial(Material selectedMaterial)
        {
            InitializeComponent();
            this.selectedMaterial = selectedMaterial;
            matType.ItemsSource = BaseConnect.baseModel.MaterialType.ToList();
            matType.DisplayMemberPath = "Title";
            suppliers.ItemsSource = BaseConnect.baseModel.Supplier.ToList();
            suppliers.DisplayMemberPath = "Title";
            LoadMaterial();
        }

        private void LoadMaterial()
        {
            matTitle.Text = selectedMaterial.Title;
            matType.SelectedItem = selectedMaterial.MaterialType;
            countInStock.Text = selectedMaterial.CountInStock.ToString();
            unit.Text = selectedMaterial.Unit;
            countInPack.Text = selectedMaterial.CountInPack.ToString();
            minCount.Text = selectedMaterial.MinCount.ToString();
            costPerUnit.Text = (selectedMaterial.Cost/selectedMaterial.CountInPack).ToString();
            foreach(var supplier in selectedMaterial.Supplier)
            {
                suppliers.SelectedItems.Add(supplier);
            }
        }

        private void BAddOrUp_Click(object sender, RoutedEventArgs e)
        {
            var changedMaterial = BaseConnect.baseModel.Material.Find(selectedMaterial.ID);
            changedMaterial.Title = matTitle.Text;
            changedMaterial.MaterialTypeID = ((MaterialType)matType.SelectedItem).ID;
            changedMaterial.CountInPack = Convert.ToInt32(countInPack.Text);
            changedMaterial.Unit = unit.Text;
            changedMaterial.CountInStock = Convert.ToInt32(countInStock.Text);
            changedMaterial.MinCount = Convert.ToInt32(minCount.Text);
            changedMaterial.Cost = (decimal)(Convert.ToInt32(countInPack.Text) * Convert.ToDouble(costPerUnit.Text));
            changedMaterial.Description = description.Text;
            foreach(var supplier in suppliers.SelectedItems)
            {
                changedMaterial.Supplier.Add((Supplier)supplier);
            }
            try
            {
                BaseConnect.baseModel.SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                Close();
            }
            catch
            {
                MessageBox.Show("Не удалось записать данные!");
            }
        }

        private void digit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char number = e.Text[0];
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void cost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char number = e.Text[0];
            if (!Char.IsDigit(number) && number != ',' && number != '.')
            {
                e.Handled = true;
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
