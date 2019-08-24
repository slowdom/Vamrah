using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public CheckpointManager checkpointManager;

    public Image deathScreen;

    private Animator deathUIAnimator;

    private void Start()
    {
        gameManager = this;
        deathUIAnimator = deathScreen.GetComponent<Animator>();
    }

    public void Die(Transform player)
    {
        StartCoroutine(FadeInAndOut(player));
    }

    IEnumerator FadeInAndOut(Transform player)
    {
        player.GetComponent<Animator>().SetBool("Die", true);
        deathUIAnimator.SetBool("Fade", true);

        yield return new WaitUntil(() => deathScreen.color.a > 0.9f);

        player.position = checkpointManager.checkpoints[checkpointManager.currentCheckpoint].transform.position;

        player.GetComponent<Animator>().SetBool("Die", false);
        deathUIAnimator.SetBool("Fade", false);
    }
}
