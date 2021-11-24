using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{

    public static EntityManager manager;
    public GameObject asteroidPrefab;
    public GameObject blueAsteroidPrefab;
    public GameObject shotPrefab;
    public GameObject muzzle;

    public int numAsteroids;
    public int numBlueAsteroids;
    public int min, max;
    int numshots = 10;
    BlobAssetStore store;

    Entity shot;

    private void Start()
    {
        store = new BlobAssetStore();
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);
        Entity asteroid = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroidPrefab, settings);
        Entity blueAsteroid = GameObjectConversionUtility.ConvertGameObjectHierarchy(blueAsteroidPrefab, settings);
        shot = GameObjectConversionUtility.ConvertGameObjectHierarchy(shotPrefab, settings);

        for (int i = 0; i < numAsteroids; i++)
        {
            var instance = manager.Instantiate(asteroid);
            float x = UnityEngine.Random.Range(min, max);
            float y = UnityEngine.Random.Range(min, max);
            float z = UnityEngine.Random.Range(min, max);
            var position = new float3(x, y, z);
            manager.SetComponentData(instance, new Translation { Value = position });
            float rspeed = UnityEngine.Random.Range(1, 3) /10.0f;
            manager.SetComponentData(instance, new Float { speed = rspeed});
        } 
        
        for (int i = 0; i < numBlueAsteroids; i++)
        {
            var instance = manager.Instantiate(blueAsteroid);
            float x = UnityEngine.Random.Range(min, max);
            float y = UnityEngine.Random.Range(min, max);
            float z = UnityEngine.Random.Range(min, max);
            var position = new float3(x, y, z);
            manager.SetComponentData(instance, new Translation { Value = position });
            float rspeed = UnityEngine.Random.Range(1, 3) /10.0f;
            manager.SetComponentData(instance, new Float { speed = rspeed});
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < numshots; i++)
            {
                var instance = manager.Instantiate(shot);
                var startPos = muzzle.transform.position + UnityEngine.Random.insideUnitSphere *2;
                manager.SetComponentData(instance, new Translation { Value = startPos });
                manager.SetComponentData(instance, new Rotation { Value = muzzle.transform.rotation });
            }
        }
    }

    private void OnDestroy()
    {
        store.Dispose();
    }

}
