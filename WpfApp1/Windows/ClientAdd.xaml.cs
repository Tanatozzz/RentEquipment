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
    /// Логика взаимодействия для ClientAdd.xaml
    /// </summary>
    public partial class ClientAdd : Window
    {
        bool isEdit = false;
        EF.Client editClient = new EF.Client();
        string photostr;
        public ClientAdd()
        {
            InitializeComponent();
            cmbGender.ItemsSource = ClassHelper.Class1.Context.Gender.ToList().Select(i => i.NameGender).ToList();
            cmbGender.SelectedIndex = 1;
            isEdit = false;
        }
        public ClientAdd(EF.Client client)
        {
            InitializeComponent();
            cmbGender.ItemsSource = ClassHelper.Class1.Context.Gender.ToList();
            cmbGender.DisplayMemberPath = "NameGender";
            txtLName.Text = client.LastName;
            txtFName.Text = client.FirstName;
            txtMName.Text = client.MiddleName;
            txtPhone.Text = client.Phone;
            txtEmail.Text = client.Email;
            cmbGender.SelectedIndex = client.IDGender - 1;
            txtAddEditEmployee.Text = "Изменение данных работника";
            btnAddEmployee.Content = "Сохранить";
            if (client.Photo != null)
            {
                using (MemoryStream stream = new MemoryStream(client.Photo))
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
            editClient = client;
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
                    editClient.LastName = txtLName.Text;
                    editClient.FirstName = txtFName.Text;
                    editClient.MiddleName = txtMName.Text;
                    editClient.Phone = txtPhone.Text;
                    editClient.Email = txtEmail.Text;
                    editClient.IDGender = (cmbGender.SelectedItem as EF.Gender).ID;

                    if (photostr != null)
                    {
                        editClient.Photo = File.ReadAllBytes(photostr);
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
                    string series = txtPassport.Text.Substring(0, 4);
                    string number = txtPassport.Text.Substring(4, 6);
                    DateTime dtbth = (DateTime)DateBirthdayPicker.SelectedDate;
                    if (txtPassport.Text.Length != 10)
                    {
                        MessageBox.Show("Поле Паспорт содержит больше 10 символов", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    EF.Client newClient = new EF.Client();
                    EF.Passport newPasport = new EF.Passport();
                    newClient.LastName = txtLName.Text;
                    newClient.FirstName = txtFName.Text;
                    newClient.MiddleName = txtMName.Text;
                    newClient.Phone = txtPhone.Text;
                    newClient.Email = txtEmail.Text;
                    newClient.DateOfBirth = dtbth;
                    newClient.IDGender = (cmbGender.SelectedItem as EF.Gender).ID;
                    newPasport.PassportSeries = series;
                    newPasport.PassportNumber = number;
                    if (photostr != null)
                    {
                        newClient.Photo = File.ReadAllBytes(photostr);
                    }
                    ClassHelper.Class1.Context.Client.Add(newClient);
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
