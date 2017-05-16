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
        private double height;
        private double bmi;
        private int age;


        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("NameAndEmail"); }
        }

        public string SecondName
        {
            get { return secondname; }
            set { secondname = value; OnPropertyChanged("NameAndEmail"); }
        }

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged("NameAndEmail"); }
        }

        public double Weight
        {
            get { return weight; }
            set { weight = value; OnPropertyChanged("NameAndEmail"); }
        }

        public double WeightToLose
        {
            get { return weight_lose; }
            set { weight_lose = value; OnPropertyChanged("NameAndEmail"); }
        }

        public double WeightLeft
        {
            get { return weight_left; }
            set { weight_left = value; OnPropertyChanged("NameAndEmail"); }
        }

        public double Height
        {
            get { return height; }
            set { height = value; OnPropertyChanged("NameAndEmail"); }
        }

        public double BMI
        {
            get { return bmi; }
            set { bmi = value; OnPropertyChanged("NameAndEmail"); }
        }

        public int Age
        {
            get { return age; }
            set { age = value; OnPropertyChanged("NameAndEmail"); }
        }

        public Person() { }
        public Person(string name, string secondname, string email, double weight,double weight_lose, double height, int age)
        {
            Name = name;
            SecondName = secondname;
            Email = email;
            Weight = weight;
            WeightToLose = weight_lose;
            Height = height;
            WeightLeft = weight_lose - (weight - weight_lose);
            BMI = weight / ((height / 100) * (height / 100));
            Age = age;
        }

        public string NameAndEmail
        {
            get
            {
                return Name + " " + SecondName;
            }
        }


        public override string ToString()
        {
            return Name + " " + SecondName + " (" + Email + ")";
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
