using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelFader : MonoBehaviour
{
    private Image img;

    void Start()
    {
        img = GetComponent<Image>();
        // Start the level with a transparent screen
        img.color = new Color(0, 0, 0, 0);
        img.raycastTarget = false; // So it doesn't block clicks while transparent
    }

    // Coroutine to fade the screen to black
    public IEnumerator FadeOut()
    {
        img.raycastTarget = true; // Block input during transition
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime; // Fades over approximately 1 second
            img.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}