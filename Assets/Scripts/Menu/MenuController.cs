using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public void Play(){
        SceneManager.LoadScene("Platform");

    }
    public void Controls(){
        SceneManager.LoadScene("Controls");

    }
    public void Quit()
    {
        Application.Quit();
    }
}
