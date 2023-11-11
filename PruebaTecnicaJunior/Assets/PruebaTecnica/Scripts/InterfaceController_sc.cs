using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceController_sc : MonoBehaviour
{
    [SerializeField] AudioSource clickSound, selectSound;
    
    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
    }
    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void Pause()
    {
        Time.timeScale = 0.0f;
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
    }
    public void ClickSound()
    {
        clickSound.Play();
    }
    public void SelectSound()
    {
        selectSound.Play();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
