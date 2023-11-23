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

namespace WpfApp1

{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MD5Hash MD5 = new MD5Hash();
        private static HttpClient httpClient = new HttpClient();
        public static string Path = "https://localhost:7244";
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            List<Workers> worker = await RolesGetAsync(Path);
            foreach(var c in worker)
            {
                UserType.Items.Add(c.Division);
            }
        }
        private async void AutoButton_Click(object sender, RoutedEventArgs e)
        {
            Users first = new Users();
            if (Login.Text != string.Empty)
                if (Password.Password != string.Empty)
                    if (UserType.Text != string.Empty)
                        if (SecretWord.Text != string.Empty)
                        {
                            try
                            {   if (UserType.Text =="Администрация"||UserType.Text=="Служба безопасности")
                                    first.WorkerCode = Convert.ToDecimal(Login.Text);
                                else
                                {
                                    first.Email = Login.Text;
                                }
                                //first.Password = MD5.GetHash(Password.Password);
                                first.Password = Password.Password;
                                Users user = await AutoAsync(first, Path);
                                MessageBox.Show(user.ToString());
                                if (user!=null)
                                {
                                    MessageBox.Show(user.Password);
                                    switch (user.Workers.Division.ToString())
                                    {
                                        case "Администрация":
                                            MessageBox.Show($"Вы вошли как {user.Workers.Division}");
                                            Security security = new Security();
                                            security.Show();
                                            break;
                                        case "Служба безопасности":
                                            MessageBox.Show($"Вы вошли как {user.Workers.Division}");
                                            break;
                                        default:
                                            MessageBox.Show("Вы вошли как обычный пользователь");
                                            break;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Неправильный логин или пароль!");
                                }
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
            else
            {
                MessageBox.Show("Заполнены не все поля");
            }
        }
        public async Task<Users> AutoAsync(Users found,string path)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync
            ($"{path}/Users/Auto/Security", found);
            string c = await response.Content.ReadAsStringAsync();
            Users Autorizhation = JsonSerializer.Deserialize<Users>(c);

            return Autorizhation;
        }   
        public static async Task<List<Workers>> RolesGetAsync(string path)
        {
            
            HttpResponseMessage response = await httpClient.GetAsync($"{path}/Users/Get/Roles");
            string c = await response.Content.ReadAsStringAsync();
            List<Workers> a = JsonSerializer.Deserialize<List<Workers>>(c);
            return a;
        }
    }
}
