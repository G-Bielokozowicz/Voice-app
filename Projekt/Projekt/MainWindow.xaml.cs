using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
            Loaded += MainWindow_Loaded;
           
        }


        private SpeechRecognitionEngine speechRecognitionEngine;
        private SpeechSynthesizer speechSynthesiserEngine = new SpeechSynthesizer();
        PizzaSizeSelectionDialog sizeDialog;


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            sizeDialog = new PizzaSizeSelectionDialog(this);
            // Inicjalizacja silnika rozpoznawania mowy
            speechRecognitionEngine = new SpeechRecognitionEngine(new CultureInfo("pl-PL"));

            // Ścieżka do pliku zawierającego gramatykę SRGS
            string grammarFilePath = "../../../grammar.xml";

            // Wczytaj gramatykę z pliku
            Grammar grammar = new Grammar(grammarFilePath);

            // Dodaj gramatykę do silnika rozpoznawania mowy
            speechRecognitionEngine.LoadGrammar(grammar);

            // Ustaw obsługę zdarzenia rozpoznawania
            speechRecognitionEngine.SpeechRecognized += SpeechRecognitionEngine_SpeechRecognized;
            
            // Start silnika rozpoznawania mowy
            speechRecognitionEngine.SetInputToDefaultAudioDevice();
            speechRecognitionEngine.RecognizeAsync(RecognizeMode.Multiple);

            speechSynthesiserEngine.SetOutputToDefaultAudioDevice();
            speechSynthesiserEngine.SpeakAsync("Witam w pizzeri");

        }

        private void SpeechRecognitionEngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Obsługa zdarzenia rozpoznania mowy
            string recognizedText = e.Result.Text;
            SemanticValue semantics = e.Result.Semantics;
            float confidence = e.Result.Confidence;
            Console.WriteLine(recognizedText + " ,confidence:" + confidence);
            if (confidence < 0.60)
            {
                speechSynthesiserEngine.SpeakAsync("Proszę powtórzyć");
                return;
            }

            if (semantics.ContainsKey("Podsumowanie"))
            {
                SummaryOrderSpeak();
                return;
            }

            if (semantics.ContainsKey("Pomoc"))
            {
                speechSynthesiserEngine.SpeakAsync("Powiedz jaką pizzę chcesz zamówić wraz z rozmiarem. Na przykład Mała Hawajska. Możesz usunąć pizze z zamówienia mówiąc usuń i numer indeksu");
                return;
            }

            if (semantics.ContainsKey("Usun"))
            {
                if (semantics.ContainsKey("Numer"))
                {
                    int chosenNumer = int.Parse(semantics["Numer"].Value.ToString());
                    OrderedPizza pizzaToRemove = orderedPizzas.FirstOrDefault(pizza => pizza.Index == chosenNumer);
                    if (pizzaToRemove != null)
                    {
                        RemovePizzaFromOrderedListBox(pizzaToRemove);
                        Console.WriteLine($"Pizza o indeksie {chosenNumer} została usunięta.");
                    } else
                    {
                        speechSynthesiserEngine.SpeakAsync("Nie ma tej pozycji w zamówieniu.");
                    }

                }
                else
                {
                    speechSynthesiserEngine.SpeakAsync("Proszę powiedzieć którą pozycję usunąć z zamówienia");
                }
                return;
            }

            if (semantics.ContainsKey("Wyczysc"))
            {
                txtPizzaIngredientSearch.Text = "";
                txtPizzaNameSearch.Text = "";
                return;
            }

            if (semantics.ContainsKey("Szukaj"))
            {
                if (semantics.ContainsKey("Skladnik"))
                {
                    string chosenIngredient = semantics["Skladnik"].Value.ToString();
                    txtPizzaIngredientSearch.Text = chosenIngredient;
                }
                else if (semantics.ContainsKey("Pizza"))
                {
                    string chosenPizza = semantics["Pizza"].Value.ToString();
                    txtPizzaNameSearch.Text = chosenPizza;
                }

                return;
            }

            if (semantics.ContainsKey("Rozmiar") && (semantics.ContainsKey("Pizza"))) // powiedziano rozmiar i pizze, np. duza pepperoni - dodaj do zamowienia
            {
                string chosenPizza = semantics["Pizza"].Value.ToString();
                string chosenSize = semantics["Rozmiar"].Value.ToString();
                AddPizzaWithSizeToOrderedListBoxVoice(chosenPizza, chosenSize); 
                return;
            }

            if (semantics.ContainsKey("Rozmiar")) // powiedziano tylko rozmiar, dziala wylacznie w przypadku gdy wpierw wybrana zostanie pizza z gui
            {
                string chosenSize = semantics["Rozmiar"].Value.ToString();
                
                if (sizeDialog.IsVisible)
                {
                   
                    switch (chosenSize)
                    {
                        case "Mała":
                            sizeDialog.MalaOption.IsChecked = true;
                            break;
                        case "Średnia":
                            sizeDialog.SredniaOption.IsChecked = true;
                            break;
                        case "Duża":
                            sizeDialog.DuzaOption.IsChecked = true;
                            break;
                        default:
                            sizeDialog.DuzaOption.IsChecked = true;
                            break;
                    }
                      
                    sizeDialog.DialogResult = true;
                    sizeDialog.Close();
                    
                } 
                else
                {
                   
                }
                return;
            }

            if (semantics.ContainsKey("Pizza")) // powiedziano tylko pizze, poniformuj, ze trzeba glosem wybrac odrazu pizze i rozmiar
            {
                //string chosenPizza = semantics["Pizza"].Value.ToString();

                //AddPizzaToOrderedListBoxVoice(chosenPizza);

                speechSynthesiserEngine.SpeakAsync("Proszę wybrać rozmiar razem z wybraną piccą");
                return;
            }

            if (semantics.ContainsKey("Wyjdz")) // jak sprawdzić jaki tag wybrano
            {
                speechSynthesiserEngine.Speak("Wychodzę z programu");
                Application.Current.Shutdown();
            }
        }
      

        public event PropertyChangedEventHandler PropertyChanged;
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

        public List<Pizza> Pizzas { get; set; }
        public List<Pizza> GetPizzaList()
        {
            return Pizzas;
        }

        


        public void InitializeAvaiblePizzas()
        {
            Pizzas = new List<Pizza>()
            {
                 new Pizza("Margherita", new List<string> { "Sos pomidorowy", "Ser",}, 29.99, 32.99, 37.99),
                 new Pizza("Pepperoni", new List<string> { "Sos pomidorowy", "Ser", "Pepperoni" },32.99, 35.99, 39.99),
                 new Pizza("Kurczakowa", new List<string> { "Sos pomidorowy", "Ser", "Kurczak", "Kukurydza" },37.99, 42.99, 45.99),
                 new Pizza("Americana", new List<string> { "Sos pomidorowy", "Ser", "Pieczarki","Pomidory" },37.99, 42.99, 45.99),
                 new Pizza("Bekonowa", new List<string> { "Sos pomidorowy", "Ser", "Boczek" },32.99, 35.99, 39.99),
                 new Pizza("Mięsna", new List<string> { "Sos pomidorowy", "Ser", "Pepperoni", "Wędlina","Kiełbasa","Bekon" },42.99, 46.99, 49.99),
                 new Pizza("Grecka", new List<string> { "Sos pomidorowy", "Ser", "Oliwki"},45.99, 50.99, 55.99),
                 new Pizza("Hawajska", new List<string> { "Sos pomidorowy", "Ser", "Ananas" },37.99, 42.99, 45.99),
                 new Pizza("Europejska", new List<string> { "Sos pomidorowy", "Ser", "Pieczarki", "Szynka","Wołowina" },53.99, 58.99, 61.99),
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
                sizeDialog = new PizzaSizeSelectionDialog(this);
                // Pokaż okno dialogowe
                if (sizeDialog.ShowDialog() == true)
                {
                    // Pobierz wybrany rozmiar i dodaj pizzę z rozmiarem do listy zamówień

                    OrderedPizza ordPizza = new OrderedPizza(pizza.Name, pizza.Ingredients, pizza.GetPriceBySize(sizeDialog.SelectedSize), sizeDialog.SelectedSize);

                    orderedPizzas.Add(ordPizza);
                    for (int i = 0; i < orderedPizzas.Count; i++)
                    {
                        orderedPizzas[i].Index = i + 1;
                    }
                    Console.WriteLine(orderedPizzas.IndexOf(ordPizza));
                    orderedPizzaListBox.ItemsSource = orderedPizzas;
                    UpdateTotalPrice();
                }
 
            }
        }

        //private void AddPizzaToOrderedListBoxVoice(string pizzaName)
        //{

        //    Pizza pizza = findPizza(pizzaName);
        //    // Dodaj wybraną pizzę do orderedPizzaListBox
        //    if (pizza != null)
        //    {
        //        sizeDialog = new PizzaSizeSelectionDialog(this);
        //        // Pokaż okno dialogowe
        //        if (sizeDialog.ShowDialog() == true)
        //        {
        //            // Pobierz wybrany rozmiar i dodaj pizzę z rozmiarem do listy zamówień

        //            OrderedPizza ordPizza = new OrderedPizza(pizza.Name, pizza.Ingredients, pizza.GetPriceBySize(sizeDialog.SelectedSize), sizeDialog.SelectedSize);

        //            orderedPizzas.Add(ordPizza);
                  
        //            orderedPizzaListBox.ItemsSource = orderedPizzas;
        //            UpdateTotalPrice();
        //        }

        //    }
        //}

        private void AddPizzaWithSizeToOrderedListBoxVoice(string pizzaName, string size)
        {

            Pizza pizza = findPizza(pizzaName);
            Console.WriteLine(pizza.ToString() + " " + size);
            // Dodaj wybraną pizzę do orderedPizzaListBox
            if (pizza != null)
            {

                OrderedPizza ordPizza = new OrderedPizza(pizza.Name, pizza.Ingredients, pizza.GetPriceBySize(size), size);

                orderedPizzas.Add(ordPizza);
                for (int i = 0; i < orderedPizzas.Count; i++)
                {
                    orderedPizzas[i].Index = i + 1;
                }
                orderedPizzaListBox.ItemsSource = orderedPizzas;
                UpdateTotalPrice();
                

            }
        }

        private Pizza findPizza(string name)
        {
            foreach (Pizza pizza in Pizzas)
            {
                if (pizza.Name == name) return pizza;
            }
            return null;
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
                for (int i = 0; i < orderedPizzas.Count; i++)
                {
                    orderedPizzas[i].Index = i + 1;

                }
                orderedPizzaListBox.ItemsSource = null;
                orderedPizzaListBox.ItemsSource = orderedPizzas;
                UpdateTotalPrice();
            }
        }

        private void UpdateTotalPrice() // aktualizuje cene i indeksy zamowionych pizz
        {
            
            TotalPrice = orderedPizzas.Sum(pizza => pizza.Price); 
            
        }


        private void SummaryOrderSpeak()
        {
            foreach (OrderedPizza ordPizza in orderedPizzas)
            {
                string size = ordPizza.Size.ToString();
                string name= ordPizza.Name.ToString();
                string speak = size + name;
                speechSynthesiserEngine.SpeakAsync(speak);
            }
            
        }
    }


}

