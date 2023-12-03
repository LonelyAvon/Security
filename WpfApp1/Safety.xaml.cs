using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
    /// <summary>
    /// Логика взаимодействия для Safety.xaml
    /// </summary>
    public partial class Safety : Window
    {
        public int index = 0;
        public Safety()
        {
            InitializeComponent();
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Workers> workers = await WorkersListGetAsync();

            for(int i =0;i<workers.Count;i++)

            Refresh(workers);
        }
        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            if(Password.Text == String.Empty)
            {
                MessageBox.Show("Измените пароль");
                return;
            }
            if(SecretWord.Text == String.Empty)
            {
                MessageBox.Show("Заполните секретное слово");
                return;
            }
            Workers workerUpdate = new Workers()
            {
                WorkerCode = decimal.Parse(WorkerCode_Textbox.Text),
                Password = Password.Text,
                SecretWord = SecretWord.Text,
                Depart = Depart.Text,
            };
            if(await WorkersUpdateAsync(workerUpdate)!=null)
            {
                MessageBox.Show("Пользователь изменён");
            }

        }
        private async void ToRight_Click(object sender, RoutedEventArgs e)
        {
            List<Workers> workers = await WorkersListGetAsync();
            if (index == workers.Count-1)
            {
                return;
            }
            index++;
            Refresh(workers);
        }

        private async void ToLeft_Click(object sender, RoutedEventArgs e)
        {
            List<Workers> workers = await WorkersListGetAsync();
            if (index == 0)
            {
                return;
            }
            index--;
            Refresh(workers);
        }

        public async void Refresh(List<Workers> workers)
        {
            if (workers[index].Role!=null)
            {
                var role = await RoleGetAsync(workers[index].Role);
                Role.Text = role.Name;
            }
            WorkerCode_Textbox.Text = workers[index].WorkerCode.ToString();
            Password.Text = workers[index].Password;
            FirstName.Text = workers[index].Surname;
            Name.Text = workers[index].Name;
            LastName.Text = workers[index].Patrynomic;
            Depart.Text = workers[index].Depart;
            SecretWord.Text = workers[index].SecretWord;
            IdWorker.Text = workers[index].IdWorker.ToString();
            var photo = await ImageGetAsync(workers[index].IdWorker);
            byte[] bytes = photo.Data;
            Photo.Source = LoadImage(bytes);
        }

        public static async Task<Workers> WorkersUpdateAsync(Workers work)
        {
            HttpResponseMessage response = await MainWindow.httpClient.PostAsJsonAsync($"{MainWindow.Path}/Workers/Update", work);
            string c = await response.Content.ReadAsStringAsync();
            Workers worker = JsonSerializer.Deserialize<Workers>(c);
            return worker;
        }
        public static async Task<List<Workers>> WorkersListGetAsync()
        {
            string answer = await MainWindow.httpClient.GetStringAsync($"{MainWindow.Path}/Worker/Get");
            return JsonSerializer.Deserialize<List<Workers>>(answer);
        }
        public static async Task<Roles> RoleGetAsync(int? IdRole)
        {
            string answer = await MainWindow.httpClient.GetStringAsync($"{MainWindow.Path}/Worker/Auto/Role/{IdRole}");
            return JsonSerializer.Deserialize<Roles>(answer);
        }
        public static async Task<WorkersPhoto> ImageGetAsync(int id)
        {
            string image = await MainWindow.httpClient.GetStringAsync($"{MainWindow.Path}/Workers/Photo/Get/{id}");
            var photo = JsonSerializer.Deserialize<WorkersPhoto>(image);
            return photo;
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

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Hide();
        }
    }
}
