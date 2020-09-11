using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField]
    float DeSpawnTime = 5f;
    float BulletSpeed;
    float BulletDamage;
    [SerializeField]
    bool EnemyBullet = false;

    [SerializeField]
    GameObject Effect;
    void Start()
    {
        Destroy(this.gameObject, DeSpawnTime);
    }
    void FixedUpdate()
    {
         transform.Translate(0, BulletSpeed * Time.fixedDeltaTime,0);
    }

    #region Get/Set
    public float GetBulletDamage()
    {
        return BulletDamage;
    }
    public float GetBulletSpeed()
    {
        return BulletSpeed;
    }
    public void SetBulletDamage(float damage = 1f)
    {
        BulletDamage = damage;
    }
    public void SetBulletSpeed(float speed = 1f)
    {
        BulletSpeed = speed;
    }
    #endregion

    #region Collision

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {         
            col.GetComponent<PlayerBoatBehaviour>().SetHit(true);
            col.GetComponent<PlayerBoatBehaviour>().DamageCheck(BulletDamage);
            SpawnExplosion();
            Destroy(this.gameObject);
        }
        if(col.tag == "Enemy")
        {
            if(!EnemyBullet)
            {                
                col.GetComponent<EnemyBehaviour>().SetHit(true);
                col.GetComponent<EnemyBehaviour>().DamageCheck(BulletDamage);
                SpawnExplosion();
                Destroy(this.gameObject);
            }
        }


    }

    #endregion
    void SpawnExplosion()
    {
        GameObject e = Instantiate(Effect, new Vector2(this.transform.position.x,this.transform.position.y), Quaternion.identity);
    }
}
