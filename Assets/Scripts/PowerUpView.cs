using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpView : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    
    [SerializeField] private Button TripleForceButton;
    [SerializeField] private Button AdditionalBulletButton;
    [SerializeField] private Button QuickFireButton;
    [SerializeField] private Button RapidBulletButton;
    [SerializeField] private Button CopyThatButton;

    private List<Button> activeButtons = new List<Button>();


    private void Awake()
    {
        activeButtons.Add(TripleForceButton);
        activeButtons.Add(AdditionalBulletButton);
        activeButtons.Add(QuickFireButton);
        activeButtons.Add(RapidBulletButton);
        activeButtons.Add(CopyThatButton);

        
        TripleForceButton.onClick.AddListener(()=>OnButtonClicked(PowerUps.TripleForce,TripleForceButton));
        AdditionalBulletButton.onClick.AddListener(()=>OnButtonClicked(PowerUps.AdditionalBullet,AdditionalBulletButton));
        QuickFireButton.onClick.AddListener(()=>OnButtonClicked(PowerUps.QuickFire,QuickFireButton));
        RapidBulletButton.onClick.AddListener(()=>OnButtonClicked(PowerUps.RapidBullet,RapidBulletButton));
        CopyThatButton.onClick.AddListener(()=>OnButtonClicked(PowerUps.CopyThat,CopyThatButton));
    }

    private void OnButtonClicked(PowerUps powerUpType, Button button)
    {
        bool returnedValue = false;
        switch (powerUpType)
        {
            case PowerUps.TripleForce:
                returnedValue = playerController.OnPowerUpViewChanged(PowerUps.TripleForce);
                break;
            case PowerUps.AdditionalBullet:
                returnedValue = playerController.OnPowerUpViewChanged(PowerUps.AdditionalBullet);
                break;
            case PowerUps.QuickFire:
                returnedValue = playerController.OnPowerUpViewChanged(PowerUps.QuickFire);
                break;
            case PowerUps.RapidBullet:
                returnedValue = playerController.OnPowerUpViewChanged(PowerUps.RapidBullet);
                break;
            case PowerUps.CopyThat:
                returnedValue = playerController.OnPowerUpViewChanged(PowerUps.CopyThat);
                break;
        }
        button.interactable = false;
        activeButtons.Remove(button);

        if (!returnedValue) return;
        foreach (var activeButton in activeButtons)
            activeButton.interactable = false;
        activeButtons.Clear();

    }
}
