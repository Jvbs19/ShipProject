using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBoatBehaviour : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] 
    float MaxLife = 15f;
    float Life;
    [SerializeField]  
    float InvensibilityFrames = 1f;
    float CanBeInvensible = 0f;
    [SerializeField]  
    Image LifeBar;
    bool HasHit = false;

    [Header("Moviment Settings")]
    [SerializeField] 
    float MoveSpeed =2.5f;
    [SerializeField]   
    float RotationSpeed =1f;

    [Header("Bullet Settings")]
    [SerializeField] 
    float NormalBulletCooldown = 0.5f;
    [SerializeField]  
    float NormalBulletSpeed = 3f;
    [SerializeField]   
    float NormalBulletDamage = 1f;
    float CanShotNormal =0f;
    [SerializeField] 
    float TripleBulletCooldown = 1.5f;
    [SerializeField] 
    float TripleBulletSpeed = 2f;
    [SerializeField] 
    float TripleBulletDamage = 0.8f;
    float CanShotTriple =0f;
    [SerializeField] 
    GameObject Bullet;
    [SerializeField] 
    GameObject TripleBullet;

    [SerializeField] 
    Transform FrontC, LeftC, RightC;
    
    [Header("Visual Settings")]
    [SerializeField]
    Sprite[] Type;
    [SerializeField]
    SpriteRenderer MySkin;


    bool CanMove = true;
    bool CanShoot = true;
    bool CanTakeDamage = true;
    bool Dead = false;

    void Start()
    {
        ResetLife();
    }
    void Update()
    {
        if(CanShoot)
            Shoot();
    }
    void FixedUpdate()
    {
        if(CanMove)
        {
            Rotate();
            Move();
        }
    }
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
            if(Time.time > CanBeInvensible)
            {
                Life -= damage;
                HealthCheck();
                ChangeSprite();
                CanBeInvensible = Time.time +InvensibilityFrames;
               
            }
            CheckForDeath();
            HasHit = false;
        }
    }
    public void SetHit(bool hit)
    {
        HasHit = hit;
    }
    void CheckForDeath()
    {
        if(Life <= 0)
        {
            CanMove = false;
            CanShoot = false;
            CanTakeDamage = false;
            Dead = true;
        }

    }
    void ChangeSprite()
    {
        if(Life == MaxLife)
        {
            MySkin.sprite = Type[0];
        }
        else if(Life > MaxLife/4 && Life <= MaxLife/2)
        {
            MySkin.sprite = Type[1];
        }
        else if(Life <= MaxLife/4)
        {
            MySkin.sprite = Type[2];
        }

    }
    #endregion

    #region Controls
    void Move()
    {
        if(Input.GetAxis("Vertical") > 0)
            transform.Translate(0, MoveSpeed * Time.fixedDeltaTime * Input.GetAxis("Vertical"),0);
    }
    void Rotate()
    {
         transform.Rotate(0, 0, -(Input.GetAxis("Horizontal") * (RotationSpeed *100) * Time.fixedDeltaTime));
    }
    void Shoot()
    {
      if(Input.GetButtonDown("Shot"))
      {
          if(Time.time > CanShotNormal)
          {
            GameObject b = Instantiate(Bullet);  
            b.transform.position = FrontC.position;
            b.transform.rotation = FrontC.rotation;
            b.GetComponent<BulletBehaviour>().SetBulletDamage(NormalBulletDamage);
            b.GetComponent<BulletBehaviour>().SetBulletSpeed(NormalBulletSpeed);
            CanShotNormal = Time.time + NormalBulletCooldown;
          }
      }
      if(Input.GetButtonDown("LeftShot"))
      {
        if(Time.time > CanShotTriple)
        {
            GameObject b = Instantiate(TripleBullet);  
            b.transform.position = LeftC.position;
            b.transform.rotation = LeftC.rotation;
            for(int i = 0; i< 3; i++)
            {
                b.GetComponent<TripleBulletBehaviour>().Bullets[i].SetBulletDamage(TripleBulletDamage);
                b.GetComponent<TripleBulletBehaviour>().Bullets[i].SetBulletSpeed(TripleBulletSpeed);
            }
          CanShotTriple = Time.time + TripleBulletCooldown;
        }
      }
      if(Input.GetButtonDown("RightShot"))
      {
        if(Time.time > CanShotTriple)
        {
           GameObject b = Instantiate(TripleBullet);  
            b.transform.position = RightC.position;
            b.transform.rotation = RightC.rotation;
            for(int i = 0; i< 3; i++)
            {
                b.GetComponent<TripleBulletBehaviour>().Bullets[i].SetBulletDamage(TripleBulletDamage);
                b.GetComponent<TripleBulletBehaviour>().Bullets[i].SetBulletSpeed(TripleBulletSpeed);
            }
          CanShotTriple = Time.time + TripleBulletCooldown;
        }
      }

    }
    #endregion  

    public bool isPlayerDead()
    {
        return Dead;
    }

}
