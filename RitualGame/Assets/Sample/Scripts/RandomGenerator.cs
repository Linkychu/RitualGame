using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System.IO;

//namespaces are handy just in-case we have functions or classes with the same name 
namespace SeededRandom
{
    public class RandomGenerator : MonoBehaviour
    {
        //Creating a Singleton so we can reference this script anywhere without having a direct reference to it
        public static RandomGenerator randomGen { get; set; }

        public int InitSeed;

        //For debugging purposes, we can have the seed stay the same for all
        public bool useRandomSeed;

        //C# Random class allows us to use the C# random functions.
        public static Random random;

        private void Awake()
        {

            //Checks to see if we have another copy of this object in this scene. If not we set the randomGen reference to this. This will only happen at the beginning of the game.

            if (randomGen == null)
            {
                randomGen = this;
                DontDestroyOnLoad(this.gameObject);
            }

            else
            {
                
                Destroy(this.gameObject);
            }

            GenerateSeed();
        }

        void GenerateSeed()
        {
            if (useRandomSeed)
            {
                //Converts the current time the date is open to a seed.
                InitSeed = (int) System.DateTime.Now.Ticks;
            }

            //We now set the random generation states of the Unity Random Function and the regular random function to start from the Seed.
            random = new Random(InitSeed);
            UnityEngine.Random.InitState(InitSeed);
        }


        public static int RngRange(int lowerBound, int upperBound)
        {
            //picks a value between the lowerBound and upperBound. https://learn.microsoft.com/en-us/dotnet/api/system.random.next?view=net-7.0
            int randomValue = random.Next(lowerBound, upperBound);
            //returns the value as an integer
            return randomValue;
        }
        
        public static int RngValue()
        {
            //returns a random value from 0 - Int.32MAX
            return random.Next();
        }

    }
}
