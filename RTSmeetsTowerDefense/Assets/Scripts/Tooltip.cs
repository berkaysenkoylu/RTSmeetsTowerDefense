using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Tooltip : MonoBehaviour
{
    public float offset = 2f;
    public Text titleLabel;
    public RectTransform canvasRectTransform;

    RectTransform rectTransform;
    Vector3 initialPosition;
    Vector3 tooltipOffset;
    string path;
    string jsonString;

    List<TowerEntity> towerEntities = new List<TowerEntity>();

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        initialPosition = rectTransform.position;
    }

    private void OnDisable()
    {
        rectTransform.position = initialPosition;
    }

    void Update()
    {
        FollowMouse();
    }

    void FollowMouse()
    {
        tooltipOffset = new Vector3(canvasRectTransform.localScale.x * rectTransform.rect.width / 2 + offset, 
                                    canvasRectTransform.localScale.y * rectTransform.rect.height / 2 + offset);

        rectTransform.position = Input.mousePosition + tooltipOffset;
    }

    public void InitializeTooltip(string type)
    {
        ReadDataFromJSON();

        foreach(TowerEntity entity in towerEntities)
        {
            if(entity.type == type)
            {
                PopulateTooltipFields(entity.type, entity.cost, entity.range, entity.damage, entity.fire_rate);
            }
        }
    }

    public void ReadDataFromJSON()
    {
        var info = new DirectoryInfo(Application.streamingAssetsPath);
        var fileInfo = info.GetFiles();
        foreach (FileInfo file in fileInfo)
        {
            string[] parsedFilePath = file.ToString().Split(char.Parse("."));
            if (parsedFilePath[parsedFilePath.Length - 1] == "meta")
            {
                continue;
            }

            jsonString = File.ReadAllText(file.ToString());

            TowerEntity newEntity = JsonUtility.FromJson<TowerEntity>(jsonString);

            towerEntities.Add(newEntity);
        }
    }

    void PopulateTooltipFields(string name, int cost, float range, float damage, float rateOfFire)
    {
        int fieldCount = transform.childCount;

        transform.GetChild(0).GetComponent<Text>().text = "<b>" + name.ToUpper() + " TOWER</b>";

        switch (name)
        {
            case "wood":
                transform.GetChild(1).GetComponent<Text>().text = "<b>Cost: </b>" + cost.ToString() + " logs";
                break;
            case "rock":
                transform.GetChild(1).GetComponent<Text>().text = "<b>Cost: </b>" + cost.ToString() + " rocks";
                break;
            default:
                transform.GetChild(1).GetComponent<Text>().text = "<b>Cost: </b>" + cost.ToString();
                Debug.Log("Check your tower type in your corresponding JSON file.");
                break;
        }

        transform.GetChild(2).GetComponent<Text>().text = "<b>Range: </b>" + range.ToString();
        transform.GetChild(3).GetComponent<Text>().text = "<b>Damage: </b>" + damage.ToString();
        transform.GetChild(4).GetComponent<Text>().text = "<b>Fire Rate: </b>" + rateOfFire.ToString();
    }
}

public class TowerEntity
{
    public string type;
    public int cost;
    public float range;
    public float damage;
    public float fire_rate;
}