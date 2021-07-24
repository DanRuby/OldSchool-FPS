using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для создания партиклей по умолчанию или любых других
/// </summary>
public class ParticlesManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem enemyHitParticle;

    [SerializeField]
    private ParticleSystem environmentHitParticle;

    public static ParticlesManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }
    private static void SetSpawnedVFX(Vector3 normal, Vector3 point, ParticleSystem spawnedVFX)
    {
        spawnedVFX.transform.position = point;
        spawnedVFX.transform.rotation = Quaternion.FromToRotation(spawnedVFX.transform.forward, normal);
        spawnedVFX.Play();
    }

    public void SpawnVFX(Vector3 normal, Vector3 point, ParticleSystem particle) => SetSpawnedVFX(normal, point, Instantiate(particle));

    public void SpawnEnemyHitVFX(Vector3 normal, Vector3 point) => SetSpawnedVFX(normal, point, Instantiate(enemyHitParticle));

    public void SpawnEnvironmentHitVFX(Vector3 normal, Vector3 point) => SetSpawnedVFX(normal, point, Instantiate(environmentHitParticle));
}
