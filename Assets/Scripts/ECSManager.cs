using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{

    public static EntityManager manager;
    public GameObject asteroidPrefab;

    public int numAsteroids;
    public int min, max;
    BlobAssetStore store;

    private void Start()
    {
        store = new BlobAssetStore();
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);
        Entity asteroid = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroidPrefab, settings);

        for (int i = 0; i < numAsteroids; i++)
        {
            var instance = manager.Instantiate(asteroid);
            float x = UnityEngine.Random.Range(min, max);
            float y = UnityEngine.Random.Range(min, max);
            float z = UnityEngine.Random.Range(min, max);
            var position = new float3(x, y, z);
            manager.SetComponentData(instance, new Translation { Value = position });
            float rspeed = UnityEngine.Random.Range(1, 10) /10.0f;
            manager.SetComponentData(instance, new Float { speed = rspeed});
        }
    }

    private void OnDestroy()
    {
        store.Dispose();
    }

}
