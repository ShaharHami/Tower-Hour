using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshCollider))]
public class CollisionDetection : MonoBehaviour
{
    private Enemy enemy;
    void Start()
    {
        enemy = transform.parent.GetComponent<Enemy>();
    }
    void OnParticleCollision(GameObject other)
    {
        TowerPlacement towerPlacement = FindObjectOfType<TowerPlacement>();
        if (towerPlacement != null)
        {
            int damage = towerPlacement.ShotDamage;
            enemy.Damage(damage);
        }
    }
}
