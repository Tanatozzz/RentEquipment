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
    /// Логика взаимодействия для ListClient.xaml
    /// </summary>
    public partial class ListClient : Window
    {
        public ListClient()
        {
            InitializeComponent();
            lvClient.ItemsSource = ClassHelper.Class1.Context.Client.ToList();
        //    < GridViewColumn Header = "Код" Width = "40" DisplayMemberBinding = "{Binding ID}" ></ GridViewColumn >
     
        //                 < GridViewColumn Header = "Фамилия" Width = "100" DisplayMemberBinding = "{Binding LastName}" ></ GridViewColumn >
          
        //                      < GridViewColumn Header = "Имя" Width = "100" DisplayMemberBinding = "{Binding FirstName}" ></ GridViewColumn >
               
        //                           < GridViewColumn Header = "Отчество" Width = "100" DisplayMemberBinding = "{Binding MiddleName}" ></ GridViewColumn >
                    
        //                                < GridViewColumn Header = "Телефон" Width = "100" DisplayMemberBinding = "{Binding Phone}" ></ GridViewColumn >
                         
        //                                     < GridViewColumn Header = "Почта" Width = "185" DisplayMemberBinding = "{Binding Email}" ></ GridViewColumn >
                              
        //                                          < GridViewColumn Header = "Пол" Width = "100" DisplayMemberBinding = "{Binding Gender.GenderName}" ></ GridViewColumn >
        }
    }
}
