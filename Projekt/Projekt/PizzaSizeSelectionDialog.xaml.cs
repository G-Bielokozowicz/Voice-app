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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt
{
    /// <summary>
    /// Logika interakcji dla klasy PizzaSizeSelectionDialog.xaml
    /// </summary>
    public partial class PizzaSizeSelectionDialog : Window
    {
        public string SelectedSize { get; private set; }

        public PizzaSizeSelectionDialog(Window owner)
        {
            InitializeComponent();
            Owner = owner; // Ustaw okno nadrzędne
            WindowStartupLocation = WindowStartupLocation.CenterOwner; // Ustaw miejsce pojawiania się
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                SelectedSize = radioButton.Content.ToString();
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
