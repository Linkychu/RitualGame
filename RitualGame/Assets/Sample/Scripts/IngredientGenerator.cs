using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SeededRandom;

using SeedRandom = SeededRandom.RandomGenerator;

public class IngredientGenerator : MonoBehaviour
{
    public Transform shelf;

    public int amount;

    public GameObject[] ingredients;

    
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject ingredient in ingredients)
        {
            ingredient.SetActive(false);
            int x = SeedRandom.RngRange(0, ingredients.Length);
            int y = SeedRandom.RngRange(0, ingredients.Length);

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
