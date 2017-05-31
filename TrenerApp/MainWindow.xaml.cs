using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
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
        Person user = new Person();
        Recipe recipe = new Recipe();

        int CaloriesCounter = 0 ;

        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        System.Windows.Threading.DispatcherTimer timer;
        bool isMusicStopped = false;

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
            user = PersonData.Instance.GetPerson(0);

            CaloriesValueDisplay.Content = CaloriesCounter;
            BMI_StatusBar.DataContext = user;
            BMI_Value_Label.DataContext = user;
            BMI_Value_TextBlock.DataContext = user;
            tb_name.DataContext = user;
            tb_surname.DataContext = user;
            tb_age.DataContext = user;
            tb_weight.DataContext = user;
            lb_weight.DataContext = user;
            lb_weight_to_lose.DataContext = user;
            tb_height.DataContext = user;
            lb_BMI.DataContext = user;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            DateTime? date = picker.SelectedDate;
            XElement xelement = XElement.Load("calendar.xml");

            IEnumerable<XElement> calendar = xelement.Elements();
            foreach (var day in calendar)
            {
                ObservableCollection<Recipe> recipesList = new ObservableCollection<Recipe>();
                if (day.Element("day").Value.Equals(date.Value.ToString("yyyy-MM-dd")))
                {
                    foreach (XElement ee in day.Descendants("recipeId"))
                    {
                        string tempValue = ee.Value;
                        recipesList.Add(RecipeData.Instance.GetRecipe(int.Parse(tempValue)));

                    }
                    calendarRecipesList.ItemsSource = recipesList;
                }
            }
        }

        private void BMIUpdateClick(object sender, EventArgs e)
        {
            Person user = new Person();
            user = PersonData.Instance.GetPerson(0);
            BMI_Value_Label.DataContext = user;
            WeightWindow dlg = new WeightWindow();
            dlg.Owner = this;
            dlg.Show();
        }

        private void CategoriesComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();
            data.Add("Kategoria");
            data.Add("Nazwa");
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
                //((recipe as Recipe).rating.IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private bool RecipesFilterByCategory(object recipe)
        {
            var category = CategoriesComboBox.SelectedValue as string;
            // return ((recipe as Recipe).category.IndexOf(category, StringComparison.OrdinalIgnoreCase) >= 0);

            // searchTextBox.Text = CategoriesComboBox.SelectedValue.ToString();
            if (string.IsNullOrEmpty(searchTextBox.Text))
            {
                return true;
            }
            else
            {
                return false;
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
            user = PersonData.Instance.GetPerson(0);

            user.Name = tb_name.Text;
            user.SecondName = tb_surname.Text;
            user.Age = Int32.Parse(tb_age.Text);
            try
            {
                user.Height = Double.Parse(tb_height.Text);
            }
            catch
            {
                MessageBoxResult result = MessageBox
                    .Show("Wprowadź poprawny wzrost", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                isValid = false;
            }

            try
            {
                user.Weight = Double.Parse(tb_weight.Text);
                user.WeightLeft = user.WeightToLose - (user.Weight - user.WeightToLose);
                BMI_StatusBar.Value = user.WeightLeft;
            }
            catch
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
                body += "<br/><br/>";
                foreach (var item in recipesList)
                {
                    body += "<div style='float: left; width: 100%; border: 1px solid black; margin-bottom: 10px; padding-top: 5px; padding-bottom: 25px; padding-left: 30px;'>";
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
            //   Name = "searchRecips"
            XElement xelement = XElement.Load("calendar.xml");

            DateTime? date = DateTime.Now;

            string data = date.Value.ToString("yyyy-MM-dd");

            int recipeIdValue = searchRecips.SelectedIndex;

            bool IsDateEmpty = true;

            Console.WriteLine(recipeIdValue);



            xelement.Save("calendar.xml");

            Console.Write(xelement);

            IEnumerable<XElement> calendar = xelement.Elements();
            // Read the entire XML
            foreach (var day in calendar)
            {
                if (day.Element("day").Value.Equals(data))
                {
                    IsDateEmpty = false;
                    Console.WriteLine("Kurwa jego mac");
                    day.Element("recipes").Add(new XElement("recipeId", recipeIdValue));
                    xelement.Save("calendar.xml");

                    CaloriesCounter = CaloriesCounter + RecipeData.Instance.GetRecipe(recipeIdValue).calories;
                    CaloriesValueDisplay.Content = CaloriesCounter;

                }
                else
                {

                }
            }

            if (IsDateEmpty == true)
            {
                xelement.AddFirst(new XElement("calendar",
                new XElement("day", data),
                new XElement("recipes", new XElement("recipeId", recipeIdValue))));
                xelement.Save("calendar.xml");

                CaloriesCounter = CaloriesCounter + RecipeData.Instance.GetRecipe(recipeIdValue).calories;
                CaloriesValueDisplay.Content = CaloriesCounter;

            }
        }

        void timer_Tick(object sender, EventArgs e)
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
            if(isMusicStopped == false)
            {
                player.Stop();
                isMusicStopped = true;
                stopMuxsic.Content = "MUSIC";
            }
            else
            {
                player.Play();
                isMusicStopped = false;
                stopMuxsic.Content = "MUTE";
            }
        }

        private void WegeCategory_Click(object sender, RoutedEventArgs e)
        {
            SetTabItemIndex();
            searchTextBox.Text = "Wegetariańskie";
        }

        private void RiceCategory_Click(object sender, RoutedEventArgs e)
        {
            SetTabItemIndex();
            searchTextBox.Text = "Dania z Ryżem";
        }

        private void FruitCategory_Click(object sender, RoutedEventArgs e)
        {
            SetTabItemIndex();
            searchTextBox.Text = "Owoce";
        }

        private void VegetableCategory_Click(object sender, RoutedEventArgs e)
        {
            SetTabItemIndex();
            searchTextBox.Text = "Warzywa";
        }

        private void FishCategory_Click(object sender, RoutedEventArgs e)
        {
            SetTabItemIndex();
            searchTextBox.Text = "Dania z Rybami";
        }

        private void SmoothieCategory_Click(object sender, RoutedEventArgs e)
        {
            SetTabItemIndex();
            searchTextBox.Text = "Smoothie";
        }

        private void BreakfastCategory_Click(object sender, RoutedEventArgs e)
        {
            SetTabItemIndex();
            searchTextBox.Text = "Śniadania";
        }

        private void SetTabItemIndex()
        {
            myTabControl.SelectedIndex = 3;
            CategoriesComboBox.SelectedIndex = 0;
        }


        private void lb_weight_to_lose_TextChanged(object sender, TextChangedEventArgs e)
        {
            user = PersonData.Instance.GetPerson(0);

            string weighttolose = lb_weight_to_lose.Text;

            double weighttolosenumber = double.Parse(weighttolose);

            double weight = user.Weight;


            user.WeightLeft = weight - weighttolosenumber;
            user.WeightLeft2 = (weight - weighttolosenumber) - (weight - weighttolosenumber);

            BMI_StatusBar.Maximum = user.WeightLeft;
            BMI_StatusBar.Value = user.WeightLeft2;


        }
    }
}
