
using System.Collections;
using System.Collections.Generic;
using SeededRandom;
using UnityEngine;
namespace Extensions
{
    
    public static class Extensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = RandomGenerator.random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}
