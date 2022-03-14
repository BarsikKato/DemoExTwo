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
    public partial class ChangeMaterial : Window
    {
        Material selectedMaterial;
        public ChangeMaterial(Material selectedMaterial)
        {
            InitializeComponent();
            this.selectedMaterial = selectedMaterial;
            matType.ItemsSource = BaseConnect.baseModel.MaterialType.ToList();
            matType.DisplayMemberPath = "Title";
            suppliers.ItemsSource = BaseConnect.baseModel.Supplier.ToList();
            suppliers.DisplayMemberPath = "Title";
            addedSuppliers.DisplayMemberPath = "Title";
            LoadMaterial();
        }

        private void LoadMaterial()
        {
            imagePath.Text = selectedMaterial.Image;
            matImage.Source = new BitmapImage(new Uri(imagePath.Text, UriKind.Relative));
            matTitle.Text = selectedMaterial.Title;
            matType.SelectedItem = selectedMaterial.MaterialType;
            countInStock.Text = selectedMaterial.CountInStock.ToString();
            unit.Text = selectedMaterial.Unit;
            countInPack.Text = selectedMaterial.CountInPack.ToString();
            minCount.Text = selectedMaterial.MinCount.ToString();
            costPerUnit.Text = (selectedMaterial.Cost/selectedMaterial.CountInPack).ToString();
            foreach(var supplier in selectedMaterial.Supplier)
            {
                addedSuppliers.Items.Add(supplier);
            }
        }

        private void change_Click(object sender, RoutedEventArgs e)
        {
            if (!IsEmpty(matTitle.Text) || !IsEmpty(countInPack.Text) || !IsEmpty(countInStock.Text) || !IsEmpty(unit.Text) || !IsEmpty(minCount.Text) || !IsEmpty(costPerUnit.Text))
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return;
            }
            var changedMaterial = BaseConnect.baseModel.Material.Find(selectedMaterial.ID);
            changedMaterial.Image = imagePath.Text;
            changedMaterial.Title = matTitle.Text;
            changedMaterial.MaterialTypeID = ((MaterialType)matType.SelectedItem).ID;
            changedMaterial.CountInPack = Convert.ToInt32(countInPack.Text);
            changedMaterial.Unit = unit.Text;
            changedMaterial.CountInStock = Convert.ToInt32(countInStock.Text);
            changedMaterial.MinCount = Convert.ToInt32(minCount.Text);
            changedMaterial.Cost = (decimal)(Convert.ToInt32(countInPack.Text) * Convert.ToDouble(costPerUnit.Text));
            changedMaterial.Description = description.Text;
            foreach(Supplier supplier in addedSuppliers.Items)
            {
                if(changedMaterial.Supplier.Where(x => x.ID == supplier.ID).ToList().Count == 0)
                    changedMaterial.Supplier.Add(supplier);
            }
            foreach (Supplier supplier in changedMaterial.Supplier)
            {
                if(!addedSuppliers.Items.Contains(supplier))
                    changedMaterial.Supplier.Remove(supplier);
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

        private bool IsEmpty(string text)
        {
            if (text.Length == 0)
                return false;
            else
                return true;
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

        private void addSupplier_Click(object sender, RoutedEventArgs e)
        {
            if (suppliers.SelectedIndex >= 0)
                if (!addedSuppliers.Items.Contains(suppliers.SelectedItem))
                    addedSuppliers.Items.Add(suppliers.SelectedItem);
                else
                    MessageBox.Show("Данный поставщик уже добавлен.");
            else
                MessageBox.Show("Выберите поставщика.");
        }

        private void addedSuppliers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            addedSuppliers.Items.Remove(addedSuppliers.SelectedItem);
        }

        private void imagePath_TextChanged(object sender, TextChangedEventArgs e)
        {
            matImage.Source = new BitmapImage(new Uri(imagePath.Text, UriKind.Relative));
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rsltMessageBox = MessageBox.Show("Вы уверены, что хотите удалить данный материал?", "Удаление материала", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(rsltMessageBox == MessageBoxResult.Yes)
                try
                {
                    var material = BaseConnect.baseModel.Material.Find(selectedMaterial.ID);
                    BaseConnect.baseModel.Material.Remove(material);
                    BaseConnect.baseModel.SaveChanges();
                    MessageBox.Show("Материал был успешно удален!");
                    Close();
                }
                catch
                {
                    MessageBox.Show("Не удалось удалить материал!");
                }
        }
    }
}
