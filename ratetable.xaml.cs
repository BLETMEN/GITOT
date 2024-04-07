using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using USSR2._0;

namespace USSR2._0
{
    public partial class ratetable : Page
    {
        public ratetable()
        {
            InitializeComponent();
            InitializeButtonsVisibility();
            // Загружаем список кандидатов в DataGrid
            LoadCandidates();
        }
        private void LoadAbiturients()
        {
            var abiturients = CollegePriemEntities.GetContext().Candidates.ToList();
            ratetab.ItemsSource = abiturients;
        }

        private void LoadCandidates()
        {
            // Получаем список кандидатов из базы данных
            List<Candidate> candidates = CollegePriemEntities.GetContext().Candidates.ToList();

            int userRole = AuthStorage.Role;

            // Устанавливаем видимость кнопки "Редактировать" для каждого кандидата
            foreach (var candidate in candidates)
            {
                candidate.IsEditButtonVisible = userRole == 1 || userRole == 2;
            }

            // Устанавливаем источник данных для DataGrid
            ratetab.ItemsSource = candidates;
            if (userRole != 2 && userRole != 1)
            {
                DataGridTemplateColumn editColumn = ratetab.Columns.FirstOrDefault(column => column.Header.ToString() == "Редактировать") as DataGridTemplateColumn;
                if (editColumn != null)
                {
                    editColumn.Visibility = Visibility.Collapsed;
                }
            }

        }
        private void InitializeButtonsVisibility()
        {
            int userRole = AuthStorage.Role;
            Add.Visibility = userRole == 2 || userRole == 1 ? Visibility.Visible : Visibility.Collapsed;
            Del.Visibility = userRole == 2 || userRole == 1 ? Visibility.Visible : Visibility.Collapsed;

        }
        private void add_Click(object sender, RoutedEventArgs e)
        {
           
                
                NavigationService.Navigate(new addag(null));

               
                
            
        }
        private void del_Click(object sender, RoutedEventArgs e)
        {
            var selectedAbiturients = ratetab.SelectedItems.Cast<Candidate>().ToList();
            MessageBoxResult messageBoxResult = MessageBox.Show($"Удалить {selectedAbiturients.Count()} записей?", "Удаление", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (messageBoxResult == MessageBoxResult.OK)
            {
                try
                {
                    var context = CollegePriemEntities.GetContext();

                    foreach (var abiturient in selectedAbiturients)
                    {
                        // Удаляем связанные записи в CandidateExams
                        var relatedExams = context.CandidateExams.Where(ce => ce.CandidateID == abiturient.CandidateID).ToList();
                        context.CandidateExams.RemoveRange(relatedExams);

                        // Теперь удаляем запись абитуриента
                        context.Candidates.Remove(abiturient);
                    }


                    context.SaveChanges();

                    MessageBox.Show("Записи удалены");

                    List<Candidate> candidates = context.Candidates.OrderBy(p => p.LastName).ToList();
                    ratetab.ItemsSource = null;
                    ratetab.ItemsSource = candidates;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void back(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Menu());
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранного кандидата из DataContext кнопки
            if (sender is Button btn && btn.DataContext is Candidate candidate && candidate.IsEditButtonVisible)
            {
                // Переходим на страницу редактирования кандидата
                NavigationService.Navigate(new addag(candidate));

                // Обновляем DataGrid после редактирования
                LoadCandidates();
            }
        }
        
    }

   
}
