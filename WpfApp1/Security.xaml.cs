using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class Security : Window
    {
        MainWindow MainWindow = new MainWindow();
        public Security()
        {
            InitializeComponent();
            this.Loaded += Security_Loaded;
        }
        private void Security_Loaded(object sender, RoutedEventArgs e)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri (@"C:\Users\Michael\Desktop\Security\WpfApp1\bin\Debug\default\logo.png", UriKind.Absolute);
            logo.EndInit();
            Photo.Source = logo;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == String.Empty)
            {
                MessageBox.Show("Вы не ввели Имя!");
                return;
            }
            if (FirstName.Text == String.Empty)
            {
                MessageBox.Show("Вы не ввели Фамилию!");
                return;
            }
            if (LastName.Text == String.Empty)
            {
                MessageBox.Show("Вы не ввели Отчество");
                return;
            }
            if (Divison.Text == String.Empty)
            {
                MessageBox.Show("Вы не ввели Должность");
                return;
            }
            if(Gender.Text == String.Empty)
            {
                MessageBox.Show("Вы не выбрали Пол");
                return;
            }

            Random random = new Random();

            Workers worker = new Workers()
            {
                WorkerCode = random.Next(1000000, 9999999),
                Name = Name.Text,
                Surname = FirstName.Text,
                Patrynomic = LastName.Text,
                Division = Divison.Text,
                SecretWord = null,
                Depart = null,
                Password = null,
            };

            if (await WorkersPostAsync(MainWindow.Path, worker))
            {
                MessageBox.Show($"Пользователь добавлен");
                Name.Text = String.Empty;
                FirstName.Text = String.Empty;
                LastName.Text = String.Empty;
                Divison.Text = String.Empty;
            }
            else
            {
                MessageBox.Show("Пользователь не был добавлен");
            }
        }
        public static async Task<bool> WorkersPostAsync(string path, Workers worker)
        {
            HttpResponseMessage response = await MainWindow.httpClient.PostAsJsonAsync
            ($"{path}/Workers/Post", worker);
            string c = await response.Content.ReadAsStringAsync();
            bool x =  bool.Parse(c);
            return x;
        }
    }
}
