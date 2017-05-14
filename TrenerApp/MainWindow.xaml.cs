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

namespace TrenerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<Recipe> recipesList;

            recipesList = new List<Recipe>();

            recipesList.Add(new Recipe()
            {
                title = "Schabowy",
                description = "Mniam mniam pyszne mięsko.",
                imagePath = "images/schabowe.jpg"
            });

            recipesList.Add(new Recipe()
            {
                title = "Bigos szlachetny",
                description = "Kunszt i tradycja.",
                imagePath = "images/bigos.jpg"
            });

            recipes.ItemsSource = recipesList;
            searchRecips.ItemsSource = recipesList;
        }

        private void DatePicker_SelectedDateChanged(object sender,
            SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            DateTime? date = picker.SelectedDate;
            //if (date == null)
            //{
            //    this.Title = "Brak daty.";
            //}
            //else
            //{
            //    this.Title = date.Value.ToShortDateString();
            //}
        }


        private void BMIUpdateClick(object sender, EventArgs e)
        {

            WeightWindow dlg = new WeightWindow();
            dlg.ShowDialog();


        }

        private void ComboBox_LoadedAllTypes(object sender, RoutedEventArgs e)
        {
            // ... A List.
            List<string> data = new List<string>();
            data.Add("Wszystkie typy");
            data.Add("Dania wegetariańskie");
            data.Add("Dania mięsne");
            data.Add("Desery");
            data.Add("Sałatki");

            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = data;

            comboBox.SelectedIndex = 0;
        }
        private void AllTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            string value = comboBox.SelectedItem as string;
            this.Title = "Posiłki: " + value;
        }

        private void ComboBox_LoadedKcal(object sender, RoutedEventArgs e)
        {
            // ... A List.
            List<string> data = new List<string>();
            data.Add("Kaloryczność");
            data.Add("0-100");
            data.Add("101-200");
            data.Add("201-300");
            data.Add("301-400");

            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = data;

            comboBox.SelectedIndex = 0;
        }
        private void Kcal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            string value = comboBox.SelectedItem as string;
            this.Title = "Kaloryczność dania: " + value;
        }

    }
    }
