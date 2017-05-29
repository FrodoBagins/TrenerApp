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


         //   recipes.Add(new Recipe("Schabowy", "Mniam mniam pyszne mięsko.", "images/schabowe", "Dania mięsne", 250.00, 2));
         //   recipes.Add(new Recipe("Bigos szlachetny", "Kunszt i tradycja.", "images/bigos", "Dania mięsne", 350.00, 5));
        //    recipes.Add(new Recipe("Ryż z warzywami", "Danie bardzo ostre.", "images/Rice", "Dania wegetariańskie", 250.00, 3));
         //   recipes.Add(new Recipe("Ryba pieczona", "Rybka lubi pływać.", "images/bigos", "Dania wegetariańskie", 250.00, 2));
        //    recipes.Add(new Recipe("Zupa nic", "Staropolska zupa nic.", "images/zupanic", "Zupa", 100.00, 1));
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
