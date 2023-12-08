using System;
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

        public PizzaSizeSelectionDialog(List<string> pizzaSizes)
        {
            InitializeComponent();
            InitializeComboBox(pizzaSizes);
        }

        private void InitializeComboBox(List<string> pizzaSizes)
        {
            cmbPizzaSize.ItemsSource = pizzaSizes;
            cmbPizzaSize.SelectedIndex = 0; // Domyślnie wybierz pierwszy rozmiar
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            // Zapisz wybrany rozmiar i zamknij okno
            SelectedSize = cmbPizzaSize.SelectedItem as string;
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Anuluj i zamknij okno
            DialogResult = false;
            Close();
        }
    }
}
