using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Available Bullets")]
    public List<BulletConfig> bulletTypes;

    private int currentBulletIndex = 0;

    private Controls controls;

    // Start is called before the first frame update
    void Start()
    {
        bulletTypes = new List<BulletConfig>(Resources.LoadAll<BulletConfig>("Configs/Bullet"));
        controls = new Controls();
        controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        controls.Player.Shoot.performed += ctx => Shoot();
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
}
