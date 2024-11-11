using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject startButton;
    public GameObject quitButton;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void QuitGame()
    {
        if (quitButton != null)
        {
            Application.Quit();
            Debug.Log("Quitting Game");
        }
    }

    public void StartGame()
    {
        if (startButton != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
