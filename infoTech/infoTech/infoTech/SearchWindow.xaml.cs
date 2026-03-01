using infoTech.Entity;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace infoTech
{
    public partial class SearchWindow : Window
    {
        private infoTechEntities db = new infoTechEntities();

        public SearchWindow()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtRequestNumber.Text, out int number))
            {
                MessageBox.Show("Введите корректный номер заявки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Вызов хранимой процедуры
                var result = db.Database.SqlQuery<SearchResult>("EXEC FindRequestByNumber @RequestNumber",
                                                                 new SqlParameter("@RequestNumber", number)).ToList();
                dgResult.ItemsSource = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при поиске: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Класс для приёма результатов процедуры (имена полей должны совпадать с алиасами в процедуре)
    public class SearchResult
    {
        public int Номер_заявки { get; set; }
        public DateTime Дата_добавления { get; set; }
        public string Тип_устройства { get; set; }
        public string Модель { get; set; }
        public string Описание_проблемы { get; set; }
        public string Статус { get; set; }
        public DateTime? Дата_завершения { get; set; }
        public string Запчасти { get; set; }
        public string Заказчик { get; set; }
        public string Телефон_заказчика { get; set; }
        public string Мастер { get; set; }
        public string Должность_мастера { get; set; }
    }
}