using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Extensions;


namespace cookingData
{
    [System.Serializable]
    public struct SIngredient
    {
        public Ingredient ingredient;
        public int serving;
    }

    public class CraftingManager : MonoBehaviour
    {
        private Ray ray;
        public List<Recipe> recipes = new List<Recipe>();
        private Dictionary<Ingredient, int> currentIngredients = new Dictionary<Ingredient, int>();
        public static CraftingManager instance { get; set; }

        private void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;
            //sets the ray position to the mouse position
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                //tries to get the component, instead of get component to prevent memory and console errors
                if (hit.transform.TryGetComponent(out IngredientScript script))
                {
                    
                   // print(script.ingredient.Name);
                    
                    //if Left mouse button is clicked
                    if (Input.GetMouseButtonDown(0))
                    {
                        
                        //Passes the ingredient and serving values into the UseItem function
                        UseItem(script.ingredient, script.ingredient.AmountPerServing);
                    }
                }

            }


        }

        void UseItem(Ingredient ingredient, int serving)
        {
            //creates a temporary SIngredient data to hold in our inputs
            SIngredient currentIngredient = new SIngredient
            {
                //sets the SIngredient ingredients and servings to our inputted ones. This is so we can convert our inputs to SIngredient structs
                ingredient = ingredient,
                serving = serving
            };
            
            //adds it to our current item list

            if (currentIngredients.ContainsKey(currentIngredient.ingredient))
            {
                int value = currentIngredients[currentIngredient.ingredient];

                value += serving;

                currentIngredients[currentIngredient.ingredient] = value;
            }

            else
            {
                 currentIngredients.Add(currentIngredient.ingredient, currentIngredient.serving);
            }
            
            Debug.ClearDeveloperConsole();

            foreach (var variIngredient in currentIngredients)
            {
                Debug.Log($"{variIngredient.Key} + {variIngredient.Value}" );
            }
          
            
        }

        public void Craft()
        {
            bool craftMatch;
            int count = 0;
            foreach (var t in recipes)
            {
                foreach (var ingredients in currentIngredients)
                {
                    SIngredient sIngredient = new SIngredient
                    {
                        ingredient = ingredients.Key,
                        serving = ingredients.Value
                    };

                    if (t.ingredients.Contains(sIngredient))
                    {
                        count++;
                        continue;
                        
                        
                    }
                    else
                    {
                        Debug.Log("You failed to Craft this item");
                        return;
                    }

                }

                if (count == t.ingredients.Count)
                {
                    Debug.Log($"You crafted {t.name}");
                }

                else
                {
                    Debug.Log("You failed to Craft Anything");
                }

                return;
            }
        }
    }
    
}
