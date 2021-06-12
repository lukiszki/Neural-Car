using UnityEngine;
using UnityEngine.UI;

public class SaveLastGeneration : MonoBehaviour
{
    [SerializeField]
    Text text;



    [SerializeField]
    Transform startPos;

    int GenNumber = 0;

    Values last;
    [SerializeField]
    private bool saveLastGeneration = true;

    public GameObject set;
    private void Start()
    {
        Application.targetFrameRate = 40;
        GenerateSet();
        text.text = "0";
    }

    void Update()
    {
        RecolorBest();
        if (saveLastGeneration)
        {
            if (transform.GetChildCount() != 0)
            {
                float najF = 0;
                Values s;
                foreach (CarController x in GetComponentsInChildren<CarController>())
                {
                    if (najF < x.fitness)
                    {
                        last = x.GetValues();
                        najF = x.fitness;
                    }
                    //last = x.GetValues();
                }
            }
            else
            {

                NextGen();
            }
        }
    }

    public void SaveBest()
    {
        SaveSystem.SaveValues(last);
    }
    void NextGen()
    {
        SaveBest();
        //AppHelper.Quit();
        GenerateSet();
        GenNumber++;
        text.text = GenNumber.ToString();
    }

    private void GenerateSet()
    {
        for (int x = 0; x < 32; x++)
        {
               GameObject @object = Instantiate(set, startPos.position, Quaternion.identity);
               @object.transform.parent = this.transform;
        }
    }
    void RecolorBest()
    {
        if (transform.childCount == 0) return;
        GameObject best = transform.GetChild(0).gameObject;
        float old = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1,1,1,0f));
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).GetComponent<CarController>().fitness > old)
            {
                old = transform.GetChild(i).GetComponent<CarController>().fitness;
                best = transform.GetChild(i).gameObject;
            }
            
        }
        best.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 0, 0, 1f));
    }
}

public static class AppHelper
{
#if UNITY_WEBPLAYER
     public static string webplayerQuitURL = "http://google.com";
#endif
    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }
}
