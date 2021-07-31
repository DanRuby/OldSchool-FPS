using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Оружие, которое при стрельбе создает проджектайл
/// </summary>
public class ProjectileWeapon : WeaponBase
{
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private Projectile projectile;

    private GenericPool<Projectile> projectilePool;

    protected override void Awake()
    {
        base.Awake();
        projectilePool = new GenericPool<Projectile>(5, projectile);
    }

    public override void Shoot()
    {
        Projectile spawned=projectilePool.GetObject();
        spawned.Respawn(spawnPoint.position, Quaternion.LookRotation(_cam.transform.forward), projectilePool);
    }
}
