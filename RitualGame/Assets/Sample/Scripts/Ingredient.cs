using UnityEngine.UI;
using UnityEngine;


    [CreateAssetMenu(fileName = "Ingredient", menuName = "Food/Ingredient")]
    public class Ingredient : ScriptableObject
    {
        public GameObject Model;
        public int AmountPerServing;
        public string Name;
        public Image icon;
    }