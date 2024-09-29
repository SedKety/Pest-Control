using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnityEngine;

public class EggScript : Projectile
{
    public float stunTime;
    public float explosionDelayInSeconds;
    public GameObject explosion;
    public LayerMask groundLayer;
    protected override async void Start()
    {
        base.Start();
        await StunTowers();
    }

    public async Task StunTowers()
    {
        await Task.Delay((int)(explosionDelayInSeconds * 1000));

        var towers = BuildingSystem.Instance.towersGO
            .Where(tower => Vector3.Distance(tower.transform.position, transform.position) <= hitDistance)
            .Select(c => c.GetComponent<CombatTower>())
            .ToList();

        for (int i = 0; i < towers.Count; i++)
        {
            towers[i].StartStun(stunTime);  
        }

        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
