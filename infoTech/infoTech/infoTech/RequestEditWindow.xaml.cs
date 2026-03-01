using infoTech.Entity;
using System;
using System.Linq;
using System.Windows;

namespace infoTech
{
    public partial class RequestEditWindow : Window
    {
        private infoTechEntities db = new infoTechEntities();
        private Requests currentRequest;

        // Конструктор для добавления
        public RequestEditWindow()
        {
            InitializeComponent();
            LoadComboBoxes();
            dpStartDate.SelectedDate = DateTime.Today;
            currentRequest = new Requests();
        }

        // Конструктор для редактирования
        public RequestEditWindow(Requests request)
        {
            InitializeComponent();
            LoadComboBoxes();
            currentRequest = request;

            // Заполняем поля
            dpStartDate.SelectedDate = request.startDate;
            cmbDeviceType.SelectedValue = request.deviceTypeID;
            txtModel.Text = request.deviceModel;
            txtProblem.Text = request.problemDescription;
            cmbStatus.SelectedValue = request.statusID;
            cmbMaster.SelectedValue = request.masterID;
            cmbClient.SelectedValue = request.clientID;
            dpCompletionDate.SelectedDate = request.completionDate;
            txtRepairParts.Text = request.repairParts;
        }

        private void LoadComboBoxes()
        {
            cmbDeviceType.ItemsSource = db.DeviceTypes.ToList();
            cmbStatus.ItemsSource = db.RequestStatuses.ToList();
            cmbMaster.ItemsSource = db.Masters.ToList();
            cmbClient.ItemsSource = db.Clients.ToList();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Валидация
            if (cmbDeviceType.SelectedValue == null || cmbStatus.SelectedValue == null || cmbClient.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtModel.Text) || string.IsNullOrWhiteSpace(txtProblem.Text))
            {
                MessageBox.Show("Заполните все обязательные поля (Тип, Модель, Проблема, Статус, Заказчик)", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Заполняем данные
            currentRequest.startDate = dpStartDate.SelectedDate ?? DateTime.Today;
            currentRequest.deviceTypeID = (int)cmbDeviceType.SelectedValue;
            currentRequest.deviceModel = txtModel.Text.Trim();
            currentRequest.problemDescription = txtProblem.Text.Trim();
            currentRequest.statusID = (int)cmbStatus.SelectedValue;
            currentRequest.masterID = cmbMaster.SelectedValue as int?; // может быть null
            currentRequest.clientID = (int)cmbClient.SelectedValue;
            currentRequest.completionDate = dpCompletionDate.SelectedDate;
            currentRequest.repairParts = string.IsNullOrWhiteSpace(txtRepairParts.Text) ? null : txtRepairParts.Text.Trim();

            try
            {
                if (currentRequest.requestID == 0) // новая
                {
                    db.Requests.Add(currentRequest);
                }
                else
                {
                    db.Entry(currentRequest).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}