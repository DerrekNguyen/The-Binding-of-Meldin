using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Available Bullets")]
    public List<BulletConfig> bulletTypes;

    private int currentBulletIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        bulletTypes = new List<BulletConfig>(Resources.LoadAll<BulletConfig>("Configs/Bullet"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Pew Pew");
            Shoot();
        }
    }

    // Method to shoot a bullet
    void Shoot() {
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        BulletBehavior bullet = bulletObj.GetComponent<BulletBehavior>();
        bullet.Init(bulletTypes[currentBulletIndex]);
    }
}
