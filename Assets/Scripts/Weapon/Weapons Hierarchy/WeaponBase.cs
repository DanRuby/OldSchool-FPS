using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoType
{
    PistolAmmo,ShotgunAmmo,RifleAmmo,RocketLauncherAmmo,LaserAmmo,SniperAmmo
}

/// <summary>
/// Структура для передачи информации о событии, связанном с оружием
/// </summary>
public readonly struct WeaponEventParams
{
    public readonly float RateOfFire;
    public readonly int BulletsLeft;

    public WeaponEventParams(float rateOfFire,int bulletsLeft)
    {
        RateOfFire = rateOfFire;
        BulletsLeft = bulletsLeft;
    }
}


/// <summary>
/// Базовый класс оружия. Реализует оружие, действующее путем каста луча.
/// </summary>
public class WeaponBase : MonoBehaviour
{
    [Header("Звуки, партикли и эффекты")]
    [SerializeField]
    private AudioClip gunSound;

    [SerializeField]
    private ParticleSystem muzzleParticle;

    [Space(4)]
    [Header("Характеристики")]
    [SerializeField]
    private AmmoType ammoType;

    [SerializeField]
    private bool isInfinite;

    [SerializeField]
    private int ammoLeft;

    [SerializeField]
    private int maxAmmo;

    [SerializeField]
    protected float _damage;

    [SerializeField]
    private float rateOfFire;

    [SerializeField]
    protected float _distance;

    private bool readyToFire;
    protected Camera _cam;

    public static event System.Action<WeaponEventParams> WeaponShot;
    public static event System.Action<AmmoType, int> AmmoChange;

    public const int INFINITE_AMMO=-1;
    public int Bullets => ammoLeft;
    public AmmoType AmmoType=>ammoType;

    protected virtual void Awake()
    {
        _cam = Camera.main;
        if (isInfinite)
            ammoLeft = INFINITE_AMMO;
    }

    private void OnDisable()
    {
        readyToFire = false;
    }

    private void Update()
    {
        if (ammoLeft <= 0 && ammoLeft!=INFINITE_AMMO )
            return;

        if (Input.GetMouseButton(0))
        {
            if (readyToFire)
            {
                if(!isInfinite)            
                    ammoLeft--;
                StartCoroutine(WeaponReload());
                Shoot();
                HandleEffects();
                WeaponShot?.Invoke(new WeaponEventParams(rateOfFire,ammoLeft));
            }

        }
    }

    private IEnumerator WeaponReload()
    {
        readyToFire = false;
        yield return new WaitForSeconds(1 / rateOfFire);
        readyToFire = true;
    }

    private void HandleEffects()
    {
        if (gunSound != null)
            AudioManager.PlayOneShot(gunSound);

        if (muzzleParticle != null)
            muzzleParticle.Play();
    }

    public virtual void Shoot()
    { 
        Ray ray = _cam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit raycastHit, _distance))
        {
            IDamagable takeDamage = raycastHit.collider.GetComponent<IDamagable>();
            if (takeDamage != null)
            {
                takeDamage.TakeDamage(_damage);
                ParticlesManager.Instance.SpawnEnemyHitVFX(raycastHit.normal, raycastHit.point);
            }
            else ParticlesManager.Instance.SpawnEnvironmentHitVFX(raycastHit.normal,raycastHit.point);
        }
    }

    public void UnHolster()
    {
        readyToFire = true;
    }

    public int AddAmmo(int amount)
    {
        if (isInfinite)
            return amount;

        int leaveAmmo=0;
        ammoLeft += amount;
        if (ammoLeft > maxAmmo)
            leaveAmmo = ammoLeft - maxAmmo;
        ammoLeft = ammoLeft - leaveAmmo;
        AmmoChange?.Invoke(ammoType, ammoLeft);
        return leaveAmmo;
    }
}
