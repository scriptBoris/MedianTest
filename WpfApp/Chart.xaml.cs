using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Логика взаимодействия для Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        public Chart()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Без фильтра",
                    Values = ValuesOrigin,
                },
                new LineSeries
                {
                    Title = "Медианный фильтр",
                    Values = ValuesFilter,
                    Foreground = Brushes.Green,
                }
            };

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => ((int)value).ToString();

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public ChartValues<float> ValuesOrigin { get; set; } = new ChartValues<float>();
        public ChartValues<float> ValuesFilter { get; set; } = new ChartValues<float>();
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

    }
}
