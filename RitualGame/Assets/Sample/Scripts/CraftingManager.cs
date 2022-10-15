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
                    
                    result = t.ingredients.FirstOrDefault(x => x.ingredient.Name == sIngredient.ingredient.Name);

                        if (!result.Equals(null))
                    {
                        
                        if (result.serving == Mathf.RoundToInt(sIngredient.serving))
                        {

                            perfectCount++;
                        }
                        
                        if(sIngredient.serving > Mathf.RoundToInt(result.serving))
                        {
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

                        else if ((sIngredient.serving < Mathf.RoundToInt(result.serving)) && sIngredient.serving > 0)
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
                            
                            Debug.Log("TooLittle");
                        }
                        
                        else
                        {
                            results.Add(TasteResults.WrongIngredient);
                        }
                        
                        count++;
                        
                        

                    }

                    

                }


                Dictionary<TasteResults, int> resultsMap = new Dictionary<TasteResults, int>();
                List<string> outputResults = new List<string>(3);
                outputResults.AddRange(new string[3]);
                Texture image;
                if (count == t.ingredients.Count)
                {
                    
                  
                   
                    image = t.icon.texture;
                    

                    if (perfectCount == results.Count)
                    {
                            perfectBueno = true;    
                        
                    }
                    
                    for (int i = 0; i < results.Count; i++)
                    {
                        if (!resultsMap.ContainsKey(results[i]))
                        {
                            resultsMap.Add(results[i], 1);
                        }

                        else
                        {
                            resultsMap[results[i]] += 1;
                        }
                        
                    }

                    
                    foreach (var ingredient in InventoryManager.instance.CurrentIngredients.ToList())
                    {
                        InventoryManager.instance.SubtractItem(ingredient.Key, ingredient.Value);
                        
                    }
                    
                    List<KeyValuePair<TasteResults, int>> list = resultsMap.ToList();

                    var sortedResults = from entry in list orderby entry.Value descending select entry;


                    var listedResults = sortedResults.ToList();
                    
                    List<string> stringNames = new List<string>(3);

                    string tasteName = String.Empty;


                    string text = String.Empty;
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



                        
                        switch (stringNames.Count)
                        {
                            case 3:
                                text =
                                    $"You crafted a {t.name}, however your {t.name} was {stringNames[0]} , {stringNames[1]} and {stringNames[2]}";

                                break;
                            case 2:
                                text =
                                    $"You crafted a {t.name}, however your {t.name} was {stringNames[0]} and {stringNames[1]}";
                                break;
                            case 1:
                                text =
                                    $"You crafted a {t.name}, however your {t.name} was {stringNames[0]}";
                                break;

                        }
                    }


                    else if (perfectBueno)
                    {
                        text = $"You crafted a {t.name}";   
                    }
                    
                    else
                    {
                        text = $"You crafted something that isn't even on the menu!??? How did you manage that?";
                        image = emptyImage;
                   

                    }

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
