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

    [CreateAssetMenu(fileName = "Recipe", menuName = "Food/Recipe")]

    public class Recipe : ScriptableObject
    {
        public List<SIngredient> ingredients;
    }
}