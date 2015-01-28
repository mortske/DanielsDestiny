using UnityEngine;
using System.Collections;

public class EdgeManager : MonoBehaviour 
{
    bool started = false;

    public int countDownTime;
    public int deathtick;
    public int damage;
    Status playerStatus;

    public void PlayerEntered(Collider player)
    {
        if (!started)
        {
            Debug.Log("started death timer");
            started = true;
            StartCoroutine(StartCountdown());
            playerStatus = player.GetComponent<Status>();
        }
    }

    public void PlayerLeft()
    {
        StopAllCoroutines();
        started = false;
    }

    IEnumerator StartCountdown()
    {
        for (int i = countDownTime; i >= 0; i--)
        {
            MessageBox.instance.SendMessage("Its too dangerous, you are going to die if you dont turn back! " + (i));
            yield return new WaitForSeconds(1);
        }
        MessageBox.instance.SendMessage("I have to turn back im dying out here!");
        while (playerStatus.health.cur > 0)
        {
            playerStatus.health.adjustCur(-damage);
            yield return new WaitForSeconds(deathtick);
        }
    }
}
