using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : Singleton<ProjectileManager>
{
    public enum WeaponType
    {
        Pistol,
        Grenade,
        Ball
    }

    [SerializeField] public GameObject PistolProjectilePrefab;
    [SerializeField] public GameObject GrenadeProjectilePrefab;
    [SerializeField] public GameObject BallProjectilePrefab;

    private List<GameObject> pooledPistolProjectiles = new List<GameObject>();
    private List<GameObject> pooledGrenadeProjectiles = new List<GameObject>();
    private List<GameObject> pooledBallProjectiles = new List<GameObject>();
    private int _amountToPool = 5;

    protected override void Awake()
    {
        base.Awake();

        GeneratePoolObjects(_amountToPool, PistolProjectilePrefab, pooledPistolProjectiles);
        GeneratePoolObjects(_amountToPool, GrenadeProjectilePrefab, pooledGrenadeProjectiles);
        GeneratePoolObjects(_amountToPool, BallProjectilePrefab, pooledBallProjectiles);
    }

    public GameObject GetPooledProjectile(WeaponType weaponTypeToPool)
    {
        if (weaponTypeToPool == WeaponType.Pistol)
            return PoolFromList(pooledPistolProjectiles);
        else if (weaponTypeToPool == WeaponType.Grenade)
            return PoolFromList(pooledGrenadeProjectiles);
        else
            return PoolFromList(pooledBallProjectiles);
    }

    private GameObject PoolFromList (List<GameObject> listToPoolFrom)
    {
        for (int i = 0; i < listToPoolFrom.Count; i++)
        {
            if (!listToPoolFrom[i].activeInHierarchy)
            {
                return listToPoolFrom[i];
            }
        }

        return null;
    }

    private void GeneratePoolObjects(int amountToPool, GameObject prefab, List<GameObject> listToAdd)
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject gameObject = Instantiate<GameObject>(prefab, transform.position, Quaternion.identity);
            gameObject.transform.SetParent(transform);
            gameObject.SetActive(false);
            listToAdd.Add(gameObject);
        }
    }
}
