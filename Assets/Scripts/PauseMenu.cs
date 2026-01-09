using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false; // Oyun durdu mu?

    public GameObject pauseMenuUI; // Panelimiz
    public MouseLook mouseLook;    // Karakterin kafa scripti

    void Update()
    {
        // ESC tuşuna basınca
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Paneli kapat
        Time.timeScale = 1f;          // Zamanı normal hıza al
        GameIsPaused = false;

        // Mouse'u kilitle ve karakteri hareket ettir
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(mouseLook) mouseLook.canLook = true;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); // Paneli aç
        Time.timeScale = 0f;         // Zamanı DURDUR
        GameIsPaused = true;

        // Mouse'u serbest bırak
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if(mouseLook) mouseLook.canLook = false;
    }

    public void LoadMenu()
    {
        // Menüye dönerken zamanı tekrar açmalıyız, yoksa menü donuk kalır!
        Time.timeScale = 1f; 
        GameIsPaused = false;
        SceneManager.LoadScene("MainMenu"); // Sahne adın neyse onu yaz
    }

    public void QuitGame()
    {
        Debug.Log("Oyundan çıkılıyor...");
        Application.Quit();
    }
}