using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� ��� ������������ ������ �� ��������� ��� ������ �������
/// </summary>
[RequireComponent(typeof(AudioSource))] 
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip weaponPickup;

    [SerializeField]
    private AudioClip playerHit;

    [SerializeField]
    private AudioClip ammoPickup;

    [SerializeField]
    private AudioClip keyPickup;

    [SerializeField]
    private AudioClip healthPickup;

    private static AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        WeaponManager.WeaponPickedup += PlayWeaponPickupSound;
        PlayerHealth.PlayerHit += PlayHitSound;
    }

    private void OnDestroy()
    {
        WeaponManager.WeaponPickedup -= PlayWeaponPickupSound;
        PlayerHealth.PlayerHit += PlayHitSound;
    }

    /// <summary>
    /// ��������� ���� ���� ���
    /// </summary>
    /// <param name="clip"></param>
    public static void PlayOneShot(AudioClip clip)
    {
        if(clip != null)
            audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// ��������� ���� �������� ������
    /// </summary>
    private void PlayWeaponPickupSound(WeaponType weapon)
    { 
        PlayOneShot(weaponPickup);
    }


    private void PlayHitSound()
    {
        PlayOneShot(playerHit);
    }
}
