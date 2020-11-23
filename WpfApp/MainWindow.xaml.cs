using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource cancel;
        private Worker.Worker worker;
        public MainWindow()
        {
            InitializeComponent();

            inputFilePath.Text = System.IO.Path.Combine(Environment.CurrentDirectory, "Data.txt");
            
            worker = new Worker.Worker();
        }

        private async void ObButtonReadFile(object sender, RoutedEventArgs e)
        {
            int window = 0;
            if (!int.TryParse(inputWindowSize.Text, out window))
            {
                MessageBox.Show($"Значение ширины окна фильтра должно быть числом", "Ошибка");
                return;
            }

            Filter filter = null;

            try
            {
                filter = new Filter(window);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка");
                return;
            }

            worker.FilePath = inputFilePath.Text;
            loading.Visibility = Visibility.Visible;
            cancel = new CancellationTokenSource();
            chart.ValuesFilter.Clear();
            chart.ValuesOrigin.Clear();

            ConsoleWrite($"Чтение \"{worker.FilePath}\"...");

            try
            {
                int count = 0;
                await foreach (float num in worker.ReadFile(cancel.Token))
                {
                    ConsoleWrite($"{count++}: {num}");
                    chart.ValuesOrigin.Add(num);

                    float? filterResult = filter.TryFilter(num);
                    if (filterResult.HasValue)
                        chart.ValuesFilter.Add(filterResult.Value);
                }

                float[] finish = filter.Finish();
                foreach (var item in finish)
                    chart.ValuesFilter.Add(item);


                ConsoleWrite($"Чтение \"{worker.FilePath}\" завершено");
            }
            catch (OperationCanceledException)
            {
                ConsoleWrite($"Чтение \"{worker.FilePath}\" отменено");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка");
            }


            loading.Visibility = Visibility.Hidden;
        }

        private void OnButtonFileBrowse(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                inputFilePath.Text = dlg.FileName;
            }
        }

        private void OnButtonCancel(object sender, RoutedEventArgs e)
        {
            loading.Visibility = Visibility.Hidden;
            cancel?.Cancel();
        }

        private void ConsoleWrite(string text)
        {
            console.Text += text + Environment.NewLine;
        }
    }
}
