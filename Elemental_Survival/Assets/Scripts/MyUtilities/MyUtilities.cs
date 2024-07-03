using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyUtilities
{
    public static class ArrayShuffle
    {
        public static T[] Shuffle<T>(T[] array)
        {
            List<T> results = array.ToList();
            int cnt = array.Length;
            for (int i = 0; i < cnt; i++)
            {
                int rand = Random.Range(0, cnt - i);
                results.Add(results[rand]);
                results.RemoveAt(rand);
            }
            return results.ToArray();
        }
    }

    public static class YieldCache
    {
        private static readonly Dictionary<float, WaitForSeconds> waitForSecondsCache = new(new Compare());

        public static WaitForSeconds GetWaitForSeconds(float seconds)
        {
            if (waitForSecondsCache.TryGetValue(seconds, out var wfs))
            {
                return wfs;
            }
            else
            {
                WaitForSeconds result = new(seconds);
                waitForSecondsCache.Add(seconds, result);
                return result;
            }
        }

        private class Compare : IEqualityComparer<float>
        {
            public bool Equals(float x, float y)
            {
                return x == y;
            }

            public int GetHashCode(float obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}