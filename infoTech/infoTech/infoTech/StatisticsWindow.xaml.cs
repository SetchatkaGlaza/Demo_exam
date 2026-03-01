using infoTech.Entity;
using System;
using System.Linq;
using System.Windows;

namespace infoTech
{
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow()
        {
            InitializeComponent();
            LoadStats();
        }

        private void LoadStats()
        {
            using (var db = new infoTechEntities())
            {
                // Статистика по мастерам (статус "Готова к выдаче" = 3)
                var masterStats = db.Requests
                    .Where(r => r.statusID == 3)
                    .GroupBy(r => r.Masters.fio)
                    .Select(g => new { Мастер = g.Key, Количество = g.Count() })
                    .ToList();
                dgMasterStats.ItemsSource = masterStats;

                // Статистика по типам устройств
                var deviceStats = db.Requests
                    .GroupBy(r => r.DeviceTypes.typeName)
                    .Select(g => new { Тип = g.Key, Количество = g.Count() })
                    .ToList();
                dgDeviceStats.ItemsSource = deviceStats;
            }
        }
    }
}