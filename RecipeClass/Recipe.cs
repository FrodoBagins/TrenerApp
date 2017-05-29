using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeClass
{
    public class Recipe : IDataErrorInfo
    {


        public string title;
        public string description;
        public string imagePath;
        public string category;
        public int calories;
        public int rating;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }


        public string Description
        {
            get { return description; }
            set { description = value; }
        }


        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }

        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        public int Calories
        {
            get { return calories; }
            set { calories = value; }
        }

        public int Rating
        {
            get { return rating; }
            set { rating = value; }
        }


        public Recipe() { }
        public Recipe(string title, string description,string imagePath, string category,int calories, int rating )
        {
            Title = title;
            Description = description;
            ImagePath = imagePath + ".jpg";
            Category = category;
            Calories = calories;
            Rating = rating;
        }






        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
