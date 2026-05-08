using System;
using System.Collections.Generic;

namespace Code.Scripts.Src.Utils
{
    public static class ListUtils
    {
        private static readonly Random Rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}