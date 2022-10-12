using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        private List<SIngredient> CurrentIngredients = new List<SIngredient>();
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
                    
                    print(script.ingredient.Name);
                    
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
            CurrentIngredients.Add(currentIngredient);

            
        }

        public void Craft()
        {
            bool craftMatch;
            
            
        }
    }

}
