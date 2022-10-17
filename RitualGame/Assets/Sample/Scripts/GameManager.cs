using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = SeededRandom.RandomGenerator;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; set; }



    public GameObject dialogueBox;
    public float multiplier;
    
    public float ingredientMultiplier = 1;

    public float inflationMultiplier = 2;

    public float deflationMultiplier = 0.5f;
    
    public float timeUntilRandomizer;
    

    private float timeRemaining;

    [SerializeField]private float coolDown;

    private bool canCountDown;

    public Transform bigHand;
    public float BigHandRate;
    public Transform smallHand;
    public float SmallHandRate;

    
    private float timer;

    
    public Color dayLight;
    public Color nightLight;

    public Light light;

    public TextMeshProUGUI text;
    public int score;
    private void Awake()
    {
        instance = this;
        
    }


    // Start is called before the first frame update
    void Start()
    {
        multiplier = ingredientMultiplier;
        AddScore(0);
        ResetTime(false);
    }

   public void ResetTime(bool isCooldown)
    {
        dialogueBox.SetActive(false);
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

    public void AddScore(int amount)
    {
        score += amount;
        text.text = score.ToString();
    }
    

    void CountDownTimer()
    {
        if (smallHand.rotation.eulerAngles.z <= -180 || smallHand.rotation.eulerAngles.z >= 180)
        {
            light.color = dayLight;
           
        }

        else
        {
            light.color = nightLight;
        }

        if (smallHand.rotation.eulerAngles.z <= -360 || smallHand.rotation.eulerAngles.z >= 360)
        {
            smallHand.Rotate(0, 0, 0);
        }
        
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            bigHand.Rotate(0, 0, -Time.deltaTime * BigHandRate);
            smallHand.Rotate(0, 0, -Time.deltaTime * (SmallHandRate % bigHand.transform.rotation.eulerAngles.z));
        }

        else
        {
            timeRemaining = timer;
            RandomEffect();
            
        }

    }

    void RandomEffect()
    {
        string text = String.Empty;
        canCountDown = false;
        
        if (canCountDown == false)
        {
            
            
            int i = multiplier == ingredientMultiplier ? Random.RngRange(1, 3) : 0;

            switch (i)
            {
                case 1:
                    multiplier = inflationMultiplier;
                    text = $"Inflation Rate is Normal";
                    break;
                case 2:
                    multiplier = deflationMultiplier;
                    text = $"Inflation Rate is Low! You take half as many materials! Which is Good! ";
                    break;
                default:
                    multiplier = ingredientMultiplier;
                    text = $"Inflation Rate is High! You now take double materials! Which is bad!";
                    break;
            }

            dialogueBox.gameObject.SetActive(true);
            dialogueBox.GetComponentInChildren<TextMeshProUGUI>().text = text;
        }
        
        
        
        
        Debug.Log(multiplier);

        
        
    }
}
