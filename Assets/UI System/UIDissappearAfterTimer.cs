using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDissappearAfterTimer : MonoBehaviour
{
    public float SecondsToStayOpen;

    // Start is called before the first frame update
    void Start()
    {
        DisappearAfterTime();
	}

    private async void DisappearAfterTime()
    {
        await System.Threading.Tasks.Task.Delay((int)(SecondsToStayOpen * 1000));

        gameObject.SetActive(false);
    }
}
