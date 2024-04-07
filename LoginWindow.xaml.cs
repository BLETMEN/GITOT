using USSR2._0;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace USSR2._0
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private string correctCaptcha;
        private bool captchaVisible = false;
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = this;
            correctCaptcha = GenerateCaptchaText();
            CaptchaTextBlock.Text = correctCaptcha;
            HideCaptcha();
        }
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string enteredCaptcha = CaptchaTextBox.Text;

            if (captchaVisible && !enteredCaptcha.Equals(correctCaptcha, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Капча введена неверно");
                CaptchaTextBox.Clear();
                return;
            }

            try
            {
                User user = OknaEntities.GetContext().Users.FirstOrDefault(p => p.Username == LoginTb.Text && p.PasswordHash == PassTb.Password);

                if (user != null)
                {
                    AuthStorage.Role = user.UserID;
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Данные введены некорректно");
                    ShowCaptcha();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void Canсel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string GenerateCaptchaText()
        {
            Random random = new Random();
            string characters = "ABCDEFabcdef1234567890";
            char[] captchaChars = new char[6];

            for (int i = 0; i < 6; i++)
            {
                captchaChars[i] = characters[random.Next(characters.Length)];
            }

            return new string(captchaChars);
        }

        private void ShowCaptcha()
        {

            CaptchaTextBlock.Visibility = Visibility.Visible;
            CaptchaTextBox.Visibility = Visibility.Visible;
            CaptchaLabel.Visibility = Visibility.Visible;
            PictureCaptcha.Visibility = Visibility.Visible;
            captchaVisible = true;
        }

        private void HideCaptcha()
        {
            CaptchaTextBlock.Visibility = Visibility.Hidden;
            CaptchaTextBox.Visibility = Visibility.Hidden;
            CaptchaLabel.Visibility = Visibility.Hidden;
            PictureCaptcha.Visibility = Visibility.Hidden;
            captchaVisible = false;
        }

    }
}
