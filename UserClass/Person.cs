using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UserClass
{
    public class Person
    {
        private string name;
        private string secondname;
        private string email;
        private double weight;
        private double weight_lose;
        private double weight_left;
        private double weight_left2;
        private double height;
        private double bmi;
        private int age;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string SecondName
        {
            get { return secondname; }
            set { secondname = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public double Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public double WeightToLose
        {
            get { return weight_lose; }
            set { weight_lose = value; }
        }

        public double WeightLeft
        {
            get { return weight_left; }
            set { weight_left = value; }
        }

        public double WeightLeft2
        {
            get { return weight_left2; }
            set { weight_left2 = value; }
        }

        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        public double BMI
        {
            get { return bmi; }
            set { bmi = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public Person() { }
        public Person(string name, string secondname, string email, double weight, double weight_lose, double height, int age)
        {
            Name = name;
            SecondName = secondname;
            Email = email;
            Weight = weight;
            WeightToLose = weight_lose;
            Height = height;
            WeightLeft = weight - weight_lose;
            WeightLeft2 = (weight - weight_lose) - (weight - weight_lose);
            BMI = weight / ((height / 100) * (height / 100));
            Age = age;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
