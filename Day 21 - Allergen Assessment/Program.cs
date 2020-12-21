using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCodeUtilities;

namespace Day_21___Allergen_Assessment
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            List<Food> foods = new List<Food>();
            Dictionary<string, Allergen> allAllergens = new Dictionary<string, Allergen>();
            Dictionary<string, Ingredient> allIngredients = new Dictionary<string, Ingredient>();

            foreach (string foodString in inputList)
                foods.Add(new Food(allAllergens, allIngredients, foodString));

            foreach (var allergenKV in allAllergens)
            {
                Allergen allergen = allergenKV.Value;

                List<Ingredient> intersection = null;

                foreach (Food foodWithThisAllergen in allergen.foodsWithThisAllergen)
                {
                    if (intersection == null)
                        intersection = foodWithThisAllergen.ingredients;
                    else
                    {
                        intersection = intersection.Intersect(foodWithThisAllergen.ingredients).ToList();
                    }
                }

                allergen.possibleIngredients = intersection;
            }

            bool changeMade = true;
            while (changeMade)
            {
                changeMade = false;

                foreach (var allergenKV in allAllergens)
                {
                    Allergen allergen = allergenKV.Value;
                    if (allergen.possibleIngredients.Count == 1 && allergen.knownIngredient == null)
                    {
                        changeMade = true;

                        allergen.knownIngredient = allergen.possibleIngredients[0];
                        allergen.knownIngredient.knownToCauseThisAllergen = allergen;

                        foreach (var otherAllergenKV in allAllergens)
                        {
                            Allergen otherAllergen = otherAllergenKV.Value;
                            if (allergen != otherAllergen)
                                otherAllergen.possibleIngredients.Remove(allergen.knownIngredient);
                        }
                    }
                }
            }

            List<Ingredient> safeIngredients = allIngredients.Values.ToList();
            foreach (var allergenKV in allAllergens)
            {
                Allergen allergen = allergenKV.Value;
                foreach (var possibleIngredient in allergen.possibleIngredients)
                {
                    safeIngredients.Remove(possibleIngredient);
                }
            }

            int numberOfSafeIngredients = 0;
            foreach (Food food in foods)
            {
                foreach (Ingredient ingredient in safeIngredients)
                {
                    if (food.ingredients.Contains(ingredient))
                        numberOfSafeIngredients++;
                }
            }

            Console.WriteLine(numberOfSafeIngredients);

            List<Allergen> allAllergensList = allAllergens.Values.OrderBy(allergen => allergen.name).ToList();
            string canonicalDangerousIngredientList = string.Join(",", allAllergensList.ConvertAll(allergen => allergen.knownIngredient.name));

            Console.WriteLine(canonicalDangerousIngredientList);

            Console.ReadLine();
        }
    }

    class Food
    {
        public List<Allergen> allergens = new List<Allergen>();
        public List<Ingredient> ingredients = new List<Ingredient>();

        public Food(Dictionary<string, Allergen> allAllergens, Dictionary<string, Ingredient> allIngredients, string foodString)
        {
            var foodStringParts = foodString.Substring(0, foodString.Length - 1).Split(new string[] { " (contains " }, StringSplitOptions.RemoveEmptyEntries);
            var ingredientsString = foodStringParts[0];
            var allergensString = foodStringParts[1];
            var ingredientsStringParts = ingredientsString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var allergensStringParts = allergensString.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string ingredientString in ingredientsStringParts)
            {
                Ingredient ingredient;
                if (allIngredients.ContainsKey(ingredientString))
                    ingredient = allIngredients[ingredientString];
                else
                    ingredient = new Ingredient(allIngredients, ingredientString);
                this.ingredients.Add(ingredient);
            }

            foreach (string allergenString in allergensStringParts)
            {
                Allergen allergen;
                if (allAllergens.ContainsKey(allergenString))
                    allergen = allAllergens[allergenString];
                else
                    allergen = new Allergen(allAllergens, allergenString);
                allergen.foodsWithThisAllergen.Add(this);
                this.allergens.Add(allergen);
            }
        }

        public override string ToString()
        {
            return $"{string.Join(" ", this.ingredients)} (contains {string.Join(", ", this.allergens)})";
        }
    }

    class Ingredient
    {
        public string name;
        public Allergen knownToCauseThisAllergen;
        public bool safe = false;

        public Ingredient(Dictionary<string, Ingredient> allIngredients, string name)
        {
            this.name = name;
            allIngredients[name] = this;
        }

        public override string ToString()
        {
            return $"{name}";
        }
    }

    class Allergen
    {
        public string name;
        public List<Food> foodsWithThisAllergen = new List<Food>();
        public List<Ingredient> possibleIngredients = new List<Ingredient>();
        public Ingredient knownIngredient;

        public Allergen(Dictionary<string, Allergen> allAllergens, string name)
        {
            this.name = name;
            allAllergens[name] = this;
        }

        public override string ToString()
        {
            return $"{name}";
        }
    }
}
