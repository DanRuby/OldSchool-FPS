using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс дробовика (несколько рейкастов для эмитации дроби)
/// </summary>
public class ShotgunWeapon : WeaponBase
{
    [SerializeField]
    private int numPellets;

    [SerializeField]
    private float maxSpread;

    public override void Shoot()
    {
        Ray mainRay = _cam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        for (int i = 0; i < numPellets; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread));
            Ray pelletRay = new Ray(mainRay.origin, _cam.transform.TransformDirection(offset+Vector3.forward));

            if (Physics.Raycast(pelletRay, out RaycastHit raycastHit, _distance))
            {
                IDamagable takeDamage = raycastHit.collider.GetComponent<IDamagable>();
                if (takeDamage != null)
                {
                    takeDamage.TakeDamage(_damage);
                    ParticlesManager.Instance.SpawnEnemyHitVFX(raycastHit.normal, raycastHit.point);
                }
                ParticlesManager.Instance.SpawnEnvironmentHitVFX(raycastHit.normal, raycastHit.point);
            }
        }
    }
}
