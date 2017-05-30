using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UserClass;

namespace TrenerApp
{
    /// <summary>
    /// Interaction logic for WeightWindow.xaml
    /// </summary>
    public partial class WeightWindow : Window
    {
        Person user = new Person();
        private bool WeightValueChanged;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GiveWeightTextBox.TextChanged += new TextChangedEventHandler(GiveWeightTextBox_TextChanged);
        }

        public WeightWindow()
        {
            InitializeComponent();
            user = PersonData.Instance.GetPerson(0);
            GiveWeightTextBox.DataContext = user;
            WeightValueChanged = false;
        }

        private void Accept(object sender, EventArgs e)
        {
            user = PersonData.Instance.GetPerson(0);
            user.WeightLeft2 = user.WeightLeft - (user.Weight - user.WeightToLose);
            (this.Owner as MainWindow).BMI_StatusBar.Value = user.WeightLeft2;
            Close();
        }

        private void Delete(object sender, EventArgs e)
        {
            Close();
        }

        private void CountBMI_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HelperClass help = new HelperClass();
            user.BMI = help.CalculateBMI(user.Weight, user.Height);
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
