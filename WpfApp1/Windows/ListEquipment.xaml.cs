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
using WpfApp1.EF;

namespace WpfApp1.Windows
{
    /// <summary>
    /// Логика взаимодействия для ListEquipment.xaml
    /// </summary>
    public partial class ListEquipment : Window
    {
        public ListEquipment()
        {
            InitializeComponent();
            Filter();
            cmbSort.ItemsSource = listSort;
            cmbSort.SelectedIndex = 0;
        }

        List<string> listSort = new List<string>()
        {
            "По умолчанию",
            "По названию",
            "По типу",
            "По цене",
            "По сроку гарантии",
            "По статусу"
        };
        private void Filter()
        {
            List<Product> listProduct = new List<Product>();

            listProduct = ClassHelper.Class1.Context.Product.ToList();

            // поиск
            listProduct = listProduct.
                Where(i => i.NameProduct.ToLower().Contains(txtSearch.Text.ToLower())
                || i.Type.ToString().ToLower().Contains(txtSearch.Text.ToLower())
                || i.Price.ToString().ToLower().Contains(txtSearch.Text.ToLower())
                || i.IDStatus.ToString().ToLower().Contains(txtSearch.Text.ToLower())
                || i.isDeleted.ToString().ToLower().Contains(txtSearch.Text.ToLower())).
                ToList();

            // сортировка

            switch (cmbSort.SelectedIndex)
            {
                case 0:
                    listProduct = listProduct.OrderBy(i => i.ID).ToList();
                    break;

                case 1:
                    listProduct = listProduct.OrderBy(i => i.NameProduct).ToList();
                    break;

                case 2:
                    listProduct = listProduct.OrderBy(i => i.IDType).ToList();
                    break;

                case 3:
                    listProduct = listProduct.OrderBy(i => i.Price).ToList();
                    break;

                case 4:
                    listProduct = listProduct.OrderBy(i => i.Warranty).ToList();
                    break;

                case 5:
                    listProduct = listProduct.OrderBy(i => i.IDStatus).ToList();
                    break;

                default:
                    listProduct = listProduct.OrderBy(i => i.ID).ToList();
                    break;
            }

            lvProduct.ItemsSource = listProduct;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void lvProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                var resClick = MessageBox.Show("Удалить товар?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                try
                {
                    if (lvProduct.SelectedItem is EF.Product)
                    {
                        var prod = lvProduct.SelectedItem as EF.Product;
                        prod.isDeleted = true;
                        ClassHelper.Class1.Context.SaveChanges();
                        MessageBox.Show("Товар успешно удален", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                        Filter();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }

            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            EquipmentAdd ProductAdd = new EquipmentAdd();
            ProductAdd.ShowDialog();
            Filter();
        }
        private void lvEmployee_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (lvProduct.SelectedItem is EF.Product)
            //{
            //    var prod = lvProduct.SelectedItem as EF.Product;

            //    EmployeeAdd addEmployeeWindow = new EmployeeAdd(prod);
            //    addEmployeeWindow.ShowDialog();
            //    Filter();

            //}
        }

        private void datePicker_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Filter();
        }
    }
}
