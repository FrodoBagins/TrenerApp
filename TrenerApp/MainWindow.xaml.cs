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

        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        System.Windows.Threading.DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            calendarRecipesList.ItemsSource = RecipeData.Instance.Recipes;
            searchRecips.ItemsSource = RecipeData.Instance.Recipes;
            Category_ComboBox.ItemsSource = RecipeData.Instance.Recipes;
            CategoriesComboBox.ItemsSource = CategoryData.Instance.Categories;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(calendarRecipesList.ItemsSource) as CollectionView;
            CollectionView searchView = (CollectionView)CollectionViewSource.GetDefaultView(searchRecips.ItemsSource) as CollectionView;

            searchView.Filter = RecipesFilter;

            player.SoundLocation = "music.wav";
            player.Play();
            
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_Tick);
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
                         Console.WriteLine("dupa"+ee.Value);
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

        private void CategoriesComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();
            data.Add("Nazwa");
            data.Add("Kategoria");
            data.Add("Opis");
            data.Add("Ocena (1-5)");

            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = data;
            comboBox.SelectedIndex = 0;
        }

        private void Categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            string value = comboBox.SelectedItem as string;
            this.Title = "Posiłki: " + value;            
        }

        private bool RecipesFilter(object recipe)
        {
            if (string.IsNullOrEmpty(searchTextBox.Text))
            {
                return true;
            }
            else if (CategoriesComboBox.SelectedValue as string == "Nazwa")
            {
                return ((recipe as Recipe).title.IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else if (CategoriesComboBox.SelectedValue as string == "Kategoria")
            {
                return ((recipe as Recipe).category.IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else if (CategoriesComboBox.SelectedValue as string == "Opis")
            {
                return ((recipe as Recipe).description.IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else
            {
                return ((recipe as Recipe).Rating == int.Parse(searchTextBox.Text));
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            CollectionViewSource.GetDefaultView(searchRecips.ItemsSource).Refresh();
        }

        private ListCollectionView View
        {
            get
            {
                return (ListCollectionView)CollectionViewSource.GetDefaultView(calendarRecipesList.ItemsSource);
            }
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

                XElement xelement = XElement.Load("calendar.xml");
                IEnumerable<XElement> calendar = xelement.Elements();
                ObservableCollection<Recipe> recipesList = new ObservableCollection<Recipe>();

                foreach (var day in calendar)
                {
                    if (day.Element("day").Value.Equals(date.Value.ToString("yyyy-MM-dd")))
                    {
                        foreach (XElement ee in day.Descendants("recipeId"))
                        {
                            string tempValue = ee.Value;
                            recipesList.Add(RecipeData.Instance.GetRecipe(int.Parse(tempValue)));
                        }
                    }
                }
                
                string body = @"";

                foreach (var item in recipesList)
                {
                    body += "<br/><br/>";
                    body += "<div style='float: left; width: 100%; border: 1px solid black; margin-bottom: 10px; padding-bottom: 30px; padding-left: 30px;'>";
                    body += "<br/><br/>";
                    body += "<span style='font-size: 16px; font-weight: bold;'>Tytuł: " + item.Title + "</span>";
                    body += "<br/><br/>";
                    body += "<span>Opis: " + item.Description + "</span>";
                    body += "<br/><br/>";
                    body += "<span  style='font-weight: bold;'>Ilość kalorii: " + item.calories + "</span>";
                    body += "</div>";
                }

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
                    Body = body,
                    IsBodyHtml = true
                })
                    smtp.Send(message);
                MessageBox.Show("Email został wysłany! ;)");
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

        void timer_Tick (object sender, EventArgs e)
        {
            seekSlider.Value = mediaElementMovie.Position.TotalSeconds;
        }

        private void playMovie_Click(object sender, RoutedEventArgs e)
        {
            mediaElementMovie.Play();
        }

        private void pauseMovie_Click(object sender, RoutedEventArgs e)
        {
            mediaElementMovie.Pause();
        }

        private void stopMovie_Click(object sender, RoutedEventArgs e)
        {
            mediaElementMovie.Stop();
        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElementMovie.Volume = (double)volumeSlider.Value;
        }

        private void seekSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElementMovie.Position = TimeSpan.FromSeconds(seekSlider.Value);
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string filename = (string)((DataObject)e.Data).GetFileDropList()[0];
           
            mediaElementMovie.Volume = (double)volumeSlider.Value;
            mediaElementMovie.Play();
        }

        private void mediaElementMovie_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = mediaElementMovie.NaturalDuration.TimeSpan;
            seekSlider.Maximum = ts.TotalSeconds;
            timer.Start();
        }

        private void stopMusic_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
        }
    }
}

