using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Collections;
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
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using UserClass;
using RecipeClass;
using System.Net.Mail;
using System.Net;

namespace TrenerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Person osoba;
        Person osoba = new Person();
        Recipe recipe = new Recipe();
        // public ObservableCollection<Recipe> recipesList;

        public MainWindow()
        {
            InitializeComponent();

            calendarRecipesList.ItemsSource = RecipeData.Instance.Recipes;
            searchRecips.ItemsSource = RecipeData.Instance.Recipes;
            Category_ComboBox.ItemsSource = RecipeData.Instance.Recipes;
            CategoriesComboBox.ItemsSource = CategoryData.Instance.Categories;
            // RatingsComboBox.ItemsSource = RecipeData.Instance.Recipes;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(calendarRecipesList.ItemsSource) as CollectionView;
            view.Filter = RecipesFilter;
            //view.Filter = RecipesFilterByCategory;

            //.Filter = RecipesFilterByCategory;

            //view.Filter = RecipesFilterByCategory;
            //   view.Filter = RatingsFilter;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //Person osoba = new Person();

            osoba = PersonData.Instance.GetPerson(0);

            BMI_StatusBar.DataContext = osoba;
            BMI_Value_Label.DataContext = osoba;
            BMI_Value_TextBlock.DataContext = osoba;
            tb_name.DataContext = osoba;
            tb_surname.DataContext = osoba;
            tb_age.DataContext = osoba;
            tb_weight.DataContext = osoba;
            lb_weight.DataContext = osoba;
            lb_weight_to_lose.DataContext = osoba;
            tb_height.DataContext = osoba;
            lb_BMI.DataContext = osoba;
        }


        private void DatePicker_SelectedDateChanged(object sender,
            SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            DateTime? date = picker.SelectedDate;

            XElement xelement = XElement.Load("calendar.xml");

            IEnumerable<XElement> calendar = xelement.Elements();
            // Read the entire XML
            foreach (var day in calendar)
            {
                //  Console.WriteLine(day);
                ObservableCollection<Recipe> recipesList = new ObservableCollection<Recipe>();

                if (day.Element("day").Value.Equals(date.Value.ToString("yyyy-MM-dd")))
                {
                    Console.WriteLine("Trafiony");

                    Console.WriteLine(day);

                    Console.WriteLine(day.Elements("recipes"));



                    foreach (XElement ee in day.Descendants("recipeId"))
                    {

                        //  Console.WriteLine("dupa"+ee.Value);
                        string tempValue = ee.Value;


                        //   Console.Write(day2.Element("recipeId").Value);

                        //        tempValue = day2.Element("recipeId").Value;


                        recipesList.Add(RecipeData.Instance.GetRecipe(int.Parse(tempValue)));

                    }

                    calendarRecipesList.ItemsSource = recipesList;

                }
            }

        }

        private void BMIUpdateClick(object sender, EventArgs e)
        {
            Person osoba = new Person();
            osoba = PersonData.Instance.GetPerson(0);

            BMI_Value_Label.DataContext = osoba;

            WeightWindow dlg = new WeightWindow();
            dlg.Owner = this;
            dlg.Show();
        }

        //private void CategoriesComboBox_Loaded(object sender, RoutedEventArgs e)
        //{
        //    // ... A List.
        //    List<string> data = new List<string>();
        //    data.Add("Wszystkie typy");
        //    data.Add("Dania wegetariańskie");
        //    data.Add("Dania mięsne");
        //    data.Add("Desery");
        //    data.Add("Sałatki");

        //    var comboBox = sender as ComboBox;

        //    comboBox.ItemsSource = data;

        //    comboBox.SelectedIndex = 0;
        //}

        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            string name = comboBox.SelectedValue as string;
            this.Title = "Posiłki: " + name;
        }

        private void CaloriesComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            // ... A List.
            List<string> data = new List<string>();
            data.Add("Kaloryczność");
            data.Add("0-100");
            data.Add("101-200");
            data.Add("201-300");
            data.Add("301-400");
            data.Add("401-500");
            data.Add("500<...");

            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = data;

            comboBox.SelectedIndex = 0;
        }

        private void Calories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            string value = comboBox.SelectedItem as string;
            this.Title = "Kaloryczność dania: " + value;
        }

        private void RatingsComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            // ... A List.
            List<string> data = new List<string>();
            data.Add("");
            data.Add("0");
            data.Add("1");
            data.Add("2");
            data.Add("3");
            data.Add("4");
            data.Add("5");

            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = data;

            //   comboBox.SelectedIndex = 0;
        }

        private void Ratings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            string value = comboBox.SelectedItem as string;
            this.Title = "Ocena dania: " + value;


            /*     if (RatingsComboBox.Text.Equals("Ocena"))
                 {
                     View.Filter = null;
                 }
                 else
                 { 
                     int minimumPrice = int.Parse(value);
                     View.Filter = delegate (object item)
                     {
                         Recipe product = item as Recipe;

                         if (product != null)
                         {
                             return (product.Rating == minimumPrice);
                         }
                         return false;
                     };
                 }  */
        }

        private bool RecipesFilter(object recipe)
        {
            if (string.IsNullOrEmpty(searchTextBox.Text))
            {
                return true;
            }
            else
            {
                return ((recipe as Recipe).title.IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private bool RecipesFilterByCategory(object recipe)
        {
            searchTextBox.Text = CategoriesComboBox.SelectedValue.ToString();
            if (string.IsNullOrEmpty(searchTextBox.Text))
            {
                return true;
            }
            else
            {

                return ((recipe as Recipe).category.IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            CollectionViewSource.GetDefaultView(searchRecips.ItemsSource).Refresh();
        }

        private void CategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(searchRecips.ItemsSource).Refresh();
        }

        private bool RatingsFilter(object recipe)
        {
            if (string.IsNullOrEmpty(RatingsComboBox.Text))
            {
                return true;
            }
            else
            {

                return ((recipe as Recipe).Rating == int.Parse(RatingsComboBox.Text));
            }
        }

        private ListCollectionView View
        {
            get
            {
                return (ListCollectionView)CollectionViewSource.GetDefaultView(calendarRecipesList.ItemsSource);
            }
        }

        private void Filter(object sender, RoutedEventArgs e)
        {
            /*
                        if (RatingsComboBox.Text.Equals("Ocena"))
                        {
                            View.Filter = null;

                        }
                        else
                        {

                            int minimumPrice = int.Parse(RatingsComboBox.Text);
                            View.Filter = delegate (object item)
                            {
                                Recipe product = item as Recipe;

                                if (product != null)
                                {
                                    return (product.Rating > minimumPrice);
                                }
                                return false;
                            };
                        }*/

        }

        private void Change_Thumbnail(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".png";
            openFileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                thumbnail.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void Change_UserData(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            osoba = PersonData.Instance.GetPerson(0);

            osoba.Name = tb_name.Text;
            osoba.SecondName = tb_surname.Text;
            osoba.Age = Int32.Parse(tb_age.Text);
            try
            {
                osoba.Height = Double.Parse(tb_height.Text);
            }
            catch (Exception w)
            {
                MessageBoxResult result = MessageBox
                    .Show("Wprowadź poprawny wzrost", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                isValid = false;
            }

            try
            {
                osoba.Weight = Double.Parse(tb_weight.Text);
                osoba.WeightLeft = osoba.WeightToLose - (osoba.Weight - osoba.WeightToLose);
                BMI_StatusBar.Value = osoba.WeightLeft;
            }
            catch (Exception w)
            {
                MessageBoxResult result = MessageBox
                    .Show("Wprowadź poprawną wagę", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                isValid = false;
            }

            if (isValid)
            {
                MessageBoxResult result2 = MessageBox
                    .Show("Zmiany zostały zapisane", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void SendEmail()
        {
            if (tbEmail.Text != "" && dpDate.SelectedDate.HasValue)
            {
                DateTime? date = dpDate.SelectedDate;
                var fromAddress = new MailAddress("bookwebemail@gmail.com", "TrenerApp Email");
                var toAddress = new MailAddress(tbEmail.Text, "Odbiorca");
                const string fromPassword = "bookweb123";
                string subject = "Twój super jadłospis z dnia: " + date.Value.ToShortDateString();
                string body = @"No i kurde wypas niesamowity teraz.";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                    smtp.Send(message);
                MessageBox.Show("Email został wysłany!");
            }
            else
            {
                MessageBox.Show("Nieprawidłowe dane.");
            }
        }

        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            SendEmail();
        }

        private void addToCalendarButton_Click(object sender, RoutedEventArgs e)
        {







        }
    }
}

