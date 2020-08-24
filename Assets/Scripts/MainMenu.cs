using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string voteUrl = "https://rebrand.ly/innerdemons";
    [SerializeField] string facebookUrl = "https://www.facebook.com/InnerDemonsGame/";
    [SerializeField] string twitterUrl = "https://twitter.com/InnerDemonsGame";

    [SerializeField] Animator cutscene;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Vote()
    {
        Application.OpenURL(voteUrl);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Facebook()
    {
        Application.OpenURL(facebookUrl);
    }

    public void Twitter()
    {
        Application.OpenURL(twitterUrl);
    }

    private void Update()
    {
        if (cutscene && cutscene.gameObject.activeInHierarchy && Input.GetMouseButton(0))
        {
            cutscene.Play("After");
        }
    }
}