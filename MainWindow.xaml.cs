using System.IO;
using System.Runtime.InteropServices;
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

namespace Unalom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<Resource> resources;
        static Dictionary<string, int> basket;
        Resource selected;
        public MainWindow()
        {
            InitializeComponent();
            resources = new();
            basket = new();
            FileRead();
            resourceLBX.ItemsSource = resources.Select(x => x.material);
        }
        static void FileRead() 
        {
            try
            {
                using StreamReader sr = new(@"..\..\..\src\resources.txt");
                while (!sr.EndOfStream) resources.Add(new Resource(sr.ReadLine()));
            }
            catch
            {
                MessageBox.Show("Valami hiba történt!","Hiba",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void resourceLBX_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected = resources.Where(x => x.material == resourceLBX.SelectedItem).First();
            pickedResourceLBL.Content = selected;
        }

        private void toBasketBTN_Click(object sender, RoutedEventArgs e)
        {
            int unit;
            if (resourceLBX.SelectedItem == null)
            {
                MessageBox.Show("Válassz ki egy anyagot!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else 
            {
                if (int.TryParse(unitTXB.Text.Trim(), out unit))
                {
                    if (basket.ContainsKey(selected.material))
                    {
                        basket[selected.material] += unit;
                    }
                    else
                    {
                        basket.Add($"{selected.material}", unit);
                    }

                    basketLBX.Items.Add($"{unit}{selected.unitOfMeasure} {selected.material}");
                }
                else MessageBox.Show("Számot írj be!", "Hiba!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void orderBTN_Click(object sender, RoutedEventArgs e)
        {
            int allPrice = 0;

            foreach (var item in basket) 
            {
                Resource temp = resources.Where(x => x.material == item.Key).First();
                allPrice += item.Value * temp.price;
            }

            MessageBox.Show($"Sikeres rendelés leadás!\nVégösszeg: {allPrice} Ft.","Siker!",MessageBoxButton.OK,MessageBoxImage.Information);

            using StreamWriter sw = new(@"..\..\..\src\orders.txt",true);

            sw.WriteLine("------------------------");

            foreach (var item in basket)
            {
                string materialUnitType = resources.Where(x => x.material == item.Key).First().unitOfMeasure;
                sw.WriteLine($"{item.Key} - {item.Value} {materialUnitType}");
            }

            sw.WriteLine($"\n{allPrice} Ft");

            basketLBX.Items.Clear();
        }
    }
}