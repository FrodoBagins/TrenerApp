using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrenerApp
{
    class HelperClass
    {

        public  HelperClass(){ }


        public double CalculateBMI(double weight, double height)
        {
            double bmi = weight / ((height / 100) * (height / 100));

            return bmi;
        }


    }
}
