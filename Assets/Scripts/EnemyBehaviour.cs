using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField]
    float MaxLife = 3f;
    float Life;
    [SerializeField]
    Image LifeBar;
    bool HasHit = false;
    bool Dead;
    
    [Header("Enemy Settings")]
    [SerializeField]
    float EnemySpeed = 1f;
    [SerializeField]
    float RotationSpeed =2;
    float offset = 1;
    [SerializeField]
    bool isChaser = false;
    bool CanMove = false;
    bool CanShoot = true;
    bool CanLook = true;
    bool CanExplode = true;
    bool CanTakeDamage = true;

    [SerializeField]
    float HitDamage = 1f;

    [Header("Player Settings")]
    [SerializeField]
    GameObject PlayerLocation;
    [SerializeField]
    float SafeDistance = 4f;
    [Header("Bullet Settings")]
    [SerializeField]
    float BulletDamage = 1f;
    [SerializeField]
    float BulletSpeed = 3f;
    [SerializeField]
    float BulletCooldown = 3f;
    float CanShotBullet = 0f;
    [SerializeField]
    Transform Cannon;
    [SerializeField]
    GameObject Bullet;
    

    [Header("Visual Settings")]
    [SerializeField]
    Sprite[] Type;
    [SerializeField]
    SpriteRenderer MySkin;

    [SerializeField]
    GameObject FlameEff;
    [SerializeField]
    GameObject ExplosionEff;

    bool SpawnOnce = true;

    [SerializeField]
    bool RandomBehaviour = false;
    int dice;

    [SerializeField]
    GameData Data;

    void Start()
    {
        if(Data == null)
        {
            Data = GameObject.FindGameObjectWithTag("Data").GetComponent<GameData>();
        }
        ResetLife();
        FindPlayer();
        
        if(RandomBehaviour)
            Randomize();
    }

    void Update()
    {
      if(PlayerLocation == null)
      {
        FindPlayer();
      }  
      else
      {      
        LookAtPlayer();
        ChasePlayer();
        CheckPlayerDeath();
      }

      CheckDeath();
    }

    void FixedUpdate()
    {
        MoveEnemy();
    }

    #region Behaviours
    void Randomize()
    {
        dice = Random.Range(1,6);
        if(dice%2 !=1)
        {
            isChaser = true;
        }
        else
        {
            isChaser = false;
        }
    }
    void FindPlayer()
    {
        PlayerLocation = GameObject.FindGameObjectWithTag("Player");
    }
     void MoveEnemy()
    {
        if(CanMove == true && !Dead)       
            transform.Translate(EnemySpeed * Time.fixedDeltaTime, 0,0);
        

    }
    void ChasePlayer()
    {
        if(!isChaser && !Dead)
        {
            if(Vector3.Distance(this.transform.position, PlayerLocation.transform.position) >= SafeDistance)
            {
                CanMove = true;
            }
            else
            {
                CanMove = false;
                ShootPlayer();
            }
        }
        else
        {
            CanMove = true;
        }
    }

    void ShootPlayer()
    {
        
        if(Time.time > CanShotBullet && CanShoot)
        {
            GameObject b = Instantiate(Bullet);  
            b.transform.position = Cannon.position;
            b.transform.rotation = Cannon.rotation;
            b.GetComponent<BulletBehaviour>().SetBulletDamage(BulletDamage);
            b.GetComponent<BulletBehaviour>().SetBulletSpeed(BulletSpeed);
            CanShotBullet = Time.time + BulletCooldown;
        }                       
    }
    void LookAtPlayer()
    {
         if(PlayerLocation != null && CanLook == true)
        {
            Vector3 direction = PlayerLocation.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
        }
    }
    #endregion

    #region Status
   void ResetLife()
    {
        Life = MaxLife;
    }
    void HealthCheck()
    {
        if(LifeBar!= null)
        LifeBar.fillAmount = Life / MaxLife;
    }
   public void DamageCheck(float damage)
    {
        if(HasHit && CanTakeDamage)
        {        
            Life -= damage;
            HealthCheck();
            CheckForPoints();
            ChangeSprite();
            HasHit = false;
        }
    }
    void CheckForPoints()
    {
        if(Life <=0)
        {
            AddPoint();
        }
    }
    void AddPoint()
    {
        if(Data != null)
        {
            Data.AddPlayerScore();
        }
    }
    public void SetHit(bool hit)
    {
        HasHit = hit;
    }
    void CheckDeath()
    {
        if(Life <= 0 && !Dead)
        {
            Life = 0;
            CanMove = false;
            CanShoot = false;
            CanLook = false;
            CanExplode = false;
            CanTakeDamage = false;
            
            Invoke("SpawnExplosion",4.5f);
            SpawnFlames();

            Destroy(this.gameObject, 5f);
            Dead = true;
        }

    }
    void CheckPlayerDeath()
    {
        if(PlayerLocation.GetComponent<PlayerBoatBehaviour>().isPlayerDead())
        {
            CanMove = false;
            CanShoot = false;
            CanLook = false;
            CanExplode = false;
            CanTakeDamage = false;
        }
    }

    void ChangeSprite()
    {
        if(Life == MaxLife)
        {
            MySkin.sprite = Type[0];
        }
        else if(Life > 0 && Life <= MaxLife/2)
        {
            MySkin.sprite = Type[1];
        }
        else if(Life <=0)
        {
            MySkin.sprite = Type[2];
        }

    }
    void SpawnExplosion()
    {
        GameObject e = Instantiate(ExplosionEff,new Vector2(this.transform.position.x,this.transform.position.y), Quaternion.identity);
    }
    void SpawnFlames()
    {
         Instantiate(FlameEff, new Vector2(this.transform.position.x,this.transform.position.y +0.4f), Quaternion.identity);
    }
    #endregion

    #region Collision
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player" && isChaser && CanExplode)
        {
            col.GetComponent<PlayerBoatBehaviour>().SetHit(true);
            col.GetComponent<PlayerBoatBehaviour>().DamageCheck(HitDamage);
            SpawnExplosion();
            Destroy(this.gameObject);
        }
      
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.tag == "Player" && isChaser && CanExplode)
        {
            col.transform.GetComponent<PlayerBoatBehaviour>().SetHit(true);
            col.transform.GetComponent<PlayerBoatBehaviour>().DamageCheck(HitDamage);
            SpawnExplosion();
            Destroy(this.gameObject);
        }
      
    }
    #endregion
}
