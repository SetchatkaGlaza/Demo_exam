using infoTech.Entity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace infoTech
{
    public partial class MainWindow : Window
    {
        private infoTechEntities db = new infoTechEntities();

        public MainWindow()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            // Загружаем заявки с включением связанных таблиц
            dgRequests.ItemsSource = db.Requests
                .Include(r => r.DeviceTypes)
                .Include(r => r.RequestStatuses)
                .Include(r => r.Clients)
                .Include(r => r.Masters)
                .ToList();
        }

        private void Refresh()
        {
            db.Dispose();
            db = new infoTechEntities();
            LoadRequests();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            RequestEditWindow win = new RequestEditWindow();
            win.Owner = this;
            if (win.ShowDialog() == true)
                Refresh();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgRequests.SelectedItem is Requests selected)
            {
                RequestEditWindow win = new RequestEditWindow(selected);
                win.Owner = this;
                if (win.ShowDialog() == true)
                    Refresh();
            }
            else
                MessageBox.Show("Выберите заявку для редактирования", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgRequests.SelectedItem is Requests selected)
            {
                var result = MessageBox.Show($"Удалить заявку №{selected.requestID}?", "Подтверждение",
                                             MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Удаляем связанные комментарии вручную
                        var comments = db.Comments.Where(c => c.requestID == selected.requestID);
                        db.Comments.RemoveRange(comments);
                        db.Requests.Remove(selected);
                        db.SaveChanges();
                        Refresh();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
                MessageBox.Show("Выберите заявку для удаления", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow win = new SearchWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void btnStats_Click(object sender, RoutedEventArgs e)
        {
            StatisticsWindow win = new StatisticsWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void btnComments_Click(object sender, RoutedEventArgs e)
        {
            if (dgRequests.SelectedItem is Requests selected)
            {
                CommentsWindow win = new CommentsWindow(selected.requestID);
                win.Owner = this;
                win.ShowDialog();
            }
            else
                MessageBox.Show("Выберите заявку для просмотра комментариев", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}