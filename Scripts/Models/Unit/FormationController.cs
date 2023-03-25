using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

/*
 * Player controllable unit called a formation. Represents a team of units. 
 * Moves within the overworld grid map
 */
namespace Assets.Scripts.Models.Unit
{
    public class FormationController: MonoBehaviour
    {
        private HexCell location, currentTravelLocation;
        private float orientation;
        public Pathfinder path;
        public static FormationController unitPrefab;
        const float travelSpeed = 4f;
        const float rotationSpeed = 180f;
        List<HexCell> pathToTravel;

        public HexGrid Grid { get; set; }

        public int VisionRange
        {
            get
            {
                return 10;
            }
        }

        public int Speed
        {
            get
            {
                return 24;
            }
        }

        //Make sure that units are always in the proper location after a recompile
        void OnEnable()
        {
            if (location)
            {
                transform.localPosition = location.Position;
                if (currentTravelLocation)
                {
                    path.IncreaseVisibility(location, VisionRange);
                    path.DecreaseVisibility(currentTravelLocation, VisionRange);
                    currentTravelLocation = null;
                }
            }
        }

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
                    //location.DecreaseVisibility();
                    path.DecreaseVisibility(location, VisionRange);
                    location.formationController = null;
                }
                location = value;
                //value.Unit = this;
                Debug.Log("RE-ENABLE LINE73 PLAYER FORMATION");
                //value.IncreaseVisibility();
                path.IncreaseVisibility(value, VisionRange);
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
            return cell.IsExplored && !cell.IsUnderwater && !cell.formationController;
        }

        public void Travel(List<HexCell> path)
        {
            location.formationController = null;
            location = path[path.Count - 1];
            //location.Unit = this;
            Debug.Log("REENABLE Line 108 PLAYER FORMATION");

            pathToTravel = path;
            StopAllCoroutines();
            StartCoroutine(TravelPath());
        }
        /*
        void OnDrawGizmos()
        {
            if (pathToTravel == null || pathToTravel.Count == 0)
            {
                return;
            }

            Vector3 a, b, c = pathToTravel[0].Position;

            for (int i = 1; i < pathToTravel.Count; i++)
            {
                a = c;
                b = pathToTravel[i - 1].Position;
                c = (b + pathToTravel[i].Position) * 0.5f;
                for (float t = 0f; t < 1f; t += Time.deltaTime * travelSpeed)
                {
                    Gizmos.DrawSphere(Bezier.GetPoint(a, b, c, t), 2f);
                }
            }

            a = c;
            b = pathToTravel[pathToTravel.Count - 1].Position;
            c = b;
            for (float t = 0f; t < 1f; t += 0.1f)
            {
                Gizmos.DrawSphere(Bezier.GetPoint(a, b, c, t), 2f);
            }
        }
        */

        public void Die()
        {
            if (location)
            {
                path.DecreaseVisibility(location, VisionRange);
            }
            location.formationController = null;
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
            grid.unitManager.AddFormation(
                Instantiate(unitPrefab), grid.GetCell(coordinates), orientation
            );
        }

        IEnumerator TravelPath()
        {
            Vector3 a, b, c = pathToTravel[0].Position;
            //transform.localPosition = c;
            yield return LookAt(pathToTravel[1].Position);
            path.DecreaseVisibility(currentTravelLocation ? currentTravelLocation : pathToTravel[0], VisionRange);

            float t = Time.deltaTime * travelSpeed;

            for (int i = 1; i < pathToTravel.Count; i++)
            {
                currentTravelLocation = pathToTravel[i];
                a = c;
                b = pathToTravel[i - 1].Position;
                c = (b + currentTravelLocation.Position) * 0.5f;

                path.IncreaseVisibility(pathToTravel[i], VisionRange);
                for (; t < 1f; t += Time.deltaTime * travelSpeed)
                {
                    transform.localPosition = Bezier.GetPoint(a, b, c, t);
                    yield return null;
                }
                path.DecreaseVisibility(pathToTravel[i], VisionRange);
                t -= 1f;
            }
            currentTravelLocation = null;

            a = c;
            //b = pathToTravel[pathToTravel.Count - 1].Position;
            b = location.Position;
            c = b;
            path.IncreaseVisibility(location, VisionRange);

            for (; t < 1f; t += Time.deltaTime * travelSpeed)
            {
                transform.localPosition = Bezier.GetPoint(a, b, c, t);
                yield return null;
            }
            transform.localPosition = location.Position;

            //Release Cell List
            ListPool<HexCell>.Add(pathToTravel);
            pathToTravel = null;
        }

        IEnumerator LookAt(Vector3 point)
        {
            point.y = transform.localPosition.y;
            Quaternion fromRotation = transform.localRotation;
            Quaternion toRotation =
                Quaternion.LookRotation(point - transform.localPosition);
            float angle = Quaternion.Angle(fromRotation, toRotation);
            if (angle > 0f)
            {
                float speed = rotationSpeed / angle;

                for (
                    float t = Time.deltaTime * speed;
                    t < 1f;
                    t += Time.deltaTime * speed
                )
                {
                    transform.localRotation =
                        Quaternion.Slerp(fromRotation, toRotation, t);
                    yield return null;
                }
            }

            transform.LookAt(point);
            orientation = transform.localRotation.eulerAngles.y;
        }

        //Return negative 1 to indicate restricted movement
        public int GetMoveCost(HexCell fromCell, HexCell toCell, HexDirection direction)
        {
            HexEdgeType edgeType = fromCell.GetEdgeType(toCell);
            if (edgeType == HexEdgeType.Cliff) //AVOID THESE EDGE TYPES
            {
                return -1;
            }

            int moveCost;
            if (fromCell.HasRoadThroughEdge(direction))
            {
                moveCost = 1;
            }
            else if (fromCell.Walled != toCell.Walled)
            {
                return -1;
            }
            else
            {
                moveCost = edgeType == HexEdgeType.Flat ? 5 : 10; //MOVE COSTS
                moveCost +=
                    toCell.UrbanLevel + toCell.FarmLevel + toCell.PlantLevel;
            }

            return moveCost;
        }

    }


}
