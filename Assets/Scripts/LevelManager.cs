using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public Player player1;
    public Player player2;
    public InputManager input1;
    public InputManager input2;
    public Ball ball;

    public GameManager gameManager;

    private bool restarting = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update() {

        if (!restarting) {

            if (!player1.alive || !player2.alive) {
                player1.ForceLogUpdate();
                player2.ForceLogUpdate();
            }

            if (!player1.alive && !player2.alive) {
                //Debug.Log("DRAW !");
                gameManager.Win("DRAW");
            } else if (!player1.alive) {
                //Debug.Log("PLAYER 2 WINS !");
                AudioManager.Death_Green(gameObject);
                gameManager.Win("P2");
                
            } else if (!player2.alive) {
                //Debug.Log("PLAYER 1 WINS !");
                AudioManager.Death_Blue(gameObject);
                gameManager.Win("P1");
                
            }

            if (!player1.alive || !player2.alive) {
                restarting = true;

                player1.ToStunState(5);
                player2.ToStunState(5);

            }
        }
        
    }
    public void Stop() {
        //input1.Stop();
        //input2.Stop();
        ball.StopSound();
    }
}
