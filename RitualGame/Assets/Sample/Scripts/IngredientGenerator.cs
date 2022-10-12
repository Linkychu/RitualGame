using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeededRandom;

using SeedRandom = SeededRandom.RandomGenerator;
using Extensions;


public class IngredientGenerator : MonoBehaviour
{
    
    public int amount;

    public Transform ingredientParent;

    public Transform shelf;
    float shelfWidth;
    
    public List<Ingredient> Ingredients = new List<Ingredient>();

    public bool randomiseIngredients = true;

    // Start is called before the first frame update
    void Start()
    {
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
        for (int i = 0; i < amount; i++)
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
