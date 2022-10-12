using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
    public static InventoryManager instance { get; set; }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseItem(Ingredient _ingredient, int serving)
    {
        Debug.Log($"{_ingredient.Name} + {serving}");
    }
}
