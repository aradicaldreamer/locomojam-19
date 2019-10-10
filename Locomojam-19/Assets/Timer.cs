using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timer;
    private float timerTime;
    [SerializeField]
    private float currentTime;
    private int minutes, seconds;
	public int timeLeft;



	// Start is called before the first frame update
    void Start()
    {
        StartCoroutine("LoseTime");
		Time.timeScale = 1;
		minutes = 0;
    }

    // Update is called once per frame
    void Update()
    {
		//minutes = (int)(currentTime / 60);
        //seconds = (int)(currentTime % 60);
		seconds = timeLeft;
        timer.text = (minutes < 10 ? "0" : "") + minutes + ":" + (seconds < 10? "0" : "") + seconds;
        
		if (timeLeft == 0)
		{
			StartCoroutine("EndGame");	
		}
    }

	IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }

	IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1);
		SceneManager.LoadSceneAsync("Credits", LoadSceneMode.Single);
    }
	
}
