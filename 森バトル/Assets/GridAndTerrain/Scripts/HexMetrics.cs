using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMetrics
{
    //These constants determine the size of each hexagon.
    //Each hexagon can be described with 2 circles. Each corner touching the outer circle, and the center of each edge touching the inner one.
    public const float outerRadius = 10f;
    public const float innerRadius = outerRadius * 0.866025404f;

    //These constants determine the strength of the color shading in each cell.
    public const float solidFactor = 0.75f;
    public const float blendFactor = 1f - solidFactor;

    //This constant determines how high each level of elevation is in the scene.
    public const float elevationStep = 5f;

    //Perlin Noise
    public static Texture2D noiseSource;
    public const float cellPerturbStrengh = 5f;
    public const float elevationPerturbStrength = 50f;
    public const float noiseScale = 0.0008f;

    //List of relative coordinates for the corners of each hexagon.
    public static Vector3[] corners =
    {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius)
    };

    //Returns the coordinates of 
    public static Vector3 GetBridge(HexDirection direction)
    {
        return (corners[(int)direction] + corners[(int)direction + 1]) * blendFactor;
    }

    public static Vector3 GetFirstCorner(HexDirection direction)
    {
        return corners[(int)direction];
    }

    public static Vector3 GetSecondCorner(HexDirection direction)
    {
        return corners[(int)direction + 1];
    }

    public static Vector3 GetFirstSolidCorner(HexDirection direction)
    {
        return corners[(int)direction] * solidFactor;
    }

    public static Vector3 GetSecondSolidCorner(HexDirection direction)
    {
        return corners[(int)direction + 1] * solidFactor;
    }

    public static Vector4 SampleNoise(Vector3 position)
    {
        return noiseSource.GetPixelBilinear(
            position.x * noiseScale,
            position.z * noiseScale);
    }
}
