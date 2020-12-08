using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SendLog : MonoBehaviour
{

    public GameManager gameManager;

    private int playerID = 0;

    public bool activated = false;
    WWWForm form;

    private void Start() {
        form = new WWWForm();
    }

    public void Restart() {
        playerID = 0;
        form = new WWWForm();
    }

    public void Send(string winner) {
        StartCoroutine(Upload(winner));
    }

    IEnumerator Upload(string winner) {

        if (activated) {
            form.AddField("winner", winner);
            AddPlayer(gameManager.level.player1);
            AddPlayer(gameManager.level.player2);

            using (UnityWebRequest www = UnityWebRequest.Post("http://oscarglo.me:1234", form)) {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError) {
                    Debug.Log(www.error);
                } else {
                    Debug.Log("Form upload complete ");
                }
            }
        }
    }

    private void AddPlayer(Player player) {
        playerID++;
        AddPlayerValue("remainingLife", player.health);
        AddPlayerValue("timeOnGround", player.timeOnGround);
        AddPlayerValue("timeInAir", player.timeInAir);
        AddPlayerValue("numberOfJump", player.stateManager.numberOfJumps);
        AddPlayerValue("numberOfFastFall", player.stateManager.numberOfFastFall);
        AddPlayerValue("numberOfDash", player.stateManager.numberOfDash);
        AddPlayerValue("timeInDash", player.stateManager.timeInDash);
        AddPlayerValue("numberOfCatch", player.stateManager.numberOfBallCatched);
        AddPlayerValue("timeInHold", player.stateManager.timeInHold);
        AddPlayerValue("numberHittedByDash", player.stateManager.numberHittedByDash);
        AddPlayerValue("numberHittedByBall", player.stateManager.numberHittedByBall);
    }
    
    private void AddPlayerValue(string valueName, Object value) {
        form.AddField("P" + playerID + "_" + valueName, "" + value);
    }
    private void AddPlayerValue(string valueName, string value) {
        form.AddField("P" + playerID + "_" + valueName, "" + value);
    }
    private void AddPlayerValue(string valueName, float value) {
        form.AddField("P" + playerID + "_" + valueName, "" + value);
    }
    private void AddPlayerValue(string valueName, int value) {
        form.AddField("P" + playerID + "_" + valueName, "" + value);
    }
}
