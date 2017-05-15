using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserClass
{
    public class PersonData
    {

        private Collection<Person> data = new ObservableCollection<Person>();

        private PersonData()
        {
            data.Add(new Person("Jan", "Kowalski", "jankowalski@gmail.com", 100.0, 195.5));    
        }


        private static PersonData singleton = null;
        public static PersonData Instance
        {
            get
            {
                if (singleton == null)
                    singleton = new PersonData();
                return singleton;
            }
        }

        public Person GetPerson(int id)
        {
            return data[id];
        }

        public Collection<Person> Persons
        {
            get
            {
                return data;
            }
        }
    }
}
