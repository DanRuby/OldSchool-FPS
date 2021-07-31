using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Вспомогательный класс для создания уровней. На старте сцены выключает всх детей. При включении включает их.
/// </summary>
public class EnemyGroup : MonoBehaviour
{
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        enabled = false;
    }

    private void OnEnable()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
    }
}
