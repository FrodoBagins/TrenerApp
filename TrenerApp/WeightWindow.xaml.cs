﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TrenerApp
{
    /// <summary>
    /// Interaction logic for WeightWindow.xaml
    /// </summary>
    public partial class WeightWindow : Window
    {
        public WeightWindow()
        {
            InitializeComponent();
        }


        private void Accept(object sender, EventArgs e)
        {

            Close();
        }

        private void Delete(object sender, EventArgs e)
        {

            Close();
        }



    }
}
