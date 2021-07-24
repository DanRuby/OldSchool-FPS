using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Класс, выводящий информацию о уровне
/// </summary>
public class LevelInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI enemiesMesh;

    [SerializeField]
    private TextMeshProUGUI secretsMesh;

    private int enemiesTotal;
    private int killedEnemies;

    private int secretsTotal;
    private int foundSecrets;

    private void Awake()
    {
        enemiesTotal=FindObjectsOfType<ShootingEnemy>().Length;
        killedEnemies = 0;
        enemiesMesh.text = $"{killedEnemies}/{enemiesTotal}";

        secretsTotal = FindObjectsOfType<SecretTrigger>().Length;
        foundSecrets = 0;
        secretsMesh.text = $"{foundSecrets}/{secretsTotal}";

        SecretTrigger.SecretFound += OnSecretFound;
        ShootingEnemy.EnemyDied += OnEnemyKilled;
    }

    private void OnDestroy()
    {
        SecretTrigger.SecretFound -= OnSecretFound;
        ShootingEnemy.EnemyDied -= OnEnemyKilled;
    }

    private void OnEnemyKilled()
    {
        killedEnemies++;
        enemiesMesh.text = $"{killedEnemies}/{enemiesTotal}";
    }

    private void OnSecretFound()
    {
        foundSecrets++;
        secretsMesh.text = $"{foundSecrets}/{secretsTotal}";
    }
}
