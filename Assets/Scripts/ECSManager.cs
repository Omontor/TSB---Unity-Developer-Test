using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{

    public static EntityManager manager;
    public GameObject asteroidPrefab;
    public GameObject ChunkPrefab;
    public GameObject shotPrefab;
    public GameObject secondShotPrefab;
    public GameObject muzzle;
    public AudioSource sfx;
    bool isSuperShot;


    public int numAsteroids;
    public int min, max;
    int numshots = 1;
    BlobAssetStore store;

    Entity shot;
    public static Entity chunk;

    private void Start()
    {
        store = new BlobAssetStore();
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);
        Entity asteroid = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroidPrefab, settings);
        shot = GameObjectConversionUtility.ConvertGameObjectHierarchy(shotPrefab, settings);
        chunk = GameObjectConversionUtility.ConvertGameObjectHierarchy(ChunkPrefab, settings);

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
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < numshots; i++)
            {
                var instance = manager.Instantiate(shot);
                var startPos = muzzle.transform.position;
                manager.SetComponentData(instance, new Translation { Value = startPos });
                manager.SetComponentData(instance, new Rotation { Value = muzzle.transform.rotation });
                sfx.Play();
            }
        }
    }

    private void OnDestroy()
    {
        store.Dispose();
    }

}
