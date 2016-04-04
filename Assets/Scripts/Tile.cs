using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public enum Status
    {
        EMPTY, PLANTED, GROWN, HARVESTED
    }
    public enum ObjectType
    {
        NONE, CARROT, POTATOES, LETTUCE
    }

    public Status status = Status.EMPTY;

    public Transform waterIconPrefab;
    public Transform smileIconPrefab;
    public Transform harvestIconPrefab;
    public Transform carrotPrefab;
    public Transform lettucePrefab;
    public Transform potatoesPrefab;
    private float waterClock = 0f;
    public float waterInterval = 3f;
    public float waterDelay = 3f;
    public int waterNeeded = 2;
    public bool needsWater = false;
    public bool beingUsed = false;

    private Transform objectInstance;
    private GameObject waterIcon;
    private GameObject harvestIcon;
    private GameObject smileIcon;

    public ObjectType bonusType = ObjectType.NONE;
    public ObjectType plantedType = ObjectType.NONE;

    void Update()
    {
        if(status == Status.PLANTED)
        {
            if(!beingUsed)
                waterClock += Time.deltaTime;

            if (waterClock > waterInterval)
            {
                needsWater = true;
                waterIcon.SetActive(true);
                smileIcon.SetActive(false);
                if (waterClock > waterInterval + waterDelay / 2f)
                    waterIcon.GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (waterClock > waterInterval + waterDelay)
            {
                status = Status.EMPTY;
                Destroy(objectInstance.gameObject);
                Destroy(waterIcon);
                Destroy(smileIcon);
            }
        }
    }

    public void Water()
    {
        needsWater = false;
        waterIcon.SetActive(false);
        smileIcon.SetActive(true);
        waterClock = 0f;
        waterNeeded--;
        if(waterNeeded <= 0)
        {
            status = Status.GROWN;
            Destroy(waterIcon);
            var harvestIconInstance = Instantiate(harvestIconPrefab) as Transform;
            harvestIconInstance.SetParent(transform);
            harvestIconInstance.localPosition = harvestIconPrefab.localPosition;
            harvestIconInstance.localRotation = harvestIconPrefab.localRotation;
            harvestIcon = harvestIconInstance.gameObject;
            smileIcon.SetActive(false);
        }
    }

    public void AddCarrot()
    {
        AddPrefab(carrotPrefab);
        plantedType = ObjectType.CARROT;
    }
    public void AddLettuce()
    {
        AddPrefab(lettucePrefab);
        plantedType = ObjectType.LETTUCE;
    }
    public void AddPotatoes()
    {
        AddPrefab(potatoesPrefab);
        plantedType = ObjectType.POTATOES;
    }
    
    public void AddPrefab(Transform prefab)
    {
        objectInstance = Instantiate(prefab) as Transform;
        objectInstance.SetParent(transform);
        objectInstance.localPosition = prefab.localPosition;
        objectInstance.rotation = prefab.rotation;
        status = Status.PLANTED;
        StartCoroutine(Coroutines.Move(objectInstance, objectInstance.position + Vector3.up * 0.5f, objectInstance.position, 0.5f, Easings.BackEaseIn));
        StartCoroutine(Coroutines.Fade(objectInstance.GetComponent<MeshRenderer>(), 0f, 1f, 0.25f));

        var waterIconInstance = Instantiate(waterIconPrefab) as Transform;
        waterIconInstance.SetParent(transform);
        waterIconInstance.localPosition = waterIconPrefab.localPosition;
        waterIconInstance.localRotation = waterIconPrefab.localRotation;
        waterIcon = waterIconInstance.gameObject;
        waterIcon.SetActive(false);
        waterClock = 0f;

        var smileIconInstance = Instantiate(smileIconPrefab) as Transform;
        smileIconInstance.SetParent(transform);
        smileIconInstance.localPosition = smileIconPrefab.localPosition;
        smileIconInstance.localRotation = smileIconPrefab.localRotation;
        smileIcon = smileIconInstance.gameObject;
    }

    public void Harvest()
    {
        status = Status.HARVESTED;

        switch (plantedType)
        {
            case ObjectType.CARROT: GameManager.instance.scoreManager.AddPoints(plantedType, bonusType, 15); break;
            case ObjectType.LETTUCE: GameManager.instance.scoreManager.AddPoints(plantedType, bonusType, 35); break;
            case ObjectType.POTATOES: GameManager.instance.scoreManager.AddPoints(plantedType, bonusType, 50); break;
        }

        Destroy(harvestIcon);
        StartCoroutine(Coroutines.Move(objectInstance, objectInstance.position, objectInstance.position + Vector3.up * 0.5f, 0.5f, Easings.BackEaseOut));
        StartCoroutine(Coroutines.Fade(objectInstance.GetComponent<MeshRenderer>(), 1f, 0f, 0.25f));
        GetComponent<MeshRenderer>().material.color = new Color(0.75f, 0.75f, 0.75f);
    }
}
