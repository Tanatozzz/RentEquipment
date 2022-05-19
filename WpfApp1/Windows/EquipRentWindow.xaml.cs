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

namespace WpfApp1.Windows
{
    /// <summary>
    /// Логика взаимодействия для EquipRentWindow.xaml
    /// </summary>
    public partial class EquipRentWindow : Window
    {
        //Данные для заполнения комбобоксов

        List<string> ListStaffSort = new List<string>()
    {
        "По фамилии","По должности"
    };
        List<string> ListClientSort = new List<string>()
    {
        "По фамилии","По паспорту"
    };
        List<string> ListEquip = new List<string>()
    {
        "По типу","По названию"
    };

        private int IDstf = -1;
        private int IDclt = -1;
        private int IDeqp = -1;

        public EquipRentWindow()
        {
            InitializeComponent();
            ClientFilter();
            StaffFilter();
            EquipFilter();
            lvStaff.ItemsSource = ClassHelper.Class1.Context.Employee.ToList();
            lvClient.ItemsSource = ClassHelper.Class1.Context.Client.ToList();
            lvEquip.ItemsSource = ClassHelper.Class1.Context.Product.ToList();
            cmbClientSort.ItemsSource = ListClientSort;
            cmbEquipSort.ItemsSource = ListEquip;
            cmbStaffSort.ItemsSource = ListStaffSort;
            cmbClientSort.SelectedIndex = 0;
            cmbEquipSort.SelectedIndex = 0;
            cmbStaffSort.SelectedIndex = 0;
        }
        private void StaffFilter()
        {
            List<EF.Employee> ListStaff = new List<EF.Employee>();
            ListStaff = ClassHelper.Class1.Context.Employee.Where(i => i.isDeleted == false).ToList();
            ListStaff = ListStaff.Where(i =>
            i.LastName.ToLower().Contains(txtStaffSearch.Text.ToLower())).ToList();
            switch (cmbStaffSort.SelectedIndex)
            {
                case 0:
                    ListStaff = ListStaff.OrderBy(i => i.LastName).ToList();
                    break;
                case 1:
                    ListStaff = ListStaff.OrderBy(i => i.IDRole).ToList();
                    break;

                default:
                    ListStaff = ListStaff.OrderBy(i => i.ID).ToList();
                    break;
            }
            lvStaff.ItemsSource = ListStaff;
        }
        private void ClientFilter()
        {
            List<EF.Client> ListClient = new List<EF.Client>();
            ListClient = ClassHelper.Class1.Context.Client.Where(i => i.isDeleted == false).ToList();
            ListClient = ListClient.Where(i =>
            i.LastName.ToLower().Contains(txtClientSearch.Text.ToLower()) ||
            i.Passport.PassportNumber.Contains(txtClientSearch.Text)).ToList();
            switch (cmbClientSort.SelectedIndex)
            {
                case 0:
                    ListClient = ListClient.OrderBy(i => i.LastName).ToList();
                    break;
                case 1:
                    ListClient = ListClient.OrderBy(i => i.Passport.PassportNumber).ToList();
                    break;

                default:
                    ListClient = ListClient.OrderBy(i => i.ID).ToList();
                    break;
            }
            lvClient.ItemsSource = ListClient;
        }
        private void EquipFilter()
        {
            List<EF.Product> ListEquip = new List<EF.Product>();
            ListEquip = ClassHelper.Class1.Context.Product.Where(i => i.isDeleted == false && i.IDStatus == 1).ToList();
            ListEquip = ListEquip.Where(i =>
            i.NameProduct.ToLower().Contains(txtEquipSearch.Text.ToLower()) ||
            i.Type.NameType.ToLower().Contains(txtEquipSearch.Text.ToLower())).ToList();
            switch (cmbEquipSort.SelectedIndex)
            {
                case 1:
                    ListEquip = ListEquip.OrderBy(i => i.NameProduct).ToList();
                    break;
                case 0:
                    ListEquip = ListEquip.OrderBy(i => i.Type.NameType).ToList();
                    break;

                default:
                    ListEquip = ListEquip.OrderBy(i => i.ID).ToList();
                    break;
            }
            lvEquip.ItemsSource = ListEquip;
        }



