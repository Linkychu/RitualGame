using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Specialized;

public class InventoryManager : MonoBehaviour
{
    //A dictionary is an unordered list that allows us to have a key and a value think of them as giving each item an id
    public Dictionary<Ingredient, int> CurrentIngredients = new Dictionary<Ingredient, int>();

    
    //Singleton instance of class, allows us to reference the class while only having one instance of the object in the scene
    public static InventoryManager instance { get; set; }

    public List<Ingredient> ingredientList = new List<Ingredient>();

    public RawImage[] images;

    private Texture defaultTexture;

    public GameObject itemManager;
    public GameObject craftIcon;

    private bool isActive = true;
    [SerializeField] private KeyCode hideKey;
    
   
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
        if (Input.GetKeyDown(hideKey))
        {
            isActive = !isActive;
            itemManager.SetActive(isActive);
            craftIcon.SetActive(isActive);
        }
    }

    //removes item from inventory when clicked
    public void onIconClick(int id)
    {
            SubtractItem(ingredientList[id], (ingredientList[id].AmountPerServing ));
    }

    

    public void AddItem(Ingredient ingredient, float amount)
    {
        
        //amount = Mathf.FloorToInt(amount * GameManager.instance.multiplier);
        //if the dictionary already has the item, changes the value of the item instead
        if (CurrentIngredients.ContainsKey(ingredient))
        {
            if (CurrentIngredients[ingredient] <= 0)
            {
                RemoveItem(ingredient);
            }

            else
            {
                CurrentIngredients[ingredient] += Mathf.RoundToInt(amount);
            }
           
        }

        else
        {
            //if not, adds it to the dictionary
            CurrentIngredients.Add(ingredient, Mathf.RoundToInt(amount));
            
        }
        
        UpdateItem();
    }

    public void SubtractItem(Ingredient ingredient, float amount)
    {
        //checks to see if the dictionary contains the item
        if (CurrentIngredients.ContainsKey(ingredient))
        {
            //if the item value is bigger than 0, subtract item value 
           
            CurrentIngredients[ingredient] -= Mathf.RoundToInt((amount));
            UpdateItem();
            
        }

        else
        {
            //if item does not exist, it will print out an error
           Debug.LogError("Item does not exist");
           UpdateItem();
        }   
        
        
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
    
    
    public void UpdateItem()
    {
        ingredientList.Clear();
        
        var removeItemsDic = CurrentIngredients.Where(kvp => kvp.Value <= 0).Select(kvp => kvp.Key).ToArray();

        Dictionary <RawImage, Texture> myImages = new Dictionary<RawImage, Texture>();
        foreach (var item in removeItemsDic)
        {
            CurrentIngredients.Remove(item);
        }
        //converts dictionary keys and values to a list
        ingredientList = CurrentIngredients.Keys.ToList();

        
        var dict = CurrentIngredients.Where(pair => pair.Value > 0).ToArray();
       
        foreach (var vImage in images)
        {
            vImage.texture = defaultTexture;
            vImage.GetComponentInChildren<TextMeshProUGUI>().text = String.Empty;
            myImages.Add(vImage, defaultTexture);
        }
        
        for (int i = 0; i < dict.Length; i++)
        {
            //since the inventory items and the ingredients will have the same amount, it's easy to compare the two and allow us to set values between each other
            //sets image texture to be the icon image of the same number as well as the text
            if(!(myImages.ContainsValue(dict[i].Key.icon.texture)))
            {
                images[i].texture = dict[i].Key.icon.texture;
                images[i].GetComponentInChildren<TextMeshProUGUI>().text = dict[i].Value.ToString();
                ingredientList.Add(dict[i].Key);
            }
            

        }
        
        //myImages.Clear();
        
    }

    public void ClearSlots()
    {
        
        foreach (var image in images)
        {
            image.texture = defaultTexture;
            image.GetComponentInChildren<TextMeshProUGUI>().text = String.Empty;
        }
        
        
        
        
    }

   
}
