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
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
          
            InitializeAvaiblePizzas();
        }

        public void InitializeAvaiblePizzas()
        {
            List<Pizza> pizzas = new List<Pizza>()
            {
                 new Pizza("Margherita", new List<string> { "Sos pomidorowy", "Ser",}, 29.99, 32.99, 37.99),
                 new Pizza("Pepperoni", new List<string> { "Sos pomidorowy", "Ser", "Pepperoni" },32.99, 35.99, 39.99),
                 new Pizza("Kurczak", new List<string> { "Sos pomidorowy", "Ser", "Kurczar", "Kukurydza" },37.99, 42.99, 45.99),
                 new Pizza("Americana", new List<string> { "Sos pomidorowy", "Ser", "Pieczarki","Pomidory" },37.99, 42.99, 45.99),
                 new Pizza("Bekonowa", new List<string> { "Sos pomidorowy", "Ser", "Boczek" },32.99, 35.99, 39.99),
                 new Pizza("Mięsna", new List<string> { "Sos pomidorowy", "Ser", "Pepperoni", "Wędlina","Kiełbasa","Bekon" },42.99, 46.99, 49.99),
                 new Pizza("Grecka", new List<string> { "Sos pomidorowy", "Ser", "Oliwki","Pomidorki koktajlowe" },45.99, 50.99, 55.99),
                 new Pizza("Hawajska", new List<string> { "Sos pomidorowy", "Ser", "Ananas" },37.99, 42.99, 45.99),
                 new Pizza("Europejska", new List<string> { "Sos pomidorowy", "Ser", "Pieczarki", "Szynka","Wołowoina" },53.99, 58.99, 61.99),
                 new Pizza("Farmerska", new List<string> { "Sos pomidorowy", "Ser", "Cebula", "Papryka","Kurczak","Pieczarki" },53.99, 58.99, 61.99),
                 
                // Dodaj inne pizze
            };

            pizzaListBox.ItemsSource = pizzas;
        }
    }
}
