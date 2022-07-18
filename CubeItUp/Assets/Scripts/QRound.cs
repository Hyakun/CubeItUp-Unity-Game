using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;



public class QRound : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string intrebare;
        public string raspuns1;
        public string raspuns2;
        public string raspuns3;
        public string raspunsC;
    }
    [System.Serializable]
    public class IntrebariArr
    {
        public Question[] Intrebari;
    }
    [SerializeField]
    private Text QuestionText;
    [SerializeField]
    private Text A1Text;
    [SerializeField]
    private Text A2Text;
    [SerializeField]
    private Text A3Text;
    [SerializeField]
    private Text ACText;
    [SerializeField]
    private Text RaspunsCorect;

    [SerializeField]
    private Button b1;
    [SerializeField]
    private Button b2;
    [SerializeField]
    private Button b3;
    [SerializeField]
    private Button br;

    GameObject joy;
    GameObject jump;
    GameObject dash;


    public IntrebariArr questions = new IntrebariArr();
    public new Vector2 tempGO;

    private int numarIntrebare;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.numarTotalIntrebari++;
        string jsonfile = "intrebariFile.json";
        joy = GameObject.FindGameObjectWithTag("joy");
        jump = GameObject.FindGameObjectWithTag("jump");
        dash = GameObject.FindGameObjectWithTag("dash");

        joy.SetActive(false);
        jump.SetActive(false);
        dash.SetActive(false);

        Vector3[] pozitiiButon = new[] { b1.transform.position, b2.transform.position, b3.transform.position, br.transform.position };

        for (int j = 0; j < pozitiiButon.Length; j++)
        {
            int rnd = Random.Range(0, pozitiiButon.Length);
            tempGO = pozitiiButon[rnd];
            pozitiiButon[rnd] = pozitiiButon[j];
            pozitiiButon[j] = tempGO;
        }

        b1.transform.position = pozitiiButon[0];
        b2.transform.position = pozitiiButon[1];
        b3.transform.position = pozitiiButon[2];
        br.transform.position = pozitiiButon[3];
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 2:
                jsonfile = "intrebari1.json";
                break;
            case 3:
                jsonfile = "intrebari2.json";
                break;
            case 4:
                jsonfile = "intrebari3.json";
                break;
            case 5:
                jsonfile = "intrebari4.json";
                break;
            case 6:
                jsonfile = "intrebari5.json";
                break;
            case 7:
                jsonfile = "intrebari6.json";
                break;
        }

        //Application.streamingAssetsPath + "/" + jsonfile
        StartCoroutine(GetDataInAndroid(jsonfile));
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void wrongAnswer()
    {
        RaspunsCorect.text = "Raspuns corect: " + questions.Intrebari[numarIntrebare].raspunsC;
        AudioManager.score -= 50;
        b1.gameObject.SetActive(false);
        b2.gameObject.SetActive(false);
        b3.gameObject.SetActive(false);
        br.gameObject.SetActive(false);
        StartCoroutine(closeQuestion());
    }

    public void rightAnswer()
    {
        AudioManager.intrebariCorecte++;
        joy.SetActive(true);
        jump.SetActive(true);
        dash.SetActive(true);
        AudioManager.score += 100;
        Destroy(transform.gameObject);
    }
    IEnumerator GetDataInAndroid(string j)
    {
        WWW www = new WWW(Application.streamingAssetsPath + "/" + j);
        yield return www;
        //string json = File.ReadAllText(Application.streamingAssetsPath + "/" + jsonfile);
        string json = www.text;
        questions = JsonUtility.FromJson<IntrebariArr>(json);
        int i = Random.Range(0, questions.Intrebari.Length);
        numarIntrebare = i;
        QuestionText.text = questions.Intrebari[i].intrebare;
        A1Text.text = questions.Intrebari[i].raspuns1;
        A2Text.text = questions.Intrebari[i].raspuns2;
        A3Text.text = questions.Intrebari[i].raspuns3;
        ACText.text = questions.Intrebari[i].raspunsC;
    }
    IEnumerator closeQuestion()
    {
        yield return new WaitForSeconds(3);
        joy.SetActive(true);
        jump.SetActive(true);
        dash.SetActive(true);
        Destroy(transform.gameObject);
    }
}
