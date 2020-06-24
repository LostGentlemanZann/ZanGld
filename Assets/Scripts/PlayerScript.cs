using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Animator animator;

    [Tooltip("Speed of the player")]
    public float MovementSpeed;

    [Tooltip("Shooting rate of the player")]
    public float ShootingRate;

    [Tooltip("Damage on enemy on each hit")]
    public int ShootingDamage;

    [Tooltip("Starting health of the enemy")]
    public int HealthPoint;

    [Tooltip("Starting ammo of the enemy")]
    public int AmmoCount;

    [Tooltip("Player bullet prefeb")]
    public GameObject PlayerBullet;

    [Tooltip("Shooting sound effect")]
    public AudioClip ShootingAudioClip;


    private Rigidbody rb = null;
    private Vector3 moveDirection = Vector3.zero;
    private bool canShoot;
    private AudioSource audioSource;
    public GameObject gunSpawn;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        canShoot = true;
        audioSource.clip = ShootingAudioClip;

        GameManager.Instance.UpdateAmmo(AmmoCount);
        GameManager.Instance.UpdateHealth(HealthPoint);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameOver)
            return;

        UpdateMovement();
        UpdateRotation();
        Shoot();

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) 
       {
           animator.SetBool("IsWalking", true);
       }
       else if (!Input.anyKey)
       {
           animator.SetBool("IsWalking", false);
       }
    }

    private void UpdateMovement()
    {
        // Get the direction based on the user input
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveDirection.Normalize();

        // Set the velocity to the direction * movement speed
        rb.velocity = new Vector3(moveDirection.x * MovementSpeed, 
                                  rb.velocity.y, 
                                  moveDirection.z * MovementSpeed);
    }

    private void UpdateRotation()
    {
        // The step size is dependent on the delta time.
        float step = MovementSpeed * 3 * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, moveDirection, step, 0.0f);

        // Rotate our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);
    }
    
    private void Shoot()
    {
        if (Input.GetButton("Fire1") && canShoot && AmmoCount > 0) {
            StartCoroutine(SpawnBullet());
        }
    }

    private IEnumerator SpawnBullet()
    {
        GameObject bullet = Instantiate(PlayerBullet, gunSpawn.transform.position + transform.forward, Quaternion.identity);
        bullet.transform.forward = transform.forward;
        bullet.GetComponent<BulletScript>().BulletDamage = ShootingDamage;
        audioSource.PlayOneShot(ShootingAudioClip);

        GameManager.Instance.UpdateAmmo(--AmmoCount);

        canShoot = false;
        //wait for some time
        yield return new WaitForSeconds(ShootingRate);

        canShoot = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            EnemyScript enemyScript = collision.gameObject.GetComponent<EnemyScript>();
            HealthPoint -= enemyScript.ContactDamage;
            GameManager.Instance.UpdateHealth(HealthPoint);

            if (HealthPoint <= 0) {
                Dead();
            }

            
        }
    }

    


    private void Dead() {
        Destroy(gameObject);
    }

    public void AddAmmo(int ammo, AudioClip audioClip) {
        AmmoCount += ammo;
        GameManager.Instance.UpdateAmmo(AmmoCount);

        audioSource.PlayOneShot(audioClip);
    }

    public void AddHealth(int health, AudioClip audioClip)
    {
        HealthPoint += health;
        GameManager.Instance.UpdateHealth(HealthPoint);

        audioSource.PlayOneShot(audioClip);
    }
}
