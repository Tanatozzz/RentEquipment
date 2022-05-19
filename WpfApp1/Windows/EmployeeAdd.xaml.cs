using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfApp1.Windows
{
    /// <summary>
    /// Логика взаимодействия для EmployeeAdd.xaml
    /// </summary>
    public partial class EmployeeAdd : Window
    {
        bool isEdit = false;
        EF.Employee editEmployee = new EF.Employee();
        string photostr;
        public EmployeeAdd()
        {
            InitializeComponent();
            cmbRole.ItemsSource = ClassHelper.Class1.Context.Role.ToList().Select(i => i.NameRole).ToList();
            cmbRole.SelectedIndex = 1;
            isEdit = false;
        }
        public EmployeeAdd(EF.Employee employee)
        {
            InitializeComponent();
            cmbRole.ItemsSource = ClassHelper.Class1.Context.Role.ToList();
            cmbRole.DisplayMemberPath = "NameRole";
            txtLName.Text = employee.LastName;
            txtFName.Text = employee.FirstName;
            txtMName.Text = employee.MiddleName;
            txtPhone.Text = employee.Phone;
            txtEmail.Text = employee.Email;
            txtLogin.Text = employee.Login;
            txtPassword.Text = employee.Password;
            cmbRole.SelectedIndex = employee.IDRole - 1;
            txtAddEditEmployee.Text = "Изменение данных работника";
            btnAddEmployee.Content = "Сохранить";
            if (employee.Image != null)
            {
                using (MemoryStream stream = new MemoryStream(employee.Image))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    PhotoUser.Source = bitmapImage;
                }
            }
            isEdit = true;
            editEmployee = employee;
        }
        private void txtPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "1234567890+-".IndexOf(e.Text) < 0;
        }
        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            bool IsValidEmail(string email)
            {
                string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
                Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
                return isMatch.Success;
            }
            //валидация
            if (string.IsNullOrWhiteSpace(txtLName.Text))
            {
                MessageBox.Show("Поле Фамилия не должно быть пустым", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtFName.Text))
            {
                MessageBox.Show("Поле Имя не должно быть пустым", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Поле Телефон не должно быть пустым", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Поле Login не должно быть пустым", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Поле Пароль не должно быть пустым", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var authUser = ClassHelper.Class1.Context.Employee.ToList().
            Where(i => i.Login == txtLogin.Text).FirstOrDefault();
            if (authUser != null && isEdit == false)
            {
                MessageBox.Show("Данный логин занят!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (IsValidEmail(txtEmail.Text) == false)
            {
                MessageBox.Show("Введен некорректный Email", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (txtPhone.Text.Length > 12)
            {
                MessageBox.Show("Поле Телефон содержит больше 12 символов", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Int32.TryParse(txtPhone.Text, out int res))
            {
                MessageBox.Show("Поле Телефон должно состоять только из цифр", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (isEdit) // Изменение пользователя
            {
                var resClick = MessageBox.Show("Изменить пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resClick == MessageBoxResult.No)
                {
                    return;
                }
                try
                {
                    editEmployee.LastName = txtLName.Text;
                    editEmployee.FirstName = txtFName.Text;
                    editEmployee.MiddleName = txtMName.Text;
                    editEmployee.Phone = txtPhone.Text;
                    editEmployee.Email = txtEmail.Text;
                    editEmployee.IDRole = (cmbRole.SelectedItem as EF.Role).ID;
                    editEmployee.Login = txtLogin.Text;
                    editEmployee.Password = txtPassword.Text;
                    if (photostr != null)
                    {
                        editEmployee.Image = File.ReadAllBytes(photostr);
                    }
                    ClassHelper.Class1.Context.SaveChanges();
                    MessageBox.Show("Пользователь изменен");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else    // Добавление пользователя
            {
                var resClick = MessageBox.Show("Добавить пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resClick == MessageBoxResult.No)
                {
                    return;
                }
                try
                {
                    EF.Employee newEmployee = new EF.Employee();
                    newEmployee.LastName = txtLName.Text;
                    newEmployee.FirstName = txtFName.Text;
                    newEmployee.MiddleName = txtMName.Text;
                    newEmployee.Phone = txtPhone.Text;
                    newEmployee.Email = txtEmail.Text;
                    newEmployee.IDRole = (cmbRole.SelectedItem as EF.Role).ID;
                    newEmployee.Login = txtLogin.Text;
                    newEmployee.Password = txtPassword.Text;
                    if (photostr != null)
                    {
                        newEmployee.Image = File.ReadAllBytes(photostr);
                    }
                    ClassHelper.Class1.Context.Employee.Add(newEmployee);
                    ClassHelper.Class1.Context.SaveChanges();
                    MessageBox.Show("Пользователь добавлен");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                PhotoUser.Source = new BitmapImage(new Uri(openFile.FileName));
                photostr = openFile.FileName;
            }
        }
    }
}
