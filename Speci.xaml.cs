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
    /// Логика взаимодействия для Speci.xaml
    /// </summary>
    public partial class Speci : Page
    {
        public Speci()
        {
            InitializeComponent();
            spec.ItemsSource = CollegePriemEntities.GetContext().Specialities.ToList();
        }

        private void back_click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Menu());
        }
    }
}
