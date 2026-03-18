using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Levels : MonoBehaviour
{
    // Called every time a collectible is picked up
    public void CheckLevelProgress()
    {
        // Find all objects with the "Collectible" tag
        int remainingCollectibles = GameObject.FindGameObjectsWithTag("Collectible").Length;

        // If 1 remains, it means the one just touched is the last one
        if (remainingCollectibles <= 1)
        {
            StartCoroutine(TransitionSequence());
        }
    }

    // Coroutine to handle the visual transition before loading the scene
    IEnumerator TransitionSequence()
    {
        // Find the fader in the scene
        LevelFader fader = FindFirstObjectByType<LevelFader>();
        
        if (fader != null)
        {
            // Wait for the Fade Out animation to finish
            yield return StartCoroutine(fader.FadeOut());
            yield return new WaitForSeconds(0.2f);
        }
        else
        {
            // If no fader is found, just wait a brief moment
            yield return new WaitForSeconds(0.5f);
        }

        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more levels in Build Settings!");
        }
    }
}