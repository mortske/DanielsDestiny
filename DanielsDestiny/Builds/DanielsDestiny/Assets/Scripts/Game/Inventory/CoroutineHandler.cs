using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoroutineHandler : MonoBehaviour 
{
    public static CoroutineHandler instance;
	// Use this for initialization
	void Start () {
        instance = this;
	}

    public void WaitForDialouge(Slot from, Slot to, DialougeBoxInv db, GameObject hover)
    {
        StartCoroutine(WaitForDialougeRoutine(from, to, db, hover));
    }

    IEnumerator WaitForDialougeRoutine(Slot from, Slot to, DialougeBoxInv db, GameObject hover)
    {
        while (!db.isDone)
        {
            yield return null;
        }
        int splitCount = db.cur;
        int fromCount = from.Items.Count - splitCount;

        Player player = Player.instance;
        GameObject go = (GameObject)Instantiate(from.CurrentItem.gameObject.transform.parent.gameObject);
        go.transform.position = player.transform.position;
        go.SetActive(true);
        go.transform.parent = player.transform;

        Item item = go.GetComponentInChildren<Item>();
        item.curSize = splitCount;
        
        for (int i = 0; i < item.curSize; i++)
        {
            player.inventory.AddItem(item);
        }

        go.name = from.CurrentItem.gameObject.transform.parent.name;
        go.SetActive(false);

        for (int i = 0; i < splitCount; i++)
        {
            from.Items.Pop();
            from.AddItems(from.Items);
        }

        from.GetComponent<Image>().color = Color.white;
        to = null;
        from = null;
        hover = null;
    }

    public void DropItemDialouge()
    {
        StartCoroutine(DropItemDialougeRoutine());
    }

    IEnumerator DropItemDialougeRoutine()
    {
        yield return null;
    }
}
