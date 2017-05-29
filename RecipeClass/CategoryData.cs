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
    public class CategoryData
    {
        private Collection<Category> categories = new ObservableCollection<Category>();

        XmlTextReader reader = new XmlTextReader("categories.xml");

        XElement xelement = XElement.Load("categories.xml");

        private CategoryData()
        {
            CategoryReader();
        }

        private void CategoryReader()
        {
            IEnumerable<XElement> categoriesData = xelement.Elements();

            string name;

            foreach (var category in categoriesData)
            {
                Console.WriteLine(category.Element("name").Value);

                name = category.Element("name").Value;

                categories.Add(new RecipeClass.Category(name));
            }
        }

        private static CategoryData singleton = null;

        public static CategoryData Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new CategoryData();
                return singleton;
            }
        }

        public Category GetCategory(int id)
        {
            return categories[id];
        }


        public Collection<Category> Categories
        {
            get
            {
                return categories;
            }
        }
    }
}
