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

namespace WpfApp1
{
    public partial class Security : Window
    {
        MainWindow MainWindow = new MainWindow();
        string filename = null;
        public Security()
        {
            InitializeComponent();
            this.Loaded += Security_Loaded;
        }
        private async void Security_Loaded(object sender, RoutedEventArgs e)
        {
            var photo = await ImageGetAsync(1);
            byte[] qq = photo.Data;
            Photo.Source = LoadImage(qq);
        }
        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
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
                Role = null,
                SecretWord = null,
                Depart = null,
                Password = null,
            };
            if (filename != null)
            {
                if (await WorkersPostAsync(MainWindow.Path, worker))
                {
                    Workers cut = await MainWindow.AutoWorkerAsync(MainWindow.Path, worker);
                    await ImagePostAsync(filename, cut.IdWorker);
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
            else
            {
                MessageBox.Show("Не выбрана фотография рабочего");
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
            if (openFileDialog1.ShowDialog() == DialogResult)
                return;
            filename = openFileDialog1.FileName;

            Photo.Source = new BitmapImage(new Uri(filename, UriKind.Absolute));
        }
        public static async Task<WorkersPhoto> ImageGetAsync(int id)
        {
            string image = await MainWindow.httpClient.GetStringAsync($"{MainWindow.Path}/Workers/Photo/Get/{id}");
            var photo = JsonSerializer.Deserialize<WorkersPhoto>(image);
            return photo;
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
    }
}
