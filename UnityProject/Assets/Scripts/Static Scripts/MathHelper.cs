using UnityEngine;
using System.Collections;

public class MathHelper
{
        public static Vector2 Normalize(Vector2 value)
        {
			float val = 1.0f / (float)Mathf.Sqrt((value.x * value.y) + (value.y * value.y));
			value.x *= val;
			value.y *= val;
            return value;
        }
	
        public static Vector2 SmoothStep(Vector2 value1, Vector2 value2, float amount)
        {
            return new Vector2(
                Mathf.MoveTowards(value1.x, value2.x, amount),
                Mathf.MoveTowards(value1.y, value2.y, amount));
        }
}
