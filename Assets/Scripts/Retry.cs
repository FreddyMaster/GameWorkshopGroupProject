using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Retry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Function to restart the game, linked to the Retry button in the Game Over panel
public void RetryGame()
{
    Time.timeScale = 1; // Resume game time
    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
}

}
