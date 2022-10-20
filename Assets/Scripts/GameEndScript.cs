using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using TMPro;

public class GameEndScript : MonoBehaviour, Interactable
{
    [SerializeField] TextMeshProUGUI endGameText;
    [SerializeField] Canvas endGameCanvas;

    private Stopwatch timer;

    private bool isLocked;

    AudioSource audioSource;
    [SerializeField] AudioClip lockedSound;
    [SerializeField] AudioClip openSound;

    // Start is called before the first frame update
    void Start()
    {
        isLocked = true;
        timer = new Stopwatch();
        timer.Start();

        audioSource = GetComponent<AudioSource>();
    }

    public void unlock()
    {
        this.isLocked = false;
    }

    public void FinishGame()
    {
        endGameCanvas.gameObject.SetActive(true);
        this.timer.Stop();
        var time = timer.Elapsed;
        endGameText.text = $"{time.Hours}h {time.Minutes}m {time.Seconds}s {time.Milliseconds / 10}ms";
    }

    public void CloseGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void interact(PlayerController pC = null, GameObject obj = null)
    {
        if (isLocked)
        {
            audioSource.PlayOneShot(lockedSound);
        }
        else
        {
            audioSource.PlayOneShot(openSound);
            pC.enableCursor();
            pC.movementEnabled = false;

            this.FinishGame();
        }
    }
}
