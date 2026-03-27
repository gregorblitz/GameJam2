using UnityEngine;

public class DangerFlashReset : MonoBehaviour
{
    void Start()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();

        if (cg == null)
            cg = gameObject.AddComponent<CanvasGroup>();

        cg.alpha = 0f; // SIEMPRE inicia invisible
    }
}