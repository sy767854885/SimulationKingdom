using Components.Grid;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameSys.Structure
{
    [System.Serializable]
    public class RoadFixer : StructureFixer
    {
        public List<TargetMatcher> targetMatchers = new List<TargetMatcher>();

        public override GameObject GetPrefab(Vector2Int indexPos,GridPlacement placement, out int rotationValue)
        {
            rotationValue = 0;
            var neighbours = placement.GetNeighboursOfTypeFor(indexPos, StructureType.Road);
            List<bool> matchList = neighbours.Select(x => x != null).ToList();
            GameObject prefab = GetRoadPrefab(matchList, out rotationValue);
            return GetRoadPrefab(matchList, out rotationValue);
        }

        public GameObject GetRoadPrefab(List<bool> matchList, out int rotationValue)
        {
            rotationValue = 0;
            GameObject prefab = null;
            foreach (var matcher in targetMatchers)
            {
                if (matcher.IsMatchWithExist(matchList))
                {
                    prefab = matcher.target;
                    rotationValue = matcher.rotationValue;
                    continue;
                }
            }

            if (prefab == null) prefab = defaultObj;
            return prefab;
        }
    }
}

