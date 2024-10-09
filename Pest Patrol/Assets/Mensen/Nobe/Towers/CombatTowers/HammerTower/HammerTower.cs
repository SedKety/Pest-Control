using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HammerTower : CombatTower
{
    public float swingAngle;
    public Transform hammerHolder;

    private Quaternion startRotation;
    private Quaternion endRotation;
    private bool isSwingingForward = false;
    private bool isSwingingBackward = false;
    private float swingProgress = 0f;
    private float swingDuration;

    private const float minSwingDuration = 0.1f;
    private const float maxSwingDuration = 5f;
    private const float baseDuration = 1.0f;
    private const float reloadSpeedSensitivity = 2.0f;

    public ProjectileType projectileType;
    protected override void Start()
    {
        Ticker.OnTickAction += OnTick;
        UpdateSwingDuration();

        startRotation = hammerHolder.rotation;
        endRotation = startRotation * Quaternion.AngleAxis(-swingAngle, Vector3.right);
    }

    protected override void OnTick()
    {
        UpdateSwingDuration();
    }

    protected override void Update()
    {
        if (!canShoot)
        {
            ResetSwing();
            return;
        }

        if (!isSwingingForward && !isSwingingBackward && canShoot)
        {
            StartSwing();
        }

        if (isSwingingForward)
        {
            SwingHammerForward();
        }

        if (isSwingingBackward)
        {
            SwingHammerBackward();
        }
    }

    void StartSwing()
    {
        isSwingingForward = true;
        swingProgress = 0f;
    }

    public void AttackNearbyEnemies()
    {
        var nearbyEnemies = GameManager.enemies
            .Where(e => e != null && Vector3.Distance(e.GetComponent<Transform>().position, shootPoint.position) < detectionRange &&
                        e.GetComponent<Enemy>().type == typeToTarget)
            .ToList();

        for (int i = 0; i < nearbyEnemies.Count; i++)
        {
            if (nearbyEnemies[i] != null)
            {
                nearbyEnemies[i].GetComponent<Enemy>().OnHit((int)(baseDamage * TowerManager.globalTowerDamageMultiplier), projectileType);
            }
        }

        if (nearbyEnemies.Count > 0)
        {
            audio.Play();
        }

        
    }

    void SwingHammerForward()
    {
        if (!canShoot)
        {
            ResetSwing();
            return;
        }

        swingProgress += Time.deltaTime / swingDuration;

        hammerHolder.rotation = Quaternion.Lerp(startRotation, endRotation, swingProgress);

        if (swingProgress >= 1f)
        {
            isSwingingForward = false;
            isSwingingBackward = true;
            AttackNearbyEnemies();
            swingProgress = 0f;
        }
    }

    void SwingHammerBackward()
    {
        if (!canShoot)
        {
            ResetSwing();
            return;
        }

        swingProgress += Time.deltaTime / swingDuration;

        hammerHolder.rotation = Quaternion.Lerp(endRotation, startRotation, swingProgress);

        if (swingProgress >= 1f)
        {
            isSwingingBackward = false;
        }
    }

    private void ResetSwing()
    {
        isSwingingForward = false;
        isSwingingBackward = false;
        swingProgress = 0f;
        hammerHolder.rotation = startRotation;  
    }

    private void UpdateSwingDuration()
    {
        if (baseReloadSpeed > 0)
        {
            swingDuration = Mathf.Clamp(baseDuration * (baseReloadSpeed / reloadSpeedSensitivity), minSwingDuration, maxSwingDuration);
        }
        else
        {
            swingDuration = maxSwingDuration;
        }
    }
}