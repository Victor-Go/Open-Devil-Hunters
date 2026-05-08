using Code.Scripts.Src.Types;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Behaviour.Environment.Background
{
    public class FogForest : Map
    {
        private readonly string[] fogs = new[]
        {
            "Map/Forest/Decorations/Cloud/ForestCloud0",
            "Map/Forest/Decorations/Cloud/ForestCloud1",
            "Map/Forest/Decorations/Cloud/ForestCloud2",
            "Map/Forest/Decorations/Cloud/ForestCloud3",
            "Map/Forest/Decorations/Cloud/ForestCloud4",
        };

        private List<GameObject> fogGameObjects = new();

        protected override void Start()
        {
            base.Start();

            int count = Random.Range(1, 3);
            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, fogs.Length);
                GameObject fog = Instantiate(resourceManager.GetResource(fogs[index]));
                fog.transform.SetParent(null);
                fog.transform.position = (Vector2)transform.position + new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2));
                fogGameObjects.Add(fog);
            }
        }
    }
}
