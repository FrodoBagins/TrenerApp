using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeClass
{ 
    public class Category: IDataErrorInfo
    {
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Category() { }
        public Category(string name)
        {
            Name = name;
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
