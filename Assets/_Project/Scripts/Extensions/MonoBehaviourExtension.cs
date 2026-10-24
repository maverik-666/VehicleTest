namespace Project
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public static class MonoBehaviourExtensions
    {
        public static T GetParentRecursive<T>(this MonoBehaviour monoBeh)
        {
            Transform parent = monoBeh.transform.parent;
            while (parent != null)
            {
                if (parent.TryGetComponent(out T c))
                {
                    return c;
                }

                parent = parent.parent;
            }

            return default;
        }
        
        public static void Shuffle<T>(this IList<T> ts) {
            int count = ts.Count;
            int last = count - 1;
            for (var i = 0; i < last; ++i) {
                int r = Random.Range(i, count);
                (ts[i], ts[r]) = (ts[r], ts[i]);
            }
        }
        
        public static void SetAlpha(this SpriteRenderer renderer, float alpha)
        {
            Color color = renderer.color;
            color.a = Mathf.Clamp01(alpha);
            renderer.color = color;
        }

        public static void SetAlpha(this Image image, float alpha)
        {
            Color color = image.color;
            color.a = Mathf.Clamp01(alpha);
            image.color = color;
        }
    }
}