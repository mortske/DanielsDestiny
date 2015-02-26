using UnityEngine;
using System.Collections;

public class Farm : MonoBehaviour 
{
    public enum PlantState
    {
        empty,
        growing,
        regrowing,
        finished
    }
    public PlantState plantState { get; set; }
    public GameObject plant;
    public GameObject fruit;
    public Material CactusMaterialPrefab;
    Material curCactusMaterial;
    public MeshRenderer cactusRenderer;
    public GameObject resultPrefab;
    public float growtime;
    public float regrowTime;
    float growth = 0.1f;
    float health;
    public float healthGrowthLimit;
    public float healthAdj;
    public Color healthMax;
    public Color healthMin;
    float healthTick = 1;

    void Start()
    {
        health = 1;
        plantState = PlantState.empty;
        plant.SetActive(false);
        fruit.SetActive(false);
        curCactusMaterial = (Material)Instantiate(CactusMaterialPrefab);
        cactusRenderer.material = curCactusMaterial;
        curCactusMaterial.color = healthMax;
    }

    void OnTriggerEnter(Collider other)
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (plantState == PlantState.empty)
                OnScreenInformationbox.instance.ShowBox("press \"" + InputManager.GetPrimaryKeyName("Pickup") + "\" to plant a seed");
            else if (plantState == PlantState.finished)
                OnScreenInformationbox.instance.ShowBox("press \"" + InputManager.GetPrimaryKeyName("Pickup") + "\" to harvest");
            else
                OnScreenInformationbox.instance.ShowBox("press \"" + InputManager.GetPrimaryKeyName("Pickup") + "\" to water");
            if (InputManager.GetKeyDown("Pickup"))
            {
                if (plantState == PlantState.empty)
                    Plant();
                else if (plantState == PlantState.finished)
                    Harvest();
                else
                    Water(0.3f);
            }
        }
    }

    void Plant()
    {
        foreach (GameObject slotgo in Player.instance.inventory.AllSlots)
        {
            Slot slot = slotgo.GetComponent<Slot>();
            if (slot.Items.Count > 0)
            {
                if (slot.CurrentItem.Name == "Cactusfruit")
                {
                    OnScreenInformationbox.instance.HideBox();
                    slot.RemoveItem();
                    plant.transform.localScale = new Vector3(growth, growth, growth);
                    plantState = PlantState.growing;
                    plant.SetActive(true);
                    StartCoroutine(GrowthTick());
                    StartCoroutine(HealthTick());
                    return;
                }
            }
        }
        MessageBox.instance.SendMessage("I need a cactusfruit to plant");
    }

    IEnumerator GrowthTick()
    {
        float from = 0.1f;
        float to = 1f;
        float t = 0;
        while (t < 1)
        {
            yield return null;
            t += (Time.deltaTime / growtime) * CanGrow();
            growth = Mathf.Lerp(from, to, t);
            plant.transform.localScale = new Vector3(growth, growth, growth);
        }
        plantState = PlantState.finished;
        fruit.SetActive(true);
    }

    IEnumerator ReGrow()
    {
        float time = regrowTime;
        while (time > 0)
        {
            yield return null;
            time -= Time.deltaTime * CanGrow();
        }
        plantState = PlantState.finished;
        fruit.SetActive(true);
    }

    int CanGrow()
    {
        int adj = 1;
        if (health < healthGrowthLimit) adj = 0;
        return adj;
    }

    void Harvest()
    {
        fruit.SetActive(false);
        plantState = PlantState.regrowing;

        GameObject go = (GameObject)Instantiate(resultPrefab);
        Item newItem = go.GetComponentInChildren<Item>();
        go.name = resultPrefab.name;
        Player.instance.inventory.AddItem(newItem);
        go.transform.position = Player.instance.transform.position;
        go.transform.gameObject.SetActive(false);
        go.transform.parent = Player.instance.transform;
        StartCoroutine(ReGrow());
    }

    IEnumerator HealthTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            health -= healthAdj;
            curCactusMaterial.color = Color.Lerp(healthMin, healthMax, health);
            if (health <= 0)
            {
                health = 1;
                plantState = PlantState.empty;
                plant.SetActive(false);
                fruit.SetActive(false);
                curCactusMaterial.color = healthMax;
                StopAllCoroutines();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            OnScreenInformationbox.instance.HideBox();
        }
    }

    public void Water(float adj)
    {
        health += adj;
    }
}
