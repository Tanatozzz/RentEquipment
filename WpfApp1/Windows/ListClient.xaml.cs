using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1.Windows
{
    /// <summary>
    /// Логика взаимодействия для ListClient.xaml
    /// </summary>
    public partial class ListClient : Window
    {
        List<string> ListSort = new List<string>()
        {
        "По умолчанию","По фамилии","По имени","По телефону","По почте","По полу"
        };
        public ListClient()
        {
            InitializeComponent();
            Filter();
            lvClient.ItemsSource = ClassHelper.Class1.Context.Client.ToList();
            cmbSort.ItemsSource = ListSort;
            cmbSort.SelectedIndex = 0;
        }
        //добавление

        private void btnStaffAdd_Click(object sender, RoutedEventArgs e)
        {
            ClientAdd clientAddWindow = new ClientAdd();
            clientAddWindow.ShowDialog();
            lvClient.ItemsSource = ClassHelper.Class1.Context.Client.ToList();
            Filter();
        }

        private void Filter()
        {
            List<EF.Client> ListClient = new List<EF.Client>();
            ListClient = ClassHelper.Class1.Context.Client.Where(i => i.isDeleted == false).ToList();
            //Фильтрация
            ListClient = ListClient.Where(i =>
            i.LastName.ToLower().Contains(txtSearch.Text.ToLower()) ||
            i.FirstName.ToLower().Contains(txtSearch.Text.ToLower()) ||
            i.MiddleName.ToLower().Contains(txtSearch.Text.ToLower()) ||
            i.FIO.ToLower().Contains(txtSearch.Text.ToLower()) ||
            i.Phone.ToLower().Contains(txtSearch.Text.ToLower()) ||
            i.Email.ToLower().Contains(txtSearch.Text.ToLower())).ToList();

            switch (cmbSort.SelectedIndex)
            {
                case 0:
                    ListClient = ListClient.OrderBy(i => i.ID).ToList();
                    break;
                case 1:
                    ListClient = ListClient.OrderBy(i => i.LastName).ToList();
                    break;
                case 2:
                    ListClient = ListClient.OrderBy(i => i.FirstName).ToList();
                    break;
                case 3:
                    ListClient = ListClient.OrderBy(i => i.Phone).ToList();
                    break;
                case 4:
                    ListClient = ListClient.OrderBy(i => i.Email).ToList();
                    break;
                case 5:
                    ListClient = ListClient.OrderBy(i => i.IDGender).ToList();
                    break;
                default:
                    ListClient = ListClient.OrderBy(i => i.ID).ToList();
                    break;
            }
            lvClient.ItemsSource = ListClient;
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        //Удаление

        private void lvStaff_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                try
                {
                    if (lvClient.SelectedItem is EF.Employee)
                    {
                        var resmsg = MessageBox.Show("Удалить пользователя?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (resmsg == MessageBoxResult.No)
                        {
                            return;
                        }
                        var clnt = lvClient.SelectedItem as EF.Client;
                        clnt.isDeleted = true;
                        //ClassHelper.AppData.Context.Staff.Remove(stf);
                        ClassHelper.Class1.Context.SaveChanges();
                        MessageBox.Show("Пользователь успешно удален", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                        Filter();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void lvStaff_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvClient.SelectedItem is EF.Client)
            {
                var stf = lvClient.SelectedItem as EF.Client;
                ClientAdd clientAddWindow = new ClientAdd(stf);
                clientAddWindow.ShowDialog();
                Filter();
            }
        }
    }
}
