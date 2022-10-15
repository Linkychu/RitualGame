using System.Collections.Generic;
using UnityEngine;
using cookingData;

    
    //Using structs here so we can have our ingredients have required servings for the recipes to work
   

    [CreateAssetMenu(fileName = "Recipe", menuName = "Food/Recipe")]

    public class Recipe : ScriptableObject
    {
        //list of ingredients and their servings
        public List<SIngredient> ingredients;
        
        public Sprite icon;
    }
