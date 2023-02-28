﻿using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Models.HexMap.HexHelpers
{
    //Manages the texture that contains cell data, This allows us to triangulate once. The cell data can be made available via a texture.
    public class HexCellShaderData : MonoBehaviour
    {
       
        Texture2D cellTexture;
        Color32[] cellTextureData;
        List<HexCell> transitioningCells = new List<HexCell>();
        const float transitionSpeed = 255f;
        bool needsVisibilityReset;

        public bool isImmediateCellReveal { get; set; }

        public HexGrid Grid { get; set; }

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

            transitioningCells.Clear();
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
            int index = cell.Index;
            if (isImmediateCellReveal)
            {
                cellTextureData[index].r = cell.IsVisible ? (byte)255 : (byte)0;
                cellTextureData[index].g = cell.IsExplored ? (byte)255 : (byte)0;
            }
            else if (cellTextureData[index].b != 255)
            {
                cellTextureData[index].b = 255;
                transitioningCells.Add(cell);
            }
            enabled = true;
        }

        void LateUpdate()
        {
            if (needsVisibilityReset)
            {
                needsVisibilityReset = false;
                Grid.ResetVisibility();
            }

            // We can now determine the delta to apply to the values, by multiplying the time delta with the speed
            int delta = (int)(Time.deltaTime * transitionSpeed);

            if (delta == 0)
            {
                delta = 1; //Minimum one
            }

            for (int i = 0; i < transitioningCells.Count; i++)
            {
                if (!UpdateCellData(transitioningCells[i], delta))
                {
                    transitioningCells[i--] =
                    transitioningCells[transitioningCells.Count - 1];
                    transitioningCells.RemoveAt(transitioningCells.Count - 1);
                }
            }

            cellTexture.SetPixels32(cellTextureData);
            cellTexture.Apply();
            enabled = transitioningCells.Count > 0;
        }

        bool UpdateCellData(HexCell cell, int delta)
        {
            int index = cell.Index;
            Color32 data = cellTextureData[index];
            bool stillUpdating = false;

            if (cell.IsExplored && data.g < 255)
            {
                stillUpdating = true;
                int t = data.g + delta;
                data.g = t >= 255 ? (byte)255 : (byte)t;
            }

            if (cell.IsVisible && data.r < 255)
            {

                if (data.r < 255)
                {
                    stillUpdating = true;
                    int t = data.r + delta;
                    data.r = t >= 255 ? (byte)255 : (byte)t;
                }
            }
        
		    else if (data.r > 0) {
			    stillUpdating = true;
			    int t = data.r - delta;
                data.r = t< 0 ? (byte)0 : (byte) t;

            }

            if (!stillUpdating)
            {
                data.b = 0;
            }

            cellTextureData[index] = data;
            return stillUpdating;
        }

        public void ViewElevationChanged()
        {
            needsVisibilityReset = true;
            enabled = true;
        }
    }
}
