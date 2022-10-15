
using System.Collections;
using System.Collections.Generic;
using SeededRandom;
using UnityEngine;
namespace Extensions
{
    
    public static class Extensions
    {
        
        //shuffles a list
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                //swaps list positions by a random factor
                int k = RandomGenerator.random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

       
        
    }
}
