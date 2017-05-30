using System.Windows.Input;

namespace TrenerApp
{
    class BmiCommands
    {
        private static RoutedUICommand countBMI;

        static BmiCommands()
        {
            countBMI = new RoutedUICommand("Count BMI", "countBMI", typeof(BmiCommands));
        }

        public static RoutedUICommand CountBMI { get { return countBMI; } }
    }
}
