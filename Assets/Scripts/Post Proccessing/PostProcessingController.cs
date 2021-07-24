using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Класс для накладывания постэффектов
/// </summary>
[RequireComponent(typeof(Volume))]
public class PostProcessingController : MonoBehaviour
{
    [SerializeField]
    private Color playerHitColor;

    private static Color color;

    private Volume volume;
    private ColorAdjustments colorAdjustments;

    
    private void Awake()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out colorAdjustments);
        PlayerHealth.PlayerHit += OnPlayerHit;
    }

    private void OnDestroy()
    {
        PlayerHealth.PlayerHit -= OnPlayerHit;
    }

    private void OnPlayerHit()
    {
        SetNewColorFilter(playerHitColor);
    }

    void Update()
    { 
        color = Color.Lerp(color, Color.white, Time.deltaTime);
        colorAdjustments.colorFilter.Override(color);
    }

    public  void SetNewColorFilter(Color newColor)
    {
        color = newColor;
    }

}
