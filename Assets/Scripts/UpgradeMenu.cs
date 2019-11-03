using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    private TowerPlacement towerPlacement;
    [SerializeField] int towerCost;
    [SerializeField] int shotCost;
    [SerializeField] int fireRateCost;
    [SerializeField] int maxUpgradesPerLevel;
    private int upgrades;
    [SerializeField] DialogueTrigger dialogueTrigger;
    private GameManager gameManager;
    private int playerTowers;
    private int playerShotDamage;
    [HideInInspector] public bool upgradable;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        towerPlacement = FindObjectOfType<TowerPlacement>();
        SetButtonLabels();
    }

    private void SetButtonLabels()
    {
        TextMeshProUGUI towerButtonText = GameObject.FindGameObjectWithTag("Add Tower Button Text").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI shotButtonText = GameObject.FindGameObjectWithTag("Increase Shot Button Text").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI fireRateButtonText = GameObject.FindGameObjectWithTag("Increase Fire Rate Button Text").GetComponent<TextMeshProUGUI>();
        towerButtonText.text = "cost:" + towerCost.ToString() + " Points";
        shotButtonText.text = "cost:" + shotCost.ToString() + " Points";
        fireRateButtonText.text = "cost:" + fireRateCost.ToString() + " Points";
    }

    public void AddTower()
    {
        if (GameWideData.Instance.maxTowersPossible > GameWideData.Instance.towers)
        {
            if (Upgradable(towerCost))
            {
                towerPlacement.UpdateMaxTowers(towerCost);
                upgrades++;
            }
        }
        else
        {
            upgradable = false;
            dialogueTrigger.SetMessageDirectly("Max Towers");
        }
    }
    public void IncreaseShotPower()
    {
        if (GameWideData.Instance.maxShotDamage > GameWideData.Instance.shotDamage)
        {
            if (Upgradable(shotCost))
            {
                towerPlacement.UpdateShotDamage();
                gameManager.UpdateScore(-shotCost);
                upgrades++;
            }
        }
        else
        {
            upgradable = false;
            dialogueTrigger.SetMessageDirectly("Max Shot Damage");
        }
    }
    public void IncreaseFireRate()
    {
        if (GameWideData.Instance.maxFireRate < GameWideData.Instance.fireRate)
        {
            if (Upgradable(fireRateCost))
            {
                towerPlacement.UpdateFireRate();
                gameManager.UpdateScore(-fireRateCost);
                upgrades++;
            }
        }
        else
        {
            upgradable = false;
            dialogueTrigger.SetMessageDirectly("Max Fire Rate");
        }
    }
    private bool Upgradable(int cost)
    {
        if (gameManager.Cheat)
        {
            return gameManager.Cheat;
        }
        bool hasEnoughMoney = cost <= gameManager.Score;
        bool canUpgrade = upgrades < maxUpgradesPerLevel;
        if (!hasEnoughMoney)
        {
            NotEnougMoneyPlayerMessage();
        }
        if (!canUpgrade)
        {
            MaxUpgradesReached();
        }
        upgradable = hasEnoughMoney && canUpgrade;
        return upgradable;
    }
    private void NotEnougMoneyPlayerMessage()
    {
        dialogueTrigger.SetMessageDirectly("Not Enough Points");
    }
    private void MaxUpgradesReached()
    {
        dialogueTrigger.SetMessageDirectly("Max upgardes reached");
    }
}
