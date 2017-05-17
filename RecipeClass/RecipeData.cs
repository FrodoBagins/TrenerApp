using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeClass
{
    public class RecipeData
    {
        private Collection<Recipe> recipes = new ObservableCollection<Recipe>();

        private RecipeData()
        {
            recipes.Add(new Recipe("Schabowy", "Mniam mniam pyszne mięsko.", "images/schabowe", "Dania mięsne", 250.00, 2));
            recipes.Add(new Recipe("Bigos szlachetny", "Kunszt i tradycja.", "images/bigos", "Dania mięsne", 350.00, 5));
            recipes.Add(new Recipe("Ryż z warzywami", "Danie bardzo ostre.", "images/Rice", "Dania wegetariańskie", 250.00, 3));
            recipes.Add(new Recipe("Ryba pieczona", "Rybka lubi pływać.", "images/bigos", "Dania wegetariańskie", 250.00, 2));
            recipes.Add(new Recipe("Zupa nic", "Staropolska zupa nic.", "images/zupanic", "Zupa", 100.00, 1));
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
