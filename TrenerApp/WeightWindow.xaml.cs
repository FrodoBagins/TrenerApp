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
using System.Windows.Shapes;
using UserClass;

namespace TrenerApp
{
    /// <summary>
    /// Interaction logic for WeightWindow.xaml
    /// </summary>
    public partial class WeightWindow : Window
    {

        Person osoba = new Person();
        private bool WeightValueChanged;
        MainWindow main;



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GiveWeightTextBox.TextChanged += new TextChangedEventHandler(GiveWeightTextBox_TextChanged);


        }

        public WeightWindow()
        {
            InitializeComponent();

            osoba = PersonData.Instance.GetPerson(0);

            GiveWeightTextBox.DataContext = osoba;

            WeightValueChanged = false;
        }


        private void Accept(object sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Delete(object sender, EventArgs e)
        {

            Close();
        }


        private void CountBMI_Executed(object sender,ExecutedRoutedEventArgs e)
        {

            HelperClass help = new HelperClass();

            osoba.BMI = help.CalculateBMI(osoba.Weight, osoba.Height);

        

        }

        private void CountBMI_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (WeightValueChanged == true)
            {
                e.CanExecute = true;
            }
            else
                e.CanExecute = false;

        }

        private void GiveWeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            WeightValueChanged = true;
        }
    }
}
