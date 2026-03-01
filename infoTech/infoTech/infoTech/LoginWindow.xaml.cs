using infoTech.Entity;
using System;
using System.Linq;
using System.Windows;

namespace infoTech
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                txtError.Text = "Введите логин и пароль";
                return;
            }

            using (var db = new infoTechEntities())
            {
                var master = db.Masters.FirstOrDefault(m => m.login == login && m.password == password);
                if (master != null)
                {
                    // Сохраняем текущего пользователя (можно в статическом поле)
                    App.CurrentUser = master;
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    txtError.Text = "Неверный логин или пароль";
                }
            }
        }
    }
}