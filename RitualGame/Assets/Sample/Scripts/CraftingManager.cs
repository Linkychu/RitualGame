using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Extensions;
using TMPro;
using UnityEngine.UI;


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
        WrongIngredient
    };

    

    public class CraftingManager : MonoBehaviour
    {
        private Ray ray;

        public GameObject itemBox;
        public List<Recipe> recipes = new List<Recipe>();
       
        public static CraftingManager instance { get; set; }
        
        
        [SerializeField]private Texture emptyImage;

        private bool isFrozen;

        private bool perfectBueno;

        private int perfectCount;

       


        private void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            isFrozen = false;
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
                    if (Input.GetMouseButtonDown(0) && !isFrozen)
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

            
            //rounds it up
            InventoryManager.instance.AddItem(currentIngredient.ingredient, Mathf.RoundToInt(serving * GameManager.instance.multiplier));
            
            
            
        }

        public void Craft()
        {

            bool canCraft;
            List<TasteResults> results = new List<TasteResults>();
            
            



            var t = recipes[0];
            
            
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



                  
                    SIngredient result;
                    
                    //checks the first item that matches the name of the ingredient and recipe ingredient
                    result = t.ingredients.FirstOrDefault(x => x.ingredient.Name == sIngredient.ingredient.Name);

                    //makes sure there's a result
                        if (!result.Equals(null))
                    {
                        
                        
                        if (result.serving == Mathf.RoundToInt(sIngredient.serving))
                        {
                            //checks how many perfect ingredients we got (one's that match)
                           
                            perfectCount++;
                        }
                        
                        if(sIngredient.serving > Mathf.RoundToInt(result.serving))
                        {
                            //checks to see if the amount in the inventory is greater than the one the recipe, if so, based on the ingredients flavour it will check and see if it's too spicy/too sweet, etc
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

                        //same thing as above, but checks if it's below instead and makes sure the ingredient isn't below 0
                        else if ((sIngredient.serving < Mathf.RoundToInt(result.serving)) && sIngredient.serving > 0 && result.serving > 0)
                        {
                            
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
                            
                            
                            GameManager.instance.AddScore(25);
                            Debug.Log("TooLittle");
                        }
                        
                        else
                        {
                            results.Add(TasteResults.WrongIngredient);
                            
                            
                        }
                        
                        count++;
                        
                        

                    }

                    

                }


                //we need a dictionary to check how many results for each taste
                Dictionary<TasteResults, int> resultsMap = new Dictionary<TasteResults, int>();
                
                //new string list to print out our top 3 results
                List<string> outputResults = new List<string>(3);
                
                //creates a range of 3 empty strings 
                outputResults.AddRange(new string[3]);
                Texture image;
                
                //just makes sure we have the right ingredients
                if (count == t.ingredients.Count)
                {
                    
                  
                   //sets image texture to be that of the recipe's
                    image = t.icon.texture;
                    

                    //checks if we have a perfect recipe by comparing our perfect scores
                    if (perfectCount == results.Count)
                    {
                            perfectBueno = true;    
                        
                    }
                    
                    
                    for (int i = 0; i < results.Count; i++)
                    {
                        //checks if the dictionary has a key with our results name, if so it changes the value instead to prevent duplicates
                        if (!resultsMap.ContainsKey(results[i]))
                        {
                            resultsMap.Add(results[i], 1);
                        }

                        else
                        {
                            resultsMap[results[i]] += 1;
                        }
                        
                    }

                    //removes the items from the inventory
                    foreach (var ingredient in InventoryManager.instance.CurrentIngredients.ToList())
                    {
                        InventoryManager.instance.SubtractItem(ingredient.Key, ingredient.Value);
                        
                    }
                    
                    //puts them to a list to remove the Enumerable operations on them
                    List<KeyValuePair<TasteResults, int>> list = resultsMap.ToList();

                    var sortedResults = from entry in list orderby entry.Value descending select entry;

                    
                    var listedResults = sortedResults.ToList();
                    
                    //another string array for the purpose of having different texts, i know it can be optimised but I'm lazy
                    List<string> stringNames = new List<string>(3);

                    
                    string tasteName = String.Empty;


                    string text = String.Empty;
                    //checks if it's the perfect bueno, if not, gives each mistake it's own dialogue
                    if (!perfectBueno)
                    {
                        for (int i = 0; i < outputResults.Capacity; i++)
                        {
                            if (listedResults[i].Value > 0)
                            {
                                #region Results

                                switch (listedResults[i].Key)
                                {
                                    case TasteResults.TooSweet:
                                        tasteName = "Lacks Sweet";
                                        break;

                                    case TasteResults.TooFluffy:
                                        tasteName = "Too Fluffy";
                                        break;

                                    case TasteResults.TooDry:
                                        tasteName = "Too Dry";
                                        break;
                                    case TasteResults.TooBitter:
                                        tasteName = "Too Bitter";
                                        break;
                                    case TasteResults.TooSalty:
                                        tasteName = "Too Bitter";
                                        break;
                                    case TasteResults.TooSour:
                                        tasteName = "Too Sour";
                                        break;

                                    case TasteResults.TooWatery:
                                        tasteName = "Too Watery";
                                        break;

                                    case TasteResults.TooSpicy:
                                        tasteName = "Too Spicy";
                                        break;
                                    case TasteResults.NotEnoughSugar:
                                        tasteName = "Lacking in Sugar";
                                        break;
                                    case TasteResults.NotEnoughSalt:
                                        tasteName = "Lacking in Salt";
                                        break;
                                    case TasteResults.NotEnoughSour:
                                        tasteName = "Lacking in Sourness";
                                        break;
                                    case TasteResults.NotEnoughSpice:
                                        tasteName = "Lacking in Spice";
                                        break;
                                    case TasteResults.NotEnoughBitter:
                                        tasteName = "Lacking in Bitterness";
                                        break;

                                }

                                if (tasteName != String.Empty)
                                {
                                    stringNames.Add(tasteName);
                                }

                                #endregion

                            }
                        }



                        //new dialogue $"{ }" is known as a string format and is easier than writing " " + (variable name) + " "
                        switch (stringNames.Count)
                        {
                            case 3:
                                text =
                                    $"You crafted a {t.name}, however your {t.name} was {stringNames[0]} , {stringNames[1]} and {stringNames[2]}";
                                GameManager.instance.AddScore(25);

                                break;
                            case 2:
                                text =
                                    $"You crafted a {t.name}, however your {t.name} was {stringNames[0]} and {stringNames[1]}";
                                GameManager.instance.AddScore(25);
                                break;
                            case 1:
                                text =
                                    $"You crafted a {t.name}, however your {t.name} was {stringNames[0]}";
                                GameManager.instance.AddScore(25);
                                break;

                        }
                    }


                    //perfect bueno dialogue
                    else if (perfectBueno)
                    {
                        text = $"You crafted a perfect {t.name}";   
                        GameManager.instance.AddScore(100);
                    }
                    
                    //result if you somehow managed to craft something that doesn't exist
                    else
                    {
                        text = $"You crafted something that isn't even on the menu!??? How did you manage that?";
                        image = emptyImage;
                        GameManager.instance.AddScore(0);
                   

                    }

                    itemBox.SetActive(true);
                    itemBox.GetComponentInChildren<RawImage>().texture = image;
                    itemBox.GetComponentInChildren<TextMeshProUGUI>().text = text;
                    isFrozen = true;


                }

                //failsafe dialogue

                else
                {
                    string text = $"You crafted something that isn't even on the menu!??? How did you manage that?";
                    image = emptyImage;
                    itemBox.SetActive(true);
                    itemBox.GetComponentInChildren<RawImage>().texture = image;
                    itemBox.GetComponentInChildren<TextMeshProUGUI>().text = text;
                    isFrozen = true;
                }
                

               
                
                
                






                /*foreach (var result in results)
                {
                    Debug.Log(result);
                }*/
        }

        public void OnBoxClicked()
        {
            //resets variables
            perfectBueno = false;
            InventoryManager.instance.CurrentIngredients.Clear();
            Time.timeScale = 1;
            itemBox.SetActive((false));
            isFrozen = false;
            perfectCount = 0;
            IngredientGenerator.instance.Begin();
        }
    }
    
    
    
}
