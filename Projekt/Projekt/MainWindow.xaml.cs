using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            InitializeAvaiblePizzas();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyNmae = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyNmae));
        }
        private double totalPrice;

        public double TotalPrice
        {
            get { return totalPrice; }
            set
            {
                totalPrice = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<OrderedPizza> orderedPizzas;
        public ObservableCollection<OrderedPizza> OrderedPizzas
        {
            get { return orderedPizzas; }
            set
            {
                orderedPizzas = value;
            }
        }

        private List<Pizza> pizzas;
        private List<Pizza> GetPizzaList()
        {
            return pizzas;
        }

      


        public void InitializeAvaiblePizzas()
        {
            pizzas = new List<Pizza>()
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
           
            pizzaListBox.ItemsSource = GetPizzaList();
            orderedPizzaListBox.ItemsSource = OrderedPizzas;

            orderedPizzas = new ObservableCollection<OrderedPizza>();
        }

        private void txtPizzaNameSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtPizzaNameSearch.Text.ToLower();

            txtPizzaIngredientSearch.TextChanged -= txtPizzaIngredientSearch_TextChanged;
            txtPizzaIngredientSearch.Text = "";
            txtPizzaIngredientSearch.TextChanged += txtPizzaIngredientSearch_TextChanged;

            // Sprawdź, czy GetPizzaList() nie zwraca null
            List<Pizza> pizzasList = GetPizzaList();
            if (pizzasList != null)
            {
                // Jeśli tekst jest pusty, pokaż wszystkie pizze, w przeciwnym razie, filtrowanie
                pizzaListBox.ItemsSource = string.IsNullOrEmpty(searchText)
                    ? pizzasList
                    : pizzasList.Where(pizza => pizza.Name.ToLower().Contains(searchText)).ToList();

            }
            else
            {
                // Obsłuż sytuację, gdy GetPizzaList() zwraca null
                // Możesz dodać odpowiednią obsługę błędów lub dostarczyć domyślną listę pizz
                pizzaListBox.ItemsSource = new List<Pizza>();
            }
        }

        private void txtPizzaIngredientSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtPizzaIngredientSearch.Text.ToLower();

            txtPizzaNameSearch.TextChanged -= txtPizzaNameSearch_TextChanged;
            txtPizzaNameSearch.Text = "";
            txtPizzaNameSearch.TextChanged += txtPizzaNameSearch_TextChanged;

            // Sprawdź, czy GetPizzaList() nie zwraca null
            List<Pizza> pizzasList = GetPizzaList();
            if (pizzasList != null)
            {
                // Jeśli tekst jest pusty, pokaż wszystkie pizze
                if (string.IsNullOrEmpty(searchText))
                {
                    pizzaListBox.ItemsSource = pizzasList;
                }
                else
                {
                    // Rozdziel wprowadzone składniki
                    string[] ingredients = searchText.Split(' ');

                    // Filtruj pizze, uwzględniając dowolne dopasowanie składników
                    pizzaListBox.ItemsSource = pizzasList
                        .Where(pizza => pizza.Ingredients.Any(ingredient => ingredients.Contains(ingredient.ToLower())))
                        .ToList();
                }
            }
            else
            {
                // Obsłuż sytuację, gdy GetPizzaList() zwraca null
                // Możesz dodać odpowiednią obsługę błędów lub dostarczyć domyślną listę pizz
                pizzaListBox.ItemsSource = new List<Pizza>();
            }
        }

        private void orderButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void pizzaListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is Pizza selectedPizza)
            {
                // Dodaj wybraną pizzę do orderedPizzaListBox
                AddPizzaToOrderedListBox(selectedPizza);
                listBox.SelectedItem = null;
            }
       
        }

        private void AddPizzaToOrderedListBox(Pizza pizza)
        {
            // Dodaj wybraną pizzę do orderedPizzaListBox
            if (pizza != null)
            {
             
                PizzaSizeSelectionDialog sizeDialog = new PizzaSizeSelectionDialog(this);

                // Pokaż okno dialogowe
                if (sizeDialog.ShowDialog() == true)
                {
                    // Pobierz wybrany rozmiar i dodaj pizzę z rozmiarem do listy zamówień

                    OrderedPizza ordPizza = new OrderedPizza(pizza.Name, pizza.Ingredients, pizza.GetPriceBySize(sizeDialog.SelectedSize), sizeDialog.SelectedSize);

                 
                    orderedPizzas.Add(ordPizza);
                    orderedPizzaListBox.ItemsSource = orderedPizzas;
                    UpdateTotalPrice();
                }
 
            }
        }

        private void orderedPizzaListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is OrderedPizza selectedPizza)
            {
                // Usuń wybraną pizzę z orderedPizzaListBox
                RemovePizzaFromOrderedListBox(selectedPizza);
               
            }
        }

        private void RemovePizzaFromOrderedListBox(OrderedPizza pizzaToRemove)
        { // Dodaj wybraną pizzę do orderedPizzaListBox
            if (pizzaToRemove != null)
            {
                // Dodaj wybraną pizzę do orderedPizzaListBox
                orderedPizzas.Remove(pizzaToRemove);
                orderedPizzaListBox.ItemsSource = orderedPizzas;
                UpdateTotalPrice();
            }
        }

        private void UpdateTotalPrice()
        {
            TotalPrice = orderedPizzas.Sum(pizza => pizza.Price); // Przykładowo, możesz zmienić to na odpowiednie obliczenia
            Console.WriteLine("Cena:" + TotalPrice);
        }
    }


}

