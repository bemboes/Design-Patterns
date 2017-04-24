using UnityEngine;

public static class ColorFunctions
{
    public static Color SetAlphaChannel(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}