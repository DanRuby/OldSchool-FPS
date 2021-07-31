using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Класс, выводящий информацию о игроке
/// </summary>
public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    private Image[] keyImages;

    [SerializeField]
    private TextMeshProUGUI hpMesh;

    private void Awake()
    {
        foreach (Image key in keyImages)
            key.enabled = false;

        PlayerHealth.PlayerHealthChanged += OnPlayerHealthChanged;
        KeyPickup.KeyPickedup += OnKeyPickedup;
    }

    private void OnDestroy()
    {
        PlayerHealth.PlayerHealthChanged -= OnPlayerHealthChanged;
        KeyPickup.KeyPickedup -= OnKeyPickedup;
    }

    private void OnPlayerHealthChanged(float health) => hpMesh.text = ((int)health).ToString();

    private void OnKeyPickedup(Keys key) => keyImages[(int)key].enabled = true;

}
