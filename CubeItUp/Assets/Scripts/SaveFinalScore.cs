using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SaveFinalScore : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text name;
    DatabaseReference firebaseDB;
    [SerializeField]
    private Text errorMsg;
    [SerializeField]
    private Button b1;

    private bool ableToAdd = true;
    void Start()
    {
        firebaseDB = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void saveScoreDB()
    {
        StartCoroutine(GetData());
    }
    private IEnumerator GetData()
    {
        var fbTask = firebaseDB.Child("Scoruri").OrderByChild("score").GetValueAsync();

        yield return new WaitUntil(predicate: () => fbTask.IsCompleted);

        if (fbTask.Exception != null)
        {
            Debug.Log("Task Fail!");
        }
        else
        {
            DataSnapshot listaScoruriFB = fbTask.Result;
            foreach (DataSnapshot scor in listaScoruriFB.Children.Reverse<DataSnapshot>())
            {
                if(scor.Child("name").Value.ToString() == name.text.ToString())
                {
                    if (scor.Child("email").Value.ToString() == "")
                    {
                        ableToAdd = false;
                        StartCoroutine(addData());
                        break;                        
                    }else if (scor.Child("email").Value.ToString() != AudioManager.emailLogat)
                    {
                        errorMsg.text = "Username already in use!";
                        StartCoroutine(resetButton());
                        ableToAdd = false;
                        break;
                    }
                }
            }
            if (ableToAdd)
            {
                StartCoroutine(addData());
            }
        }
    }

    private IEnumerator addData()
    {
        b1.gameObject.SetActive(false);
        yield return new WaitForSeconds(10);
        UserScoreClass instance = new UserScoreClass();
        instance.name = name.text;
        instance.score = AudioManager.score;
        instance.rcdrt = AudioManager.intrebariCorecte.ToString() + "/" + AudioManager.numarTotalIntrebari.ToString();
        instance.email = AudioManager.emailLogat;
        string json = JsonUtility.ToJson(instance);
        firebaseDB.Child("Scoruri").Child(instance.name).SetRawJsonValueAsync(json).ContinueWith(task => {

            if (task.IsCanceled)
            {
                Debug.Log("Canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("Faulted");
                return;
            }
            if (task.IsCompleted)
            {
                Debug.Log("Completed!");
                return;
            }
        });
        SceneManager.LoadScene(0);
    }
    private IEnumerator resetButton()
    {
        b1.gameObject.SetActive(false);
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(11);
    }

}
