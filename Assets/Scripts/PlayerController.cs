using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller
public class PlayerController : MonoBehaviour
{
    private const int MAX_POWERUP_AMOUNT = 3;
    
    //Model
    [SerializeField] private BulletSpawner bulletSpawner;
    
    //Bullet template to change and give it to model
    private BulletTemplate _playerBullet;
    //<----------------------------------------------->
    private List<PowerUps> playerPowerUps = new List<PowerUps>();

    public Func<PowerUps,bool> OnPowerUpViewChanged;
    
    private bool isCopy;

    public bool IsCopy
    {
        get => isCopy;
        set => isCopy = value;
    }

    private void Start()
    {
        InstantiatePlayer();
    }

    void InstantiatePlayer()
    {
        if (isCopy) return;
        OnPowerUpViewChanged ??= PowerUpChanger;
        _playerBullet ??= new BulletTemplate();
        Shoot(_playerBullet);
    }

    void Shoot(BulletTemplate bullet)
    {
        bulletSpawner.SpawnBullet(bullet);
    }

    void CopyPlayer()
    {
        transform.position = Vector3.right * 2f;
        var newPlayer = Instantiate(this, Vector3.right * 3, Quaternion.identity);
        newPlayer.IsCopy = true;
        var bulletToPass = _playerBullet;
        newPlayer.Shoot(_playerBullet);
        if(playerPowerUps.Contains(PowerUps.QuickFire))
        {
            bulletToPass.SetFireRate(4f);
            newPlayer.bulletSpawner.ChangeBullet(bulletToPass);
        }
    }

    bool PowerUpChanger(PowerUps powerUps)
    {
        playerPowerUps.Add(powerUps);
        switch (powerUps)
        {
            case PowerUps.TripleForce:
                _playerBullet.SetTriple(true);
                break;
            case PowerUps.AdditionalBullet:
                _playerBullet.SetProjectiles(2);
                break;
            case PowerUps.QuickFire:
                _playerBullet.SetFireRate(2f);
                bulletSpawner.ChangeBullet(_playerBullet);
                break;
            case PowerUps.RapidBullet:
                _playerBullet.SetBulletSpeed(2f);
                break;
            case PowerUps.CopyThat:
                CopyPlayer();
                break;
        }
        return playerPowerUps.Count >= MAX_POWERUP_AMOUNT;
    }
}

public enum PowerUps
{
    TripleForce,
    AdditionalBullet,
    QuickFire,
    RapidBullet,
    CopyThat
}
