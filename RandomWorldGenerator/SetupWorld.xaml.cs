﻿using System;
using System.Collections.Generic;
using System.Drawing;
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
using Path = System.IO.Path;

namespace RandomWorldGenerator
{
    /// <summary>
    /// Interaction logic for SetupWorld.xaml
    /// </summary>
    public partial class SetupWorld : Window
    {
        private RandomWorldGeneratorWindow generatedWorldWindow;
        public SetupWorld()
        {
            InitializeComponent();
        }

        // Clicking the button will open a new window with a randomly generated map
        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {

            generatedWorldWindow = GenerateMapGrid();

            GenerateEmptyTilesAndCenterPoints();
            FillTilesBasedOnCenterPoints();
            AddRiversToMap();

            generatedWorldWindow.KeyDown += GeneratedWorldWindowOnKeyDown;
            generatedWorldWindow.Show();

        }

        // When 'R' key is pressed in the window of the generated map, another map will be generated
        private void GeneratedWorldWindowOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
               generatedWorldWindow.Close();
               BtnGenerate_Click(sender, e);
            }
                
        }

    }
}
