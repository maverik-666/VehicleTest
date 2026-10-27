namespace Project
{
    using UnityEngine;

    public static class VectorExtensions
    {
        public static Vector2 ToXZ(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }
        
        public static Vector2 Rotate(this Vector2 v, float degrees) 
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
        
            float tx = v.x;
            float ty = v.y;
        
            return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
        }

        public static float CalcRotation(this Vector2 v)
        {
            return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        }
    }
}