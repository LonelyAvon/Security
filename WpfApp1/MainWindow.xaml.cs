using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Hash;
using WpfApp1.Models;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace WpfApp1

{
    public partial class MainWindow : Window
    {
        MD5Hash MD5 = new MD5Hash();
        public static HttpClient httpClient = new HttpClient();
        public static string Path = "https://localhost:7244";
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        //Заполнение ComboBox ролями из базы данных
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Workers> worker = await RolesGetAsync(Path);
                foreach (var c in worker)
                {
                    UserType.Items.Add(c.Division);
                }
            }
            catch 
            {
                MessageBox.Show("Соединение с сервером не установлено");
                this.Close(); 
            }
        }
        private async void AutoButton_Click(object sender, RoutedEventArgs e)
        {
            //Проверка на нулевые значения
            if (UserType.Text == String.Empty)
            {
                MessageBox.Show("Вы не ввели Тип пользователя!");
                return;
            }
            if (Login.Text == String.Empty)
            {
                MessageBox.Show("Вы не ввели логин!");
                return;
            }
            if (Password.Password == String.Empty)
            {
                MessageBox.Show("Вы не ввели пароль");
                return;
            }
            if (SecretWord.Text == string.Empty)
            {
                MessageBox.Show("Вы не ввели секретное слово");
                return;
            }


            //Проверка кода сотрудника
            decimal workerCode = 0;
            try 
            {
                if (Login.Text.Length == 7)
                    workerCode = decimal.Parse(Login.Text);
                else
                    return;
            }
            catch { MessageBox.Show("Введите код сотрудника правильно"); };
            //Создание рабочего для авторизации из полученных данных
            Workers first = new Workers()
            {
                WorkerCode = workerCode,
                Password = Password.Password,
                SecretWord = SecretWord.Text,

            };
            //Авторизация
            var worker = await AutoWorkerAsync(Path, first);
            if (worker!=null)
            {
                //Проверка на правильность выбранной роли
                if (worker.Division != UserType.Text)
                {
                    MessageBox.Show("Выбран неверный тип пользователя!");
                    return;
                }
                switch(worker.Division)
                {
                    case "Администрация":
                        MessageBox.Show($"Вы зашли под аккаунтом {worker.Division}");
                        Security admin = new Security();
                        admin.Show();
                        break;
                    case "Служба безопасности":
                        MessageBox.Show($"Вы зашли под аккаунтом {worker.Division}");
                        break;
                    //Страница для остальных рабочих
                    default:
                        MessageBox.Show($"Вы зашли под аккаунтом {worker.Division}");
                        break;
                }
            }

        }
        // Метод отправки рабочего на авторизацию и получение полных данных о рабочем в случае успешной авторизации
        public static async Task<Workers> AutoWorkerAsync(string path, Workers work)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync
            ($"{path}/Users/Auto/Security", work);
            string c = await response.Content.ReadAsStringAsync();
            Workers worker = JsonSerializer.Deserialize<Workers>(c);
            return worker;
        }   
        // Метод для заполнения Типов пользователей
        public static async Task<List<Workers>> RolesGetAsync(string path)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{path}/Users/Get/Roles");
            string c = await response.Content.ReadAsStringAsync();
            List<Workers> a = JsonSerializer.Deserialize<List<Workers>>(c);
            return a;
        }
    }
}
