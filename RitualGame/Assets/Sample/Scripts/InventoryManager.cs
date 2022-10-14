using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    //A dictionary is an unordered list that allows us to have a key and a value think of them as giving each item an id
    public Dictionary<Ingredient, int> CurrentIngredients = new Dictionary<Ingredient, int>();
    
    //Singleton instance of class, allows us to reference the class while only having one instance of the object in the scene
    public static InventoryManager instance { get; set; }

    private List<Ingredient> ingredientList = new List<Ingredient>();

    public RawImage[] images;

    private Texture defaultTexture;
    
    private void Awake()
    {
        instance = this;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentIngredients.Clear();
        defaultTexture = images[0].texture;
        UpdateItem();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //removes item from inventory when clicked
    public void onIconClick(int id)
    {
        SubtractItem(ingredientList[id], ingredientList[id].AmountPerServing);
    }

    

    public void AddItem(Ingredient ingredient, int amount)
    {
        //if the dictionary already has the item, changes the value of the item instead
        if (CurrentIngredients.ContainsKey(ingredient))
        {
            CurrentIngredients[ingredient] += amount;
        }

        else
        {
            //if not, adds it to the dictionary
            CurrentIngredients.Add(ingredient, amount);
        }
        
        UpdateItem();
    }

    public void SubtractItem(Ingredient ingredient, int amount)
    {
        //checks to see if the dictionary contains the item
        if (CurrentIngredients.ContainsKey(ingredient))
        {
            //if the item value is bigger than 0, subtract item value 
            if (CurrentIngredients[ingredient] > 0)
            {
                CurrentIngredients[ingredient] -= amount;
            }

            else
            {
                //if it's less than 0, remove it from the inventory preventing underflowing items
                RemoveItem(ingredient);
            }
        }

        else
        {
            //if item does not exist, it will print out an error
           Debug.LogError("Item does not exist");
        }   
        
        UpdateItem();
    }

    public void RemoveItem(Ingredient ingredient)
    {
        //checks if item is in the inventory, if so, removes it from the dictionary
        if (CurrentIngredients.ContainsKey(ingredient))
        {
            CurrentIngredients.Remove(ingredient);
        }
        
        UpdateItem();
    }
    
    
    void UpdateItem()
    {
        //converts dictionary keys and values to a list
        ingredientList = CurrentIngredients.Keys.ToList();
        List<int> myIngredientsCount = CurrentIngredients.Values.ToList();
        for (int i = 0; i < ingredientList.Count; i++)
        {
            if (myIngredientsCount[i] != 0)
            {
                
                //since the inventory items and the ingredients will have the same amount, it's easy to compare the two and allow us to set values between each other
                //sets image texture to be the icon image of the same number as well as the text
                images[i].texture = ingredientList[i].icon.texture;
                images[i].GetComponentInChildren<TextMeshProUGUI>().text = myIngredientsCount[i].ToString();
            }

            else
            {
                //sets them back to default if the item is below 0
                images[i].texture = defaultTexture;
                images[i].GetComponentInChildren<TextMeshProUGUI>().text = String.Empty;
            }

        }
        
        
    }

   
}
