using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class ExitButton : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void CloseWindow();
#endif

    public void ExitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Exit pressed (Editor)");
        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_WEBGL
        Debug.Log("Exit pressed (WebGL)");

        // В браузере нельзя закрыть вкладку напрямую.
        // Можно оставить пусто или сделать переход на сайт.
        // CloseWindow(); // если есть JS плагин

#else
        Debug.Log("Exit pressed (Build)");
        Application.Quit();
#endif
    }
}
