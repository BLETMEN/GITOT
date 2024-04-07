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
    /// Логика взаимодействия для addag.xaml
    /// </summary>
    public partial class addag : Page
    {
        private Order _currentUs = new Order();
        public addag(Order abit)
        {
            InitializeComponent();
            LoadAndInitData(abit);
        }
        void LoadAndInitData(Order abit)
        {
            if (abit != null)
            {
                _currentUs = abit;
            }
            DataContext = _currentUs;
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUs.CandidateID == 0)
            {

                OknaEntities.GetContext().Orders.Add(_currentUs);

            }
            try
            {
                OknaEntities.GetContext().SaveChanges();
                MessageBox.Show("Запись Изменена");
                NavigationService.Navigate(new ratetable());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            NavigationService.GoBack();
        }


    }
}
