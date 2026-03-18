using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuPanel : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    
    public void PauseGame()
    {
        Time.timeScale=0; //pause the game
        pauseButton.SetActive(false); //desactivates the pause button
        //activate the menu
        pauseMenu.SetActive(true);
    }

    //resume button
    public void ResumeGame()
    {
        Time.timeScale=1; //resumes the game
        pauseButton.SetActive(true); //activates the pause button
        //desactivate the menu
        pauseMenu.SetActive(false);
    }

    //restart button
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restarts the game
        Time.timeScale=1;
    }

    public void QuitGame()
    {
        Debug.Log("close game"); //to ensures it works when not being on the .ex
        Application.Quit();
    }
}
