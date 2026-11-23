using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance { get; private set; }
    [SerializeField] private int maxPoolSize=20;
    Dictionary<GameObject, ObjectPool<GameObject>> pools=new();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public GameObject GetProjectile(GameObject prefab)
    {
        if(!pools.ContainsKey(prefab))
        {
            pools[prefab] = CreatePool(prefab);
        }

        return pools[prefab].Get();
    }

    private ObjectPool<GameObject> CreatePool(GameObject prefab)
    {
        ObjectPool<GameObject> pool = null;

        pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab, transform);
                obj.GetComponent<Projectile>().SetPool(pool);

                return obj;
            },
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            maxSize: maxPoolSize
        );

        return pool;
    }
}
