using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ������������ �������
/// </summary>
public class Destructable : MonoBehaviour,IDamagable
{
    [SerializeField]
    private bool destructableByCasualAttack;

    public void TakeDamage(float damage)
    {
        if(destructableByCasualAttack)
            Destroy(gameObject);
    }

    public void TakeDamage(float damage, float expForce, float expRadius, Vector3 expPosition)
    {
        Destroy(gameObject);
    }
}
