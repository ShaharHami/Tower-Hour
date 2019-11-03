using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] Tower tower;
    [SerializeField] float offsetY;
    int shotDamage;
    float fireRate;
    int maxTowers = 4;
    private int maxTowersPossible = 6;
    private TowerComplex last;
    private Tower[] towers;
    Queue<TowerComplex> queue = new Queue<TowerComplex>();
    [SerializeField] GameObject towersDisplay;
    [SerializeField] GameObject displayTower;
    private List<GameObject> displayTowers;
    [SerializeField] TextMeshProUGUI shotPowerText;
    [SerializeField] TextMeshProUGUI towersUsedDisplay;
    [SerializeField] TextMeshProUGUI fireRateText;
    private void Start()
    {
        maxTowersPossible = GameWideData.Instance.maxTowersPossible;
        maxTowers = GameWideData.Instance.towers;
        shotDamage = GameWideData.Instance.shotDamage;
        fireRate = GameWideData.Instance.fireRate;
        shotPowerText.text = shotDamage.ToString();
        fireRateText.text = fireRate.ToString();
        displayTowers = new List<GameObject>();
        UpdateTowerDisplay(maxTowers);
    }
    public int ShotDamage
    {
        get { return shotDamage; }
    }
    public void PlaceTower(Waypoint block)
    {
        if (queue.Count < maxTowers)
        {
            InstantiateTower(block);
            UpdateTowerDisplay(maxTowers - queue.Count);
        }
        else
        {
            MoveTower(block);
        }
    }
    public void UpdateMaxTowers(int cost)
    {
        if (maxTowers < maxTowersPossible)
        {
            maxTowers++;
            GameWideData.Instance.towers = maxTowers;
            FindObjectOfType<GameManager>().UpdateScore(-cost);
            UpdateTowerDisplay(maxTowers - queue.Count);
        }
    }
    public void UpdateShotDamage()
    {
        if (shotDamage < GameWideData.Instance.maxShotDamage)
        {
            shotDamage++;
            GameWideData.Instance.shotDamage = shotDamage;
            towers = FindObjectsOfType<Tower>();
            foreach (Tower tower in towers)
            {
                tower.ShotDamage = shotDamage;
            }
            shotPowerText.text = shotDamage.ToString();
        }
    }
    public void UpdateFireRate()
    {
        if (fireRate > GameWideData.Instance.maxFireRate)
        {
            fireRate -= 0.1f;
            fireRate = Mathf.Round(fireRate * 100.0f) / 100f;
            GameWideData.Instance.fireRate = fireRate;
            towers = FindObjectsOfType<Tower>();
            foreach (Tower tower in towers)
            {
                tower.FireRate = fireRate;
            }
            fireRateText.text = fireRate.ToString();
        }
    }
    private void MoveTower(Waypoint block)
    {
        last = queue.Dequeue();
        last.tower.transform.position = TowerPos(block);
        last.origin.isPlacable = true;
        last.origin = block;
        queue.Enqueue(last);
    }
    private Vector3 TowerPos(Waypoint block)
    {
        return new Vector3(
                        block.transform.position.x,
                        block.transform.position.y + offsetY,
                        block.transform.position.z
                    );
    }
    private void InstantiateTower(Waypoint block)
    {
        Tower towerInstance = Instantiate(tower, TowerPos(block), Quaternion.identity);
        towerInstance.transform.SetParent(transform);
        towerInstance.ShotDamage = shotDamage;
        TowerComplex towerComplex = new TowerComplex(block, towerInstance);
        queue.Enqueue(towerComplex);
    }
    private class TowerComplex
    {
        public Waypoint origin;
        public Tower tower;
        public TowerComplex(Waypoint towerOrigin, Tower towerInstance)
        {
            origin = towerOrigin;
            tower = towerInstance;
        }
    }
    private void UpdateTowerDisplay(int towers)
    {
        while (maxTowers > displayTowers.Count)
        {
            GameObject newTower = Instantiate(displayTower, towersDisplay.transform);
            if (displayTowers.Count == 0)
            {
                newTower.transform.localPosition = Vector3.zero;
            }
            else
            {
                Transform previousTower = displayTowers[displayTowers.Count - 1].transform;
                float towerHeight = previousTower.GetComponent<RectTransform>().rect.height;
                newTower.transform.localPosition = new Vector3(
                                    previousTower.transform.localPosition.x,
                                    previousTower.transform.localPosition.y - towerHeight,
                                    previousTower.transform.localPosition.z
                                );
            }
            displayTowers.Add(newTower);
        }
        for (int i = 0; i < towers; i++)
        {
            SetOpacity(displayTowers[i].transform, 1.0f);
        }
        for (var i = maxTowers - towers; i > 0; i--)
        {
            SetOpacity(displayTowers[displayTowers.Count - i].transform, 0.2f);
        }
        towersUsedDisplay.text = towers.ToString() + "/" + maxTowers.ToString();
    }
    private static void SetOpacity(Transform tower, float opacity)
    {
        RawImage rawImage = tower.GetComponent<RawImage>();
        var color = rawImage.color;
        color.a = opacity;
        rawImage.color = color;
    }
}
