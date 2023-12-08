using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt
{
    public class OrderedPizza
    {
        private string name;
        private List<string> ingredients;
        private double price;
        private string size;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<string> Ingredients
        {
            get { return ingredients; }
            set { ingredients = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        public string Size
        {
            get { return size; }
            set { size = value; }
        }

        public OrderedPizza(string name, List<string> ingredients, double price, string size)
        {
            this.Name = name;
            this.Ingredients = ingredients;
            this.price = price;
            this.size = size;
        }

        // Funkcja do dodawania składników
        public void AddIngredient(string ingredient)
        {
            Ingredients.Add(ingredient);
        }

        // Funkcja do usuwania składników
        public void RemoveIngredient(string ingredient)
        {
            Ingredients.Remove(ingredient);
        }

        // Funkcja do wyświetlania informacji o pizzy
        public void ShowInfo()
        {
            Console.WriteLine($"Pizza: {name}");
            Console.WriteLine("Składniki:");
            foreach (var ingredient in Ingredients)
            {
                Console.WriteLine($"- {ingredient}");
            }
            Console.WriteLine($"Cena: {price:C}");
            Console.WriteLine($"Rozmiar: {size}");
        }
    }
}
