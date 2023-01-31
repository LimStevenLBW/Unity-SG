using UnityEngine;

namespace Assets.Scripts.Models.HexMap.HexHelpers
{
    //Manages the texture that contains cell data, This allows us to triangulate once. The cell data can be made available via a texture.
    public class HexCellShaderData : MonoBehaviour
    {

        Texture2D cellTexture;
        Color32[] cellTextureData;

        public void Initialize(int x, int z)
        {
            if (cellTexture)
            {
                cellTexture.Resize(x, z);
            }
            else
            {
                cellTexture = new Texture2D(
                    x,
                    z,
                    TextureFormat.RGBA32,
                    false,
                    true
                );

                cellTexture.filterMode = FilterMode.Point;
                cellTexture.wrapMode = TextureWrapMode.Clamp;
                Shader.SetGlobalTexture("_HexCellData", cellTexture); //Static, make texture name globally known
            }

            Shader.SetGlobalVector(
                "_HexCellData_TexelSize",
                new Vector4(1f / x, 1f / z, x, z)
            );

            //Using a Color32 array as a color buffer, we apply all of the cell data, else we reset if we already have it
            if (cellTextureData == null || cellTextureData.Length != x * z)
            {
                cellTextureData = new Color32[x * z];
            }
            else
            {
                for (int i = 0; i < cellTextureData.Length; i++)
                {
                    cellTextureData[i] = new Color32(0, 0, 0, 0);
                }
            }

            enabled = true;
            
        }

        public void RefreshTerrain(HexCell cell)
        {
            //Store terrain type index in the pixel's alpha component, support up to 256 terrain types by converting to byte
            cellTextureData[cell.Index].a = (byte)cell.TerrainTypeIndex;
            enabled = true;
        }
        public void RefreshVisibility(HexCell cell)
        {
            cellTextureData[cell.Index].r = cell.IsVisible ? (byte)255 : (byte)0;
            enabled = true;
        }

        void LateUpdate()
        {
            cellTexture.SetPixels32(cellTextureData);
            cellTexture.Apply();
            enabled = false;
        }



    }
}
