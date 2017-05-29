using System;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RecipeClass
{
    public class RecipeData
    {
        private Collection<Recipe> recipes = new ObservableCollection<Recipe>();

        XmlTextReader reader = new XmlTextReader("recipes.xml");

        XElement xelement = XElement.Load("recipes.xml");

        private RecipeData()
        {
            RecipeReader();

        }



        private void RecipeReader()
        {
            IEnumerable<XElement> recipesData = xelement.Elements();

            string title;
            string description;
            string imagePath;
            string category;
            string calories;
            string rating;


            foreach (var recipe in recipesData)
            {
                Console.WriteLine(recipe.Element("title").Value);

                title = recipe.Element("title").Value;
                description = recipe.Element("description").Value;
                imagePath = recipe.Element("imagePath").Value;
                category = recipe.Element("category").Value;
                calories = recipe.Element("calories").Value;
                rating = recipe.Element("rating").Value;



                int rating2 = int.Parse(rating);

                int calories2 = int.Parse(calories);

                recipes.Add(new RecipeClass.Recipe(title, description, imagePath, category, calories2, rating2));

            }
        }





        private static RecipeData singleton = null;

        public static RecipeData Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new RecipeData();
                return singleton;
            }
        }

        public Recipe GetRecipe(int id)
        {
            return recipes[id];
        }


        public Collection<Recipe> Recipes
        {
            get
            {
                return recipes;
            }
        }

    }
}
