using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = SeededRandom.RandomGenerator;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }

    
   
    
    public float multiplier;
    
    public float ingredientMultiplier = 1;

    public float inflationMultiplier = 2;

    public float deflationMultiplier = 0.5f;
    
    public float timeUntilRandomizer;
    

    private float timeRemaining;

    [SerializeField]private float coolDown;

    private bool canCountDown;

    private float timer;
    private void Awake()
    {
        instance = this;
        
    }


    // Start is called before the first frame update
    void Start()
    {
        multiplier = ingredientMultiplier;
        //ResetTime(false);
    }

    void ResetTime(bool isCooldown)
    {
         timer = isCooldown ? coolDown : timeUntilRandomizer;
         timeRemaining = timer;
         canCountDown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canCountDown)
        {
            CountDownTimer();
        }
    }

    public void RandomRecipe()
    {
        
    }
    

    void CountDownTimer()
    {
        
    
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            
        }

        else
        {
            timeRemaining = timer;
            RandomEffect();
            
        }

    }

    void RandomEffect()
    {
        canCountDown = false;
        
        if (canCountDown == false)
        {
            
            
            int i = multiplier == ingredientMultiplier ? Random.RngRange(1, 3) : 0;

            switch (i)
            {
                case 1:
                    multiplier = inflationMultiplier;
                    break;
                case 2:
                    multiplier = deflationMultiplier;
                    break;
                default:
                    multiplier = ingredientMultiplier;
                    break;
            }
        }
        
        Debug.Log(multiplier);

        
        ResetTime(true);
    }
}
