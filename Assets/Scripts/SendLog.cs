using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendLog : MonoBehaviour
{
    void Start() {
        for (int i = 0; i < 5; i++) {
            StartCoroutine(Upload(i));
        }
    }

    IEnumerator Upload(int k) {
        WWWForm form = new WWWForm();

        for (int i=0; i < 15; i ++) {
            if (Random.value > 0.5f)
                form.AddField("Number "+i, ""+Random.Range(1, 10));
        }

        using (UnityWebRequest www = UnityWebRequest.Post("http://oscarglo.me:1234", form)) {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                Debug.Log("Form upload complete " + k);
            }
        }
    }
}
