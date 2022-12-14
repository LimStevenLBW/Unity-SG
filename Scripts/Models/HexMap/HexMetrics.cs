using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics
{
    public const float outerToInner = 0.866025404f;
    public const float innerToOuter = 1f / outerToInner;
    public const float outerRadius = 10f;
    public const float innerRadius = outerRadius * outerToInner;

    public const float solidFactor = .75f; //Default value .75f, at 1f, there is no space between cells
    public const float blendFactor = 1f - solidFactor;
    public const float elevationStep = 3f;
    public const int chunkSizeX = 1, chunkSizeZ = 1; //cell count = x * z, i.e 25
    public const float waterElevationOffset = -0.5f;

    //Terraced Edge Connections
    public const int terracesPerSlope = 2;
    public const int terraceSteps = terracesPerSlope * 2 + 1;

    public static Texture2D noiseSource;
    public const float noiseScale = 0.003f;
    public const float elevationPerturbStrength = 1.5f; //Perturbing Cell Elevation //Default value 1.5f
    public const float cellPerturbStrength = 1f;      //Default value 4f

    //River
    public const float streamBedElevationOffset = -1.75f;

    public const float waterFactor = 0.6f;
    public const float waterBlendFactor = 1f - waterFactor;

    //Hash Grid
    public const int hashGridSize = 256;
    static HexHash[] hashGrid;
    public const float hashGridScale = 0.25f;

    //Walls
    public const float wallHeight = 4f;
    public const float wallYOffset = -1f; //Sink into ground slightly
    public const float wallThickness = 0.75f;
    public const float wallElevationOffset = verticalTerraceStepSize;
    public const float wallTowerThreshold = 0.75f; //spawns towers about 75% of the time on corners

    public const float bridgeDesignLength = 7f;

    public static Vector3 GetWaterBridge(HexDirection direction)
    {
        return (corners[(int)direction] + corners[(int)direction + 1]) * waterBlendFactor;
    }

    public static Vector3 GetFirstWaterCorner(HexDirection direction)
    {
        return corners[(int)direction] * waterFactor;
    }

    public static Vector3 GetSecondWaterCorner(HexDirection direction)
    {
        return corners[(int)direction + 1] * waterFactor;
    }

    private static Vector3[] corners = {

        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius)
    };

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

    //Bridge Offset, midpoint between two relevant corners
    public static Vector3 GetBridge(HexDirection direction)
    {
        return (corners[(int)direction] + corners[(int)direction + 1]) * blendFactor;
    }

    //It takes an unperturbed point and returns a perturbed one
    public static Vector3 Perturb(Vector3 position)
    {
        Vector4 sample = SampleNoise(position);
        position.x += (sample.x * 2f - 1f) * cellPerturbStrength;
        position.z += (sample.z * 2f - 1f) * cellPerturbStrength;
        return position;
    }

    //For terrace step interpolation
    public const float horizontalTerraceStepSize = 1f / terraceSteps;
    public const float verticalTerraceStepSize = 1f / (terracesPerSlope + 1);

    public static Vector3 TerraceLerp(Vector3 a, Vector3 b, int step)
    {
        float h = step * HexMetrics.horizontalTerraceStepSize;
        a.x += (b.x - a.x) * h;
        a.z += (b.z - a.z) * h;

        float v = ((step + 1) / 2) * HexMetrics.verticalTerraceStepSize;
        a.y += (b.y - a.y) * v;

        return a;
    }

    public static Color TerraceLerp(Color a, Color b, int step)
    {
        float h = step * HexMetrics.horizontalTerraceStepSize;
        return Color.Lerp(a, b, h);
    }

    public static HexEdgeType GetEdgeType(int elevation1, int elevation2)
    {
        if (elevation1 == elevation2) //If elevations are the same, flat edge
        {
            return HexEdgeType.Flat;
        }

        int delta = elevation2 - elevation1;
        if (delta == 1 || delta == -1) //If the level difference is exactly one step, then we have a slope
        {
            return HexEdgeType.Slope;
        }

        return HexEdgeType.Cliff; //in all other cases we have a cliff.
    }

    public static Vector4 SampleNoise(Vector3 position)
    {
        return noiseSource.GetPixelBilinear(
            position.x * noiseScale,
            position.z * noiseScale
        );
    }
    public static Vector3 GetSolidEdgeMiddle(HexDirection direction)
    {
        return
            (corners[(int)direction] + corners[(int)direction + 1]) *
            (0.5f * solidFactor);
    }
    public static void InitializeHashGrid(int seed)
    {
        hashGrid = new HexHash[hashGridSize * hashGridSize];
        Random.State currentState = Random.state;
        Random.InitState(seed);
        for (int i = 0; i < hashGrid.Length; i++)
        {
            hashGrid[i] = HexHash.Create();
        }
        Random.state = currentState;
    }

    /* it uses the 
     * XZ coordinates of a 
     * position to retrieve a value. 
     * The hash index is found by clamping the coordinates to integer values, then taking the remainder of the integer division by the grid size.
     */
    public static HexHash SampleHashGrid(Vector3 position)
    {
        int x = (int)(position.x * hashGridScale) % hashGridSize;
        if (x < 0)
        {
            x += hashGridSize;
        }
        int z = (int)(position.z * hashGridScale) % hashGridSize;
        if (z < 0)
        {
            z += hashGridSize;
        }
        return hashGrid[x + z * hashGridSize];
    }

    /* EXAMPLE
     *  For level 1, let's use a 40% chance for a hovel. The other building won't appear at all. This requires the threshold triplet (0.4, 0, 0).
        For level 2, let's replace the hovels with larger buildings, and add a 20% chance for additional hovels. Still no high-rises. 
        That suggests the threshold triplet (0.2, 0.4, 0).
        For level 3, let's upgrade the medium buildings to high-rises, replace the hovels again, and add another 20% change for more hovels. 
        The thresholds for that would be (0.2, 0.2, 0.4).
        So the idea is that we upgrade existing building and add new ones in empty lots as the urban level increases. 
        To replace an existing building, we have to use the same hash value ranges. If hashes between 0 and 0.4 were hovels at level 1,
        the same range should produce high-rises at level 3. Specifically, at level 3 high-rises should spawn for hash values in the 0–0.4 range, 
        the two-story houses in the 0.4–0.6 range, and the hovels in the 0.6–0.8 range. If we check them from highest to lowest,
        we can do this with the threshold triplet (0.4, 0.6, 0.8). 
        The level 2 thresholds then become (0, 0.4, 0.6), and the level 1 thresholds become (0, 0, 0.4).
    */
    static float[][] featureThresholds = {
        new float[] {0.0f, 0.0f, 0.4f},
        new float[] {0.0f, 0.4f, 0.5f},
        new float[] {0.4f, 0.5f, 0.6f}
    };

    public static float[] GetFeatureThresholds(int level)
    {
        return featureThresholds[level];
    }

    public static Vector3 WallThicknessOffset(Vector3 near, Vector3 far)
    {
        Vector3 offset;
        offset.x = far.x - near.x;
        offset.y = 0f;
        offset.z = far.z - near.z;

        return offset.normalized * (wallThickness * 0.5f);
    }

	public static Vector3 WallLerp(Vector3 near, Vector3 far)
    {
        near.x += (far.x - near.x) * 0.5f;
        near.z += (far.z - near.z) * 0.5f;
        float v =
            near.y < far.y ? wallElevationOffset : (1f - wallElevationOffset);
        near.y += (far.y - near.y) * v + wallYOffset; 
        return near;
    }

}
