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
                imagePath = "images/schabowe.png"
            });

            recipesList.Add(new Recipe()
            {
                title = "Bigos szlachetny",
                description = "Kunszt i tradycja.",
                imagePath = "images/bigos.png"
            });

            recipes.ItemsSource = recipesList;
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

    }
}
