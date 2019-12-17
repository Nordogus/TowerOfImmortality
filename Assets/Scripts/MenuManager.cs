using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [SerializeField]
    private GameObject canvasMenu = null;
    [SerializeField]
    private GameObject gameTitle = null;
    [SerializeField]
    private bool tittleScreen = false;
    [SerializeField]
    private List<GameObject> canvasList = new List<GameObject>();

    //fade variables
    [SerializeField]
    private RawImage cache = null;
    private Color tmpColor;
    [SerializeField]
    private float speedFade = 1;
    [SerializeField]
    private bool inFadeOut = false;
    [SerializeField]
    private bool inFadeIn = false;

    [SerializeField] private bool inPause = false;
    [SerializeField] private Text qwerty;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (GameObject item in canvasList)
        {
            item.SetActive(false);
        }
        cache.gameObject.SetActive(true);

        if (tittleScreen)
        {
            canvasMenu.SetActive(false);
            gameTitle.SetActive(false);
            if (GameManager.instance.option.qwerty)
            {
                qwerty.text = "Qwerty";
            }
            else
            {
                qwerty.text = "Azerty";
            }
        }

        StartManager();
    }

    public void StartManager()
    {
        tmpColor.a = 1;
        if (tittleScreen)
        {
            canvasMenu.SetActive(true);
            gameTitle.SetActive(true);
        }
        inFadeOut = true;
    }

    private void Update()
    {
        Faid();

        if (Input.GetKeyDown(KeyCode.Escape) && !canvasMenu)
        {
            if (!inPause)
            {
                inPause = true;
                PauseGame();
            }else
            {
                inPause = false;
                ResumeGame();
            }
        }
    }

    public void ActivateCanvas(GameObject _canvas)
    {
        foreach (GameObject item in canvasList)
        {
            item.SetActive(false);
        }
        if (tittleScreen)
        {
            gameTitle.SetActive(false);
        }
        _canvas.SetActive(true);
    }

    public void UnactiveCanvas(GameObject _canvas)
    {
        _canvas.SetActive(false);
        if (tittleScreen)
        {
            gameTitle.SetActive(true);
        }
    }

    private void Faid()
    {
        if (inFadeIn)
        {
            FadeIn();
        }
        else if (inFadeOut)
        {
            FadeOut();
        }
    }

    private void FadeIn()
    {
        tmpColor.a += speedFade * Time.deltaTime;
        cache.color = tmpColor;

        if (tmpColor.a >= 1)
        {
            inFadeIn = false;
        }
    }

    private void FadeOut()
    {
        tmpColor.a -= speedFade * Time.deltaTime;
        cache.color = tmpColor;
        if (tmpColor.a <= 0)
        {
            inFadeOut = false;
        }
    }
    
    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void LoadScene(int idScene)
    {
        SceneManager.LoadScene(idScene);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void SetQwerty()
    {
        GameManager.instance.option.qwerty = !GameManager.instance.option.qwerty;

        if (GameManager.instance.option.qwerty)
        {
            qwerty.text = "Qwerty";
        }
        else
        {
            qwerty.text = "Azerty";
        }

        GameManager.instance.SaveSettings();
    }
}
