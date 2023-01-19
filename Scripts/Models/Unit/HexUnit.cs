using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace Assets.Scripts.Models.Unit
{
    public class HexUnit: MonoBehaviour
    {
        private HexCell location;
        private float orientation;
        public static HexUnit unitPrefab;

        public HexCell Location
        {
            get
            {
                return location;
            }
            set
            {
                if (location)
                {
                    location.Unit = null;
                }
                location = value;
                value.Unit = this;
                transform.localPosition = value.Position;
            }
        }

        public float Orientation
        {
            get
            {
                return orientation;
            }
            set
            {
                orientation = value;
                transform.localRotation = Quaternion.Euler(0f, value, 0f);
            }
        }

        public void ValidateLocation()
        {
            transform.localPosition = location.Position;
        }

        public bool IsValidDestination(HexCell cell)
        {
            return !cell.IsUnderwater && !cell.Unit;
        }

        public void Die()
        {
            location.Unit = null;
            Destroy(gameObject);
        }
        public void Save(BinaryWriter writer)
        {
            location.coordinates.Save(writer);
            writer.Write(orientation);

        }
        public static void Load(BinaryReader reader, HexGrid grid)
        {
            HexCoordinates coordinates = HexCoordinates.Load(reader);
            float orientation = reader.ReadSingle();
            grid.AddUnit(
                Instantiate(unitPrefab), grid.GetCell(coordinates), orientation
            );
        }

    }
}
