using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;

    private ObjectPool<Bullet> _pool;
    private BulletTemplate _incomingTemplate;
    private Tween bulletSpawnTween;

    private void Awake()
    {
        _pool = new ObjectPool<Bullet>(CreateBullet,OnBulletTaken,OnBulletRecycle,OnBulletDestroy,true,100,500);
    }

    public void SpawnBullet(BulletTemplate bulletToSpawn)
    {
        _incomingTemplate = bulletToSpawn;
        bulletSpawnTween = DOVirtual.DelayedCall(_incomingTemplate.GetFireRate(), () =>
        {
            var bulletSpawnPos = transform.position;
            if (_incomingTemplate.GetIsTriple())
            {
                for (int i = 0; i < _incomingTemplate.GetProjectiles(); i++)
                {
                    var bullet = _pool.Get();
                    bullet.transform.position = bulletSpawnPos;
                    bullet.GetRigidbody().velocity = transform.forward * _incomingTemplate.GetBulletSpeed();

                    var bullet1 = _pool.Get();
                    bullet1.transform.position = bulletSpawnPos + (Vector3.right * 0.04f);
                    bullet1.GetRigidbody().velocity = Quaternion.Euler(0, 45, 0) * transform.forward * _incomingTemplate.GetBulletSpeed();

                    var bullet2 = _pool.Get();
                    bullet2.transform.position = bulletSpawnPos + (Vector3.left * 0.04f);
                    bullet2.GetRigidbody().velocity = Quaternion.Euler(0, -45, 0) * transform.forward * _incomingTemplate.GetBulletSpeed();
                    
                    bulletSpawnPos += Vector3.back * 0.04f;
                    
                    DOVirtual.DelayedCall(10f, () => _pool.Release(bullet));
                    DOVirtual.DelayedCall(10f, () => _pool.Release(bullet1));
                    DOVirtual.DelayedCall(10f, () => _pool.Release(bullet2));
                }
            }
            else
            {
                for (int i = 0; i < _incomingTemplate.GetProjectiles(); i++)
                {
                    var bullet = _pool.Get();
                    bullet.GetRigidbody().velocity = transform.forward * _incomingTemplate.GetBulletSpeed();
                    bulletSpawnPos += Vector3.back * 0.04f;
                    DOVirtual.DelayedCall(10f, () => _pool.Release(bullet));
                }
            }
        }).SetLoops(-1);
    }
    
    public void ChangeBullet(BulletTemplate newBullet)
    {
        bulletSpawnTween.DOTimeScale(newBullet.GetFireRate(), newBullet.GetFireRate());
        _incomingTemplate = newBullet;
    }

    #region Object Pool Functions
    
    private Bullet CreateBullet()
    {
        var bulletSpawnPos = transform.position;
        var bullet = Instantiate(bulletPrefab, bulletSpawnPos, Quaternion.identity);

        return bullet;
    }

    private void OnBulletTaken(Bullet bullet)
    {
        bullet.GetRigidbody().velocity = Vector3.zero;
        bullet.transform.position = transform.position;
        
        bullet.gameObject.SetActive(true);
    }

    private void OnBulletRecycle(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnBulletDestroy(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    #endregion
    
}

public class BulletTemplate
{
    private float _bulletSpeed;
    private float _fireRate;
    private int _projectiles;
    private bool _isTriple;
    
    public BulletTemplate()
    {
        _bulletSpeed = 1f;
        _fireRate = 1f;
        _projectiles = 1;
        _isTriple = false;
    }
    
    public void SetBulletSpeed(float bulletSpeed)
    {
        _bulletSpeed = bulletSpeed;
    }
    
    public void SetFireRate(float fireRate)
    {
        _fireRate = fireRate;
    }
    
    public void SetProjectiles(int projectileAmount)
    {
        _projectiles = projectileAmount;
    }
    
    public void SetTriple(bool isTriple)
    {
        _isTriple = isTriple;
    }

    public float GetBulletSpeed()
    {
        return _bulletSpeed;
    }
    
    public float GetFireRate()
    {
        return _fireRate;
    }
    
    public int GetProjectiles()
    {
        return _projectiles;
    }
    
    public bool GetIsTriple()
    {
        return _isTriple;
    }
}