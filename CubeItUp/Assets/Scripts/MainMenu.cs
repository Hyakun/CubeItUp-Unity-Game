using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System.Linq;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    DatabaseReference firebaseDB;
    private Transform scoreZone;
    private Transform template;

    private void Awake()
    {
        if (AudioManager.LoggedIn == true)
        {
            try
            {
                firebaseDB = null;
                scoreZone = transform.Find("ScoreZone");
                template = scoreZone.Find("HighScoreTemplate");
                template.gameObject.SetActive(false);
            }
            catch { }
        }
    }

    private void Start()
    {
        if (AudioManager.LoggedIn == true)
        {
            firebaseDB = FirebaseDatabase.DefaultInstance.RootReference;
            StartCoroutine(GetData());
        }
        else
        {
            try
            {
                transform.Find("NameLabel").gameObject.SetActive(false);
                transform.Find("ScoreLabel").gameObject.SetActive(false);
                transform.Find("ScoreZone").gameObject.SetActive(false);
                transform.Find("AnsLabel").gameObject.SetActive(false);
            }
            catch{}
        }
    }

    private IEnumerator GetData()
    {
        yield return new WaitForSeconds(10);
        var fbTask = firebaseDB.Child("Scoruri").OrderByChild("score").GetValueAsync();

        yield return new WaitUntil(predicate: () => fbTask.IsCompleted);

        if(fbTask.Exception != null)
        {
            Debug.Log("Task Fail!");
        }
        else
        {
            DataSnapshot listaScoruriFB = fbTask.Result;
            int i = 0;
            int j = 0;
            foreach (DataSnapshot scor in listaScoruriFB.Children.Reverse<DataSnapshot>())
            {
                if ( j >= 10)
                {
                    break;
                }
                j++;
                Transform scoreTransform = Instantiate(template, scoreZone);
                RectTransform scoreRectTransform = scoreTransform.GetComponent<RectTransform>();
                scoreRectTransform.anchoredPosition = new Vector2(0, -35 * i);
                i++;
                scoreTransform.gameObject.SetActive(true);
                scoreTransform.Find("Name").GetComponent<Text>().text = scor.Child("name").Value.ToString();
                scoreTransform.Find("Score").GetComponent<Text>().text = scor.Child("score").Value.ToString();
                scoreTransform.Find("Ans").GetComponent<Text>().text = scor.Child("rcdrt").Value.ToString();
            }

        }
    }

    public void tutorialStart()
    {
        FindObjectOfType<AudioManager>().Play("Buttons");
        AudioManager.score = 0;
        AudioManager.numarTotalIntrebari = 0;
        AudioManager.intrebariCorecte = 0;
        SceneManager.LoadScene(1);
    }
    public void start()
    {
        FindObjectOfType<AudioManager>().Play("Buttons");
        AudioManager.score = 0;
        AudioManager.numarTotalIntrebari = 0;
        AudioManager.intrebariCorecte = 0;
        SceneManager.LoadScene(2);
    }

    public void startEndless()
    {
        FindObjectOfType<AudioManager>().Play("Buttons");
        SceneManager.LoadScene("EndLess");
    }

    public void restart()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            AudioManager.score = 0;
            AudioManager.numarTotalIntrebari = 0;
            AudioManager.intrebariCorecte = 0;
            FindObjectOfType<AudioManager>().Play("Buttons");
            SceneManager.LoadScene(1);
        }else if(SceneManager.GetActiveScene().name == "EndLess")
        {
            FindObjectOfType<AudioManager>().Play("Buttons");
            SceneManager.LoadScene("EndLess");
        }else
        {
            FindObjectOfType<AudioManager>().Play("Buttons");
            AudioManager.score = 0;
            AudioManager.numarTotalIntrebari = 0;
            AudioManager.intrebariCorecte = 0;
            SceneManager.LoadScene(2);
        }    

        
    }

    public void home()
    {
        AudioManager.score = 0;
        AudioManager.numarTotalIntrebari = 0;
        AudioManager.intrebariCorecte = 0;
        FindObjectOfType<AudioManager>().Play("Buttons");
        SceneManager.LoadScene(0);
    }
    public void login()
    {
        FindObjectOfType<AudioManager>().Play("Buttons");
        SceneManager.LoadScene("LoginRegister");
    }
}
