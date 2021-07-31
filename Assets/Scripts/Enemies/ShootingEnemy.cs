using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Inactive,Persuing,Attacking,Dead,Unknown
}

/// <summary>
/// Класс врага, создающего проджектайл при атаке 
/// </summary>
public class ShootingEnemy : MonoBehaviour,IDamagable
{
    [SerializeField]
    private float upwardsExpModifier;
    
    [SerializeField]
    private float currentHP;

    [Header("Projectiles")]
    [SerializeField]
    private Transform projectileSpawnPoint;

    [SerializeField]
    private Transform playerTarget;

    [SerializeField]
    private Projectile projectile;

    [SerializeField]
    private float rateOfFire;

    [SerializeField]
    private float shotDistance;
    
    [Header("LOS")]
    [SerializeField]
    private LayerMask obstructionLayer;
    
    [SerializeField]
    private float fov;

    [SerializeField]
    private float lookDistance;

    private Transform target;
    private NavMeshAgent agent;
    private Rigidbody rigid;
    
    private State currentState;
    private bool readyToShoot;
    private static Dictionary<int,GenericPool<Projectile>> pools=new Dictionary<int, GenericPool<Projectile>>();

    private int projID;
    public static event System.Action EnemyDied;

    private void Awake()
    {
        target = playerTarget;
        currentState = State.Inactive;
        readyToShoot = true;
        
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        
        projID=projectile.GetInstanceID();
        if (!pools.ContainsKey(projID))
            pools.Add(projID,new GenericPool<Projectile>(3, projectile));
    }

    private void OnEnable()
    {
        StartCoroutine(UpdateRoutine());
    }

    private void OnDisable()
    {
        StopCoroutine(UpdateRoutine());
    }

    /// <summary>
    /// Цикл ИИ
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateRoutine()
    {
        for (;;)
        {
            switch (currentState)
            {
                case State.Inactive:
                {
                    if (TargetIsSeen())
                        currentState = State.Persuing;
                    break;
                }
                case State.Persuing:
                {
                    FollowTargetUntillClose();
                    break;
                }
                case State.Attacking:
                {
                    AttackTargetWhileInLOS();
                    break;
                }
            }
            yield return new WaitForSeconds(.15f);
        }
    }


    private void AttackTargetWhileInLOS()
    {
        agent.SetDestination(transform.position);
        if (TargetIsSeen())
        {
            if (readyToShoot)
            {
                StartCoroutine(AttackCooldown());
                Projectile spawned = pools[projID].GetObject();
                spawned.Respawn(projectileSpawnPoint.position,
                    Quaternion.LookRotation((target.position - transform.position).normalized), pools[projID]);
            }
        }
        else currentState = State.Persuing;
    }

    private void FollowTargetUntillClose()
    {
        if (Vector3.Distance(transform.position, target.position) < shotDistance || !TargetIsSeen())
            agent.SetDestination(target.position);
        else currentState = State.Attacking;
    }

    private bool TargetInLOS()
    {
        Vector3 direction = target.position - transform.position;
        return Vector3.Angle(transform.forward, direction) <= fov/2;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Правда, если цель находится в поле зрения и перед ней нет преграды</returns>
    private bool TargetIsSeen()
    {
        if(TargetInLOS())
        {
            if(Physics.Raycast(new Ray(transform.position,target.position-transform.position),out RaycastHit raycastHit,lookDistance,obstructionLayer))
                return raycastHit.transform == target;
        }
        return false;
    }

    /// <summary>
    /// Добавляет кд следующей атаке (заглушка анимации)
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackCooldown()
    {
        readyToShoot = false;
        yield return new WaitForSeconds(rateOfFire);
        readyToShoot = true;
    }

    #region IDamagableImplementation

    public void TakeDamage(float damage)
    {
        currentState = State.Attacking;
        currentHP -= damage;
        if (currentHP <= 0)
        {
            EnemyDied?.Invoke();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage, float expForce, float expRadius, Vector3 expPosition)
    {
        TakeDamage(damage);
        rigid.AddExplosionForce(expForce, expPosition, expRadius,upwardsExpModifier,ForceMode.VelocityChange);
    }

    #endregion
}
