using System;
using System.Collections;
using System.Collections.Generic;
using cookingData;
using UnityEngine;
using SeededRandom;

using SeedRandom = SeededRandom.RandomGenerator;
using Extensions;
using UnityEngine.UI;


public class IngredientGenerator : MonoBehaviour
{
    public static IngredientGenerator instance { get; set; }
    public int amount;

    public Transform ingredientParent;

    public Transform shelf;
    float shelfWidth;
    
    private List<Ingredient> Ingredients = new List<Ingredient>();

    public bool randomiseIngredients = true; public GameObject NPC;
    public RawImage image;
    
    
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
     void Start()
     {
         Begin();
     }

     public void Begin()
     {
         DestroyChildren();
         Ingredients.Clear();
         RandomiseRecipe();
         //sets shelf width to the X scale of the object
         shelfWidth = shelf.transform.localScale.x;

         if (randomiseIngredients)
         {
             GenerateRandomItems();
         }
         else
         {
             GenerateItems();
         }
     }

     void DestroyChildren()
     {
         //looks for any ingredients and destroys them 
         foreach (Transform IngredientTransform in ingredientParent)
         {
             Destroy(IngredientTransform.gameObject);
         }
     }
    public void RandomiseRecipe()
    {
        //shuffles list and adds ingredients
        CraftingManager.instance.recipes.Shuffle();
        var MyRecipes = CraftingManager.instance.recipes[0].ingredients;
        foreach (var varIngredient in MyRecipes)
        {
            Ingredients.Add(varIngredient.ingredient);
        }

        image.texture = CraftingManager.instance.recipes[0].icon.texture;
        NPC.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();
    }
    
    void GenerateRandomItems()
    {
        //does this X amount of times
        for (int i = 0; i < amount; i++)
        {
            //sets scriptable object position to be the same distance away from the next
            Vector3 pos = new Vector3((shelfWidth / amount) * i, shelf.transform.position.y + 1, shelf.transform.position.z);
            //spawns and gets a reference to a random ingredient using our seed
            Ingredient ingredient = Instantiate(Ingredients[SeedRandom.RngRange(0, Ingredients.Count)]);
            //spawns ingredient into the world
            ingredient.Spawn(pos, ingredientParent);
           
        }
    }
    void GenerateItems()
    {
        Ingredients.Shuffle();
        for (int i = 0; i < Ingredients.Count; i++)
        {
            
            //sets scriptable object position to be the same distance away from the next
            Vector3 pos = new Vector3((shelfWidth / amount) * i, shelf.transform.position.y + 1, shelf.transform.position.z);
            //spawns and gets a reference to a random ingredient using our seed
              Ingredient ingredient = Instantiate(Ingredients[i]);
            //spawns ingredient into the world
            ingredient.Spawn(pos, ingredientParent);

        }
        
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
