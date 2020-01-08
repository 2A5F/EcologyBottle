using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Utils
{
    public static IEnumerable<int> RangeTo(this int a, int b)
    {
        if(a > b) (a, b) = (b, a);
        for (int i = a; i <= b; i++)
        {
            yield return i;
        }
    }

    public static bool AllTrue(this IEnumerable<bool> self)
    {
        foreach (var item in self)
        {
            if (!item) return false;
        }
        return true;
    }

    public static Vector3 Y(this Vector2 self)
    {
        return new Vector3(self.x, 0, self.y);
    }

    public static void Deconstruct(this Vector3 self, out float x)
    {
        x = self.x;
    }
    public static void Deconstruct(this Vector3 self, out float x, out float y)
    {
        Deconstruct(self, out x);
        y = self.y;
    }
    public static void Deconstruct(this Vector3 self, out float x, out float y, out float z)
    {
        Deconstruct(self, out x, out y);
        z = self.z;
    }

    public static Vector3 FMap(this Vector3 self, Func<float, float> fn)
    {
        return new Vector3(fn(self.x), fn(self.y), fn(self.z));
    }
}