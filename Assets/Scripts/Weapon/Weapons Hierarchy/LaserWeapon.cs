using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Оружие стреляющее лазером в виде лайн рендерера
/// </summary>
public class LaserWeapon : WeaponBase
{
    [SerializeField]
    private Transform startingPoint;

    [SerializeField]
    private int maxReflections;

    [SerializeField]
    private Laser laser;

    private LineRenderer laserLine;

    protected override void Awake()
    {
        base.Awake();
        laserLine=laser.GetComponent<LineRenderer>();
        laserLine.positionCount = 0;
    }

    public override void Shoot()
    {
        float distance = _distance;
        laser.enabled = true;
        laserLine.positionCount = 1;
        laserLine.SetPosition(0, startingPoint.position);

        Ray ray = _cam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        for (int i = 0; i < maxReflections; i++)
        {
            if (Physics.Raycast(ray, out RaycastHit raycastHit, distance))
            {
                laserLine.positionCount = i + 2;
                laserLine.SetPosition(i + 1, raycastHit.point);

                IDamagable takeDamage = raycastHit.collider.GetComponent<IDamagable>();
                if (takeDamage != null)
                {
                    takeDamage.TakeDamage(_damage*i);
                    break;
                }
                else
                {
                    ray.origin = raycastHit.point;
                    ray.direction = Vector3.Reflect(ray.direction, raycastHit.normal);
                }
                distance -= raycastHit.distance;
            }
            else
            {
                laserLine.positionCount = laserLine.positionCount + 1;
                laserLine.SetPosition(laserLine.positionCount - 1, ray.origin + ray.direction * distance);
                break;
            };
        }

      

    }
}
