using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    private PlayerConfig playerConfig;

    [Header("Available Bullets")]
    public List<BulletConfig> bulletTypes;

    private int currentBulletIndex = 0;
    private bool canShoot = true;

    private Controls controls;

    // Start is called before the first frame update
    void Start()
    {
        playerConfig = Resources.Load<PlayerConfig>("Configs/Entity/PlayerConfig");
        bulletTypes = new List<BulletConfig>(Resources.LoadAll<BulletConfig>("Configs/Bullet"));
        controls = new Controls();
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        controls.Player.Shoot.performed += ctx =>
        {
            if (canShoot)
            {
                Shoot();
                StartCoroutine(ShootCooldown());
            }
        };
    }

    // Method to shoot a bullet
    void Shoot() {
        Vector2 facingVector = GetComponent<NewPlayerMovement>().GetFacingVector();
        Quaternion bulletRotation = new Quaternion();
        switch (facingVector)
        {
            case Vector2 v when v == Vector2.up:
                bulletRotation = Quaternion.Euler(0, 0, 180);
                break;
            case Vector2 v when v == Vector2.down:
                bulletRotation = Quaternion.Euler(0, 0, 0);
                break;
            case Vector2 v when v == Vector2.left:
                bulletRotation = Quaternion.Euler(0, 0, -90);
                break;
            case Vector2 v when v == Vector2.right:
                bulletRotation = Quaternion.Euler(0, 0, 90);
                break;
        }

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        BulletBehavior bullet = bulletObj.GetComponent<BulletBehavior>();
        bullet.Init(bulletTypes[currentBulletIndex]);
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(playerConfig.shootCooldown);
        canShoot = true;
    }
}
