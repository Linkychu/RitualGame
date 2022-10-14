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

   

    public enum Taste
    {
        Default,
        Sweet,
        Bitter,
        Wet,
        Sour,
        Salty,
        Flavour,
        Spicy   
    };

    public enum TasteResults
    {
        None,
        Correct,
        TooSweet,
        TooBitter,
        TooSalty,
        TooSour,
        TooSpicy,
        TooWatery,
        TooFluffy,
        NotEnoughSugar,
        NotEnoughSalt,
        NotEnoughSour,
        NotEnoughBitter,
        NotEnoughSpice,
        TooDry,
        LacksFlavour,
    };
    

    public class CraftingManager : MonoBehaviour
    {
        private Ray ray;
        public List<Recipe> recipes = new List<Recipe>();
       
        public static CraftingManager instance { get; set; }

        private void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            InventoryManager.instance.CurrentIngredients.Clear();
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

            
            
            InventoryManager.instance.AddItem(currentIngredient.ingredient, serving);
            Debug.ClearDeveloperConsole();

            Debug.Log(currentIngredient.ingredient.Name + ": :" + InventoryManager.instance.CurrentIngredients[currentIngredient.ingredient]);
          
            
        }

        public void Craft()
        {

            bool canCraft;
            List<TasteResults> results = new List<TasteResults>();
            


            List<Recipe> CurrentRecipes = new List<Recipe>();
            CurrentRecipes = recipes;
            
            foreach (var t in CurrentRecipes)
            {
                int count = 0;
                //var RIngredient = t.ingredients.ToList();
                foreach (var ingredients in InventoryManager.instance.CurrentIngredients)
                {
                    Taste taste = ingredients.Key.itemTaste;
                    
                    
                    
                    SIngredient sIngredient = new SIngredient
                    {
                        ingredient = ingredients.Key,
                        serving = ingredients.Value
                    };





                    var result = t.ingredients.First(x => x.ingredient.Name == sIngredient.ingredient.Name);
                    if (!result.Equals(null))
                    {
                        
                        if (result.serving == sIngredient.serving)
                        {
                            Debug.Log($"{sIngredient.ingredient} + {sIngredient.serving} + {result.ingredient} + {result.serving}" );
                            results.Add(TasteResults.Correct);
                            
                        }
                        
                        else if(sIngredient.serving > result.serving)
                        {
                            Debug.Log($"{sIngredient.ingredient} + {sIngredient.serving} + {result.ingredient} + {result.serving}" );
                            switch (taste)
                            {
                                case Taste.Sweet:
                                    results.Add(TasteResults.TooSweet);
                                    break;
                                case Taste.Salty:
                                    results.Add(TasteResults.TooSalty);
                                    break;
                                case Taste.Bitter:
                                    results.Add(TasteResults.TooBitter);
                                    break;
                                case Taste.Sour:
                                    results.Add(TasteResults.TooSour);
                                    break;
                                case Taste.Spicy:
                                    results.Add(TasteResults.TooSpicy);
                                    break;
                                case Taste.Flavour:
                                    results.Add(TasteResults.TooFluffy);
                                    break;
                                case Taste.Wet:
                                    results.Add(TasteResults.TooWatery);
                                    break;
                            }
                            
                            Debug.Log("AddedTooMuch");
                        }

                        else if ((sIngredient.serving < result.serving) && sIngredient.serving > 0)
                        {
                            Debug.Log($"{sIngredient.ingredient} + {sIngredient.serving} + {result.ingredient} + {result.serving}" );
                            switch (taste)
                            {
                                case Taste.Sweet:
                                    results.Add(TasteResults.NotEnoughSugar);
                                    break;
                                case Taste.Salty:
                                    results.Add(TasteResults.NotEnoughSalt);
                                    break;
                                case Taste.Bitter:
                                    results.Add(TasteResults.NotEnoughBitter);
                                    break;
                                case Taste.Sour:
                                    results.Add(TasteResults.NotEnoughSour);
                                    break;
                                case Taste.Spicy:
                                    results.Add(TasteResults.NotEnoughSpice);
                                    break;   
                                case Taste.Flavour:
                                    results.Add(TasteResults.LacksFlavour);
                                    break;
                                case Taste.Wet:
                                    results.Add(TasteResults.TooDry);
                                    break;
                            }
                            
                            Debug.Log("TooLittle");
                        }
                        
                        count++;
                        
                        

                    }
                    else
                    {
                        Debug.Log($"{sIngredient.ingredient}");
                        
                    }

                }

                if (count == t.ingredients.Count)
                {
                    
                    Debug.Log($"You crafted a {t.name}");
                    
                    foreach (var varResult in results)
                    {
                       
                        Debug.Log(varResult);
                    }
                    
                    
                    foreach (var ingredient in InventoryManager.instance.CurrentIngredients)
                    {
                        InventoryManager.instance.SubtractItem(ingredient.Key, ingredient.Value);
                            
                    }
                    
                    
                    
                    
                    break;
                }

                else
                {
                    Debug.Log(t.name);
                }
                
                
            }
            
            /*foreach (var result in results)
            {
                Debug.Log(result);
            }*/
        }
    }
    
}
