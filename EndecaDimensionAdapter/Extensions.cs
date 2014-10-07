using System.Collections.Generic;
using System.Linq;

namespace EndecaDimensionAdapter
{
    public static class Extensions
    {
        public static void AddRange<T>(this HashSet<T> hs, IEnumerable<T> collection)
        {
            foreach (var i in collection)
            {
                hs.Add(i);
            }
        }

        public static bool ScrambledEquals<T>(this IEnumerable<T> collection1, IEnumerable<T> collection2)
        {
            if (collection1 == null && collection2 == null) return true;
            if (collection1 == null || collection2 == null) return false;
            var cnt = new Dictionary<T, int>();
            foreach (var s in collection1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }
            foreach (var s in collection2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }
    }
}
