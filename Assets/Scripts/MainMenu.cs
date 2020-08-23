using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string url = "https://starkkoder.itch.io/dads-comming";
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Vote()
    {
        Application.OpenURL(url);
    }

    public void Quit()
    {
        Application.Quit();
    }
}