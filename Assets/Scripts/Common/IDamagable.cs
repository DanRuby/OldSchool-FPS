using UnityEngine;

/// <summary>
/// ��������� ��� ��������� ����� ������ ����
/// </summary>
public interface IDamagable 
{
    public void TakeDamage(float damage);

    public void TakeDamage(float damage, float expForce, float expRadius,Vector3 expPosition);
}
