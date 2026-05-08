using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Src.Utils
{
    public static class TriggerGroup
    {
        private static List<List<string>> interactiveLayers = new()
        {
            new(){ "PlayerSkill", "EnemyHurt" },
            new(){ "CollidablePlayerSkill", "EnemyHurt" },
            new(){ "PlayerSkillTrigger", "EnemyHurt"},
            new(){ "EnemyHit", "PlayerHurt" },
            new(){ "EnemySkill", "PlayerHurt" },
            new(){ "PlayerPickUp", "PickableObject" }
        };

        public static bool CanInteract(string layerNameA, string layerNameB)
        {
            foreach (var layerGroup in interactiveLayers)
            {
                if (layerGroup.Count == 1 && layerGroup.Contains(layerNameA) && layerNameA.Equals(layerNameB))  // If there's only 1 string in list, it means it can interact with itself.
                {
                    return true;
                }
                else if (layerGroup.Contains(layerNameA) && layerGroup.Contains(layerNameB) && !layerNameA.Equals(layerNameB))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CanInteract(int layerIndexA, int layerIndexB)
        {
            return CanInteract(LayerMask.LayerToName(layerIndexA), LayerMask.LayerToName(layerIndexB));
        }
    }
}
