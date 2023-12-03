using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Recovery.xaml
    /// </summary>
    public partial class Recovery : Window
    {
        public Recovery()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Hide();
        }

        private async void Found_Click(object sender, RoutedEventArgs e)
        {
            Workers worker = new Workers()
            {
                WorkerCode = decimal.Parse(WorkerCode_Textbox.Text),
                SecretWord = SecretWord.Text,
            };

            Password.Text = await WorkerPasswordGetAsync(worker);
        }
        private static async Task<string> WorkerPasswordGetAsync(Workers worker)
        {
            HttpResponseMessage response = await MainWindow.httpClient.PostAsJsonAsync($"{MainWindow.Path}/Worker/Get/Password",worker);
            return await response.Content.ReadAsStringAsync();
        }
        private static async Task<bool> WorkerPasswordPostAsync(Workers worker)
        {
            HttpResponseMessage response = await MainWindow.httpClient.PostAsJsonAsync($"{MainWindow.Path}/Worker/Password/Rename", worker);
            bool x = Convert.ToBoolean(await response.Content.ReadAsStringAsync());
            
            return x;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Workers worker = new Workers()
            {
                WorkerCode = decimal.Parse(WorkerCode_Textbox.Text),
                SecretWord = SecretWord.Text,
                Password = Password.Text,
            };
            if(await WorkerPasswordPostAsync(worker))
            {
                MessageBox.Show("Вы изменили пароль");
                return;
            }
        }
    }
}
