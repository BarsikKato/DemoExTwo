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
    /// Логика взаимодействия для ChangeMinCount.xaml
    /// </summary>
    public partial class ChangeMinCount : Window
    {
        List<Material> selectedList;
        public ChangeMinCount(List<Material> selectedList)
        {
            this.selectedList = selectedList;
            InitializeComponent();
            countBox.Text = selectedList.OrderByDescending(x => x.MinCount).First().MinCount.ToString();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(countBox.Text) < 1)
            {
                MessageBox.Show("Количество не может быть меньше 1!");
                return;
            }
                
            foreach(Material item in selectedList)
            {
                BaseConnect.baseModel.Material.Where(x => x.ID == item.ID).FirstOrDefault().MinCount = Convert.ToInt32(countBox.Text);
            }
            try
            {
                BaseConnect.baseModel.SaveChanges();
                MessageBox.Show("Данные успешно сохранены.");
                Close();
            }
            catch
            {
                MessageBox.Show("Не удалось записать данные.");
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void countBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char number = e.Text[0];

            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }
    }
}
