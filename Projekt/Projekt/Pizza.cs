using System;
using System.Collections.Generic;

namespace Projekt
{
    public class Pizza
    {
        private string name;
        private List<string> ingredients;
        private double priceSmall;
        private double priceMedium;
        private double priceBig;

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

      
        public double PriceSmall
        {
            get { return priceSmall; }
            set { priceSmall = value; }
        }

        public double PriceMedium
        {
            get { return priceMedium; }
            set { priceMedium = value; }
        }

        public double PriceBig
        {
            get { return priceBig; }
            set { priceBig = value; }
        }

        public Pizza(string name, List<string> ingredients, double priceSmall, double priceMedium, double priceBig)
        {
            this.Name = name;
            this.Ingredients = ingredients;
            this.PriceSmall = priceSmall;
            this.PriceMedium = priceMedium;
            this.PriceBig = priceBig;
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
            Console.WriteLine($"Cena (mała): {priceSmall:C}");
            Console.WriteLine($"Cena (średnia): {priceMedium:C}");
            Console.WriteLine($"Cena (duża): {priceBig:C}");
        }
    }
}
