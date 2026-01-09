using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Button için gerekli

public class MainMenuManager : MonoBehaviour
{
    public Button continueButton; // Inspector'dan Devam Et butonunu sürükle

    private void Start()
    {
        // Eğer kayıt dosyası yoksa "Devam Et" butonu pasif olsun
        if (continueButton != null)
        {
            // SaveManager static fonksiyonu ile kontrol ediyoruz
            string path = Application.persistentDataPath + "/savegame.json";
            bool hasSave = System.IO.File.Exists(path);
            
            continueButton.interactable = hasSave; 
            
            // Opsiyonel: Butonu şeffaflaştır
            if (!hasSave) continueButton.GetComponent<CanvasGroup>().alpha = 0.5f;
        }
    }

    public void NewGame()
    {
        // Yeni oyun isteği (Yükleme yapma)
        PlayerPrefs.SetInt("LoadGameRequest", 0);
        SceneManager.LoadScene(1); // GameScene
    }

    public void ContinueGame()
    {
        // Oyun sahnesine "Yükleme Yap" mesajı bırakıyoruz
        PlayerPrefs.SetInt("LoadGameRequest", 1);
        SceneManager.LoadScene(1); // GameScene
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
