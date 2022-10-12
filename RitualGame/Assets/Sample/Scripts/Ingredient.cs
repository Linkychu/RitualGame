using UnityEngine.UI;
using UnityEngine;


    [CreateAssetMenu(fileName = "Ingredient", menuName = "Food/Ingredient")]
    public class Ingredient : ScriptableObject
    {
        public GameObject Model;
        public int AmountPerServing;
        public string Name;
        public Image icon;

        [HideInInspector] public GameObject SpawnedModel;
        

        //function created in the scriptable object instead of Generator script so we can spawn the model
        public void Spawn(Vector3 position, Transform parent)
        {
            SpawnedModel = Instantiate(Model, position, Quaternion.identity, parent);
            
            //spawns at offset
            SpawnedModel.transform.position += new Vector3(parent.transform.position.x, 0, 0);

            
            
            //Sets the SO information in the ingredient Script
            
        }
    }
    
   