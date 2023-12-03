using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Drawing;
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
using System.Windows.Shapes;
using WpfApp1.Models;
using System.Data.SqlTypes;
using System.Windows.Media.Imaging;
using System.Runtime.Remoting.Contexts;
using System.Net;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices.ComTypes;
using Xamarin.Forms.Shapes;
using Microsoft.Win32;
using Path = System.IO.Path;
using System.Runtime.InteropServices;

namespace WpfApp1
{
    
    public partial class Security : Window
    {
        List<Roles> k;
        MainWindow MainWindow = new MainWindow();
        string filename = null;
        public Security()
        {
            InitializeComponent();
            this.Loaded += Security_Loaded;
        }
        string sourceLogo = Path.GetFullPath("def\\logo.png");

        //Метод вызываемый при загрузке формы
        private async void Security_Loaded(object sender, RoutedEventArgs e)
        {
            Photo.Source = new BitmapImage(new Uri(sourceLogo));

            //Получение всех ролей из базы и добавление их в ComboBox
            List<Roles> role = await MainWindow.RolesGetAsync(MainWindow.Path);
            k = role;
            foreach (var c in role)
            {
                UserType.Items.Add(c.Name);
            }
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
            if (UserType.Text == String.Empty)
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

            //Получение Id роли для создания объекта класса Worker
            int c = 1;
            foreach(var roles in k)
            {
                if(roles.Name==UserType.Text)
                {
                    c = roles.IdRole;
                    break;
                }
            }

            //Создание рабочего для вставки в базу данных
            Workers worker = new Workers()
            {
                WorkerCode = random.Next(1000000, 9999999),
                Name = Name.Text,
                Surname = FirstName.Text,
                Patrynomic = LastName.Text,
                Role = c,
                SecretWord = null,
                Depart = null,
                Password = null,
            };

            // очистка полей
            if (await WorkersPostAsync(MainWindow.Path, worker))
            {
                Workers cut = await MainWindow.AutoWorkerAsync(MainWindow.Path, worker);
                await ImagePostAsync(filename, cut.IdWorker);
                MessageBox.Show($"Пользователь добавлен");
                Name.Text = String.Empty;
                FirstName.Text = String.Empty;
                LastName.Text = String.Empty;
                UserType.Text = String.Empty;
                Photo.Source = new BitmapImage(new Uri(sourceLogo));
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
            bool x = bool.Parse(c);
            return x;
        }

        private void PhotoDownload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "image|*.jpg|image|*.png|image|*.jpeg";

            if (openFileDialog1.ShowDialog()==false)
            {
                MessageBox.Show("Не выбрана фотография");
                return;
            }
            filename = openFileDialog1.FileName;
            Photo.Source = new BitmapImage(new Uri(filename, UriKind.Absolute));
        }
        public static async Task<WorkersPhoto> ImagePostAsync(string filename,int id)
        {
            byte[] buffer = File.ReadAllBytes(filename);
            WorkersPhoto a = new WorkersPhoto()
            {   IdWorker = id,
                Data = buffer };
            HttpResponseMessage response = await MainWindow.httpClient.PostAsJsonAsync
            ($"{MainWindow.Path}/Workers/Photo/Post", a);
            string c = await response.Content.ReadAsStringAsync();
            var photo = JsonSerializer.Deserialize<WorkersPhoto>(c);
            return photo;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