        private void txtStaffSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            StaffFilter();
        }

        private void txtClientSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ClientFilter();
        }

        private void txtEquipSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            EquipFilter();
        }

        private void cmbStaffSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StaffFilter();
        }

        private void cmbClientSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientFilter();
        }

        private void cmbEquipSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EquipFilter();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lvStaff_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var stf = lvStaff.SelectedItem as EF.Employee;
            txtStaff.Text = stf.LastName;
            IDstf = stf.ID;
        }

        private void lvClient_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var clt = lvClient.SelectedItem as EF.Client;
            txtClient.Text = clt.LastName;
            IDclt = clt.ID;
        }

        private void lvEquip_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var eqp = lvEquip.SelectedItem as EF.Product;
            txtEquip.Text = eqp.NameProduct;
            IDeqp = eqp.ID;
            TotalPriceMethod();
        }
        private void btnAddExt_Click(object sender, RoutedEventArgs e)
        {

            if (IDstf == -1)
            {
                MessageBox.Show("Ошибка! Сотрудник не выбран", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (IDclt == -1)
            {
                MessageBox.Show("Ошибка! Клиент не выбран", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (IDeqp == -1)
            {
                MessageBox.Show("Ошибка! Оборудование не выбрано", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpStartUse.SelectedDate == null)
            {
                MessageBox.Show("Ошибка! Дата выдачи не выбрана!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpStartUse.SelectedDate < DateTime.Today)
            {
                MessageBox.Show("Ошибка! Дата выдачи меньше текущего дня!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpEndUse.SelectedDate == null)
            {
                MessageBox.Show("Ошибка! Дата возврата не выбрана!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpEndUse.SelectedDate < DateTime.Today)
            {
                MessageBox.Show("Ошибка! Дата сдачи меньше текущего дня!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dpStartUse.SelectedDate > dpEndUse.SelectedDate)
            {
                MessageBox.Show("Ошибка! Дата возврата не может быть раньше даты выдачи!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var resClick = MessageBox.Show("Вы уверены?", "Подтвержение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resClick == MessageBoxResult.No)
            {
                return;
            }
            try
            {
                EF.ClientProduct clientProduct = new EF.ClientProduct();
                clientProduct.IDProduct = IDeqp;
                clientProduct.IDClient = IDclt;
                clientProduct.IDEmployee = IDstf;
                clientProduct.DateOfIssue = DateTime.Now;
                clientProduct.DateStart = Convert.ToDateTime(dpStartUse.SelectedDate) + DateTime.Now.TimeOfDay;
                clientProduct.DateEnd = Convert.ToDateTime(dpEndUse.SelectedDate) + DateTime.Now.TimeOfDay;
                clientProduct.TotalPrice = Convert.ToDecimal(txtTotalPrice.Text);
                ClassHelper.Class1.Context.ClientProduct.Add(clientProduct);
                ClassHelper.Class1.Context.SaveChanges();
                MessageBox.Show("Запись добавлена");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public void TotalPriceMethod()
        {
            if (dpStartUse.SelectedDate != null && dpEndUse.SelectedDate != null && txtEquip.Text != null)
            {

                if (dpStartUse.SelectedDate < DateTime.Today && dpEndUse.SelectedDate < DateTime.Today)
                {
                    MessageBox.Show("Не правильно введена дата начала или конца", "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    var prd = lvEquip.SelectedItem as EF.Product;
                    double b = Convert.ToDouble(prd.Price);
                    DateTime dateStart = DateTime.Parse(Convert.ToString(dpStartUse.SelectedDate));
                    DateTime dateEnd = DateTime.Parse(Convert.ToString(dpEndUse.SelectedDate));
                    string daySubtrut = Convert.ToString(dateEnd.Subtract(dateStart));
                    string[] daySplit = daySubtrut.Split(new char[] { '.' });
                    double totalPrice = Convert.ToDouble(daySplit[0]) * ((b * 5) / 100);
                    txtTotalPrice.Text = totalPrice.ToString();
                }

            }
        }

        private void dpStartUse_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TotalPriceMethod();
        }

        private void dpEndUse_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TotalPriceMethod();
        }
    }
}