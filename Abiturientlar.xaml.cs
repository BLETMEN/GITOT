using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using USSR2._0;

namespace USSR2._0
{
    public partial class Abiturientlar : Page
    {
        private List<Order> candidates; // Список всех абитуриентов
        private List<Order> filteredCandidates; // Отфильтрованный список абитуриентов после поиска
        private int currentPage = 1; // Текущая страница
        private int itemsPerPage = 4; // Количество записей на странице

        public Abiturientlar()
        {
            InitializeComponent();
            LoadCandidates(); // Загружаем список абитуриентов
            LoadPage(); // Загружаем первую страницу
        }

        private void LoadCandidates()
        {
            // Загрузка всех абитуриентов из базы данных
            candidates = OknaEntities.GetContext().Orders.ToList();
        }

        private void LoadPage()
        {
            // Вычисляем общее количество страниц и максимальную страницу
            var totalCount = filteredCandidates != null ? filteredCandidates.Count : candidates.Count;
            var maxPage = (int)Math.Ceiling((double)totalCount / itemsPerPage);

            // Получаем текущую страницу
            var currentPageCandidates = filteredCandidates != null
                ? filteredCandidates.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage).ToList()
                : candidates.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            // Заполняем список абитуриентов на текущей странице
            Abit.ItemsSource = currentPageCandidates;

            // Обновляем номер текущей страницы и текст на кнопках
            CurrentPageTextBlock.Text = currentPage.ToString();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Получаем выбранный вариант сортировки из комбобокса
            var selectedSortOption = SortComboBox.SelectedItem as ComboBoxItem;

            if (selectedSortOption != null)
            {
                var sortOption = selectedSortOption.Content.ToString();

                // Сортируем список абитуриентов в соответствии с выбранной сортировкой
                switch (sortOption)
                {
                    case "По фамилии":
                        candidates = candidates.OrderBy(c => c.Specification).ToList();
                        break;

                    case "По специальности":
                        candidates = candidates.OrderBy(c => c.TotalAmount).ToList();
                        break;

                    default:
                        break;
                }

                // Если был выполнен поиск, снова применяем фильтр к отсортированным данным
                if (filteredCandidates != null)
                {
                    filteredCandidates = filteredCandidates.OrderBy(c => c.Specification).ToList();
                }

                // Возвращаемся на первую страницу после сортировки
                currentPage = 1;
                LoadPage();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = searchBox.Text.ToLower();
            filteredCandidates = OknaEntities.GetContext().Orders.Where(c => c.Specification.ToLower().Contains(searchTerm)).ToList();
            currentPage = 1;
            LoadPage();
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPage();
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            var totalCount = filteredCandidates != null ? filteredCandidates.Count : candidates.Count;
            var maxPage = (int)Math.Ceiling((double)totalCount / itemsPerPage);

            if (currentPage < maxPage)
            {
                currentPage++;
                LoadPage();
            }
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Menu());
        }
    }
}
