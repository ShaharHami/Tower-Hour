using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] Transform enemyBase;
    [SerializeField] ParticleSystem deathFX;
    [SerializeField] float destroyDelay;
    public void DestroyBase()
    {
        ParticleSystem deathFXInstance = Instantiate(deathFX, transform.position, Quaternion.identity);
        deathFXInstance.transform.localScale = new Vector3(5, 5, 5);
        ToggleVisibility();
        Destroy(gameObject, destroyDelay);
    }
    private void ToggleVisibility()
    {
        enemyBase.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
    private void DestroyBaseGO()
    {


    }
}
