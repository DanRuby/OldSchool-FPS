using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������� ����� ��� ����������� ������ � ������
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Header("�������")]
    [SerializeField]
    private ParticleSystem onHitParticle;

    [SerializeField]
    private AudioClip onHitSound;
    
    [Header("�����")]
    [SerializeField, Tooltip("���� �� ����� ������ ������, �� ������ ������ ���� ������ ���� ����� 0")]
    private float explosionRadius;

    [SerializeField]
    private LayerMask explosionLayer;

    [SerializeField]
    private float expForce;
    
    [SerializeField]
    private float damage;
    
    [Header("���������")]
    [SerializeField]
    private float initialImpulse;
    
    [SerializeField]
    private float lifeTime;
    
    private bool isExploding => explosionRadius > 0;
    private Rigidbody rigid;
    private GenericPool<Projectile> pool;
    private float timer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (timer<= 0)
            Despawn();

        timer -= Time.deltaTime;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (isExploding)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, explosionLayer);
            foreach (Collider col in colliders)
            {
                IDamagable takeDamage = col.GetComponent<IDamagable>();
                if (takeDamage != null)
                    takeDamage.TakeDamage(damage, expForce, explosionRadius, transform.position);
            }
        }
        else
        {
            IDamagable damagable = collision.collider.GetComponent<IDamagable>();
            if (damagable != null)
                damagable.TakeDamage(damage);
        }
        Despawn();
    }

    /// <summary>
    /// ��������� ������ �������, �������� ���������, ������� ������ � ���
    /// </summary>
    private void Despawn()
    {
        if (onHitParticle != null)
            ParticlesManager.Instance.SpawnVFX(Vector3.up, transform.position, onHitParticle);
        if (onHitSound != null)
            AudioManager.PlayOneShot(onHitSound); 

        rigid.isKinematic = true;
        rigid.velocity = Vector3.zero;
        if (pool != null)
            pool.ReturnObject(this);
        else Destroy(gameObject);
    }

    /// <summary>
    /// ����������� ����� �������, ������� � ������������ ������
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rotation"></param>
    /// <param name="pool"></param>
    public void Respawn(Vector3 pos,Quaternion rotation, GenericPool<Projectile> pool)
    {
        transform.position = pos;
        transform.rotation = rotation;
        rigid.rotation = rotation;
        rigid.position = pos;
        rigid.isKinematic = false;

        this.pool = pool;
        timer = lifeTime;

        gameObject.SetActive(true);
        rigid.AddForce(initialImpulse * transform.forward, ForceMode.Impulse);
    }
}