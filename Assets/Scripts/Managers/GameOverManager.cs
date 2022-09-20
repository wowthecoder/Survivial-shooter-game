using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Button restartbutton;

    Animator anim;
    bool showedButton;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("GameOver");
            if (!showedButton)
            {
                showedButton = true;
                StartCoroutine(ShowButton());
            }
        }
        else
        {
            if (restartbutton.gameObject.activeSelf) 
            {
                showedButton = false;
                restartbutton.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator ShowButton() 
    {
        yield return new WaitForSeconds(4);
        restartbutton.gameObject.SetActive(true);
    }

    public void Restart()
    {
        Debug.Log("Hello");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
