using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;

public class Ball : MonoBehaviour
{
    
    private Rigidbody2D rb;

    public PhysicsMaterial2D material;

    public bool sleeping;

    public Vector2 speedWhenFreeFromDash = new Vector2(0, 10);

    public float gravityWhenThorwn = 0.2f;
    public float gravity = 2f;

    private float maxSpeedSound = 15;
    private float speedSound = 0;

    [HideInInspector] public Player player;
    [HideInInspector] public Player previousPlayer;
    [HideInInspector] public Player previousPlayertouched;
    [HideInInspector] public float previousPlayertouchedTimeStamp;
    [HideInInspector] public float timeBeforeballCanBeCatchBySamePlayer = 0.20f;
    [HideInInspector] public bool charged = false;
    
    public float collisionRadius = 0.25f;
    public VisualEffect ballEffect;
    public VisualEffect trailEffect;
    public Transform trailAnchor;
    private Color ballColor;
    private Gradient trailGradient;
    private GradientColorKey[] trailColorKeys;
    public float trailYScaleDiviser = 20f;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Ball_Idle();
        AudioManager.Ball_Air(gameObject);
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravity;
        sleeping = false;
        
        ballColor = ballEffect.GetVector4("Color");
        trailGradient = trailEffect.GetGradient("ColorOverLife");
        trailColorKeys = trailGradient.colorKeys;
        
        SetBallColor(ballColor);

        BallSpawnAnimation();
    }

    private void BallSpawnAnimation()
    {
        transform.localScale = Vector3.zero;
        rb.simulated = false;
        StartCoroutine(GrowBall());
    }

    private IEnumerator GrowBall()
    {
        yield return new WaitForSeconds(1.5f);
        transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack).OnComplete(
            () => rb.simulated = true);
    }

    // Update is called once per frame
    void Update()
    {
        GetSpeedSound();
        AudioManager.Ball_Velocity(speedSound);
        if (player != null) {
            transform.position = player.BallTransform.position;
        }

        AdjustTrails();
    }

    private void SetBallColor(Color color)
    {
        ballEffect.SetVector4("Color", color * 3);
        
        for (int key = 0; key < trailColorKeys.Length; key++)
            trailColorKeys[key].color = color;
        
        trailGradient.colorKeys = trailColorKeys;
        trailEffect.SetGradient("ColorOverLife", trailGradient);
    }

    private void AdjustTrails()
    {
        trailAnchor.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.down, rb.velocity));
        trailEffect.transform.localScale = new Vector3(1, 
            (player == null) ? rb.velocity.magnitude / trailYScaleDiviser : 0, 1);
    }

    public bool Catch(Player player) {
        if (!sleeping) {
            SetBallColor(player.color);
            this.player = player;
            sleeping = true;
            rb.Sleep();
            return true;
        }else {
            return false;
        }
    }

    public void Throw(Vector2 dir, float force) {

            previousPlayertouchedTimeStamp = Time.time;
            Charge();
            Free();
            rb.velocity = Vector2.zero;
            rb.velocity += dir.normalized * force;
    }

    public void Free() {
        previousPlayer = player;
        previousPlayer.StartDecayTimer();
        player = null;
        sleeping = false;
        rb.WakeUp();
    }

    [SerializeField] private GameObject tornade;
    [SerializeField] private Material tornade_mat;

    public void Charge() {
        AudioManager.Ball_Charged();
        previousPlayer = player;
        rb.gravityScale = 0;
        charged = true;
        tornade.SetActive(true);
        tornade_mat.SetColor("MainColor", player.GetComponent<Player>().color * 1);
    }

    public void Uncharge()
    {
        SetBallColor(ballColor);
        tornade.SetActive(false);
        AudioManager.Ball_Idle();
        charged = false;
        rb.gravityScale = gravity;
    }

    public void MoveToPreviousPlayer() {

    }

    public void Hit(bool isHittingPlayer) {
        if (charged) ShowImpact(isHittingPlayer);
        Uncharge();
        MoveToPreviousPlayer();
    }

    public void ShowImpact(bool isHittingPlayer)
    {
        VFXManager.Spawn(VFXManager.Instance.BallImpact, transform.position, previousPlayer.color, isHittingPlayer);
    }

    public void SetSpeed(Vector2 speed) {
        rb.velocity = speed;
    }

    public void FakeCollision() {
        rb.velocity = -rb.velocity * material.bounciness;
    }

    public Vector2 GetSpeed() {
        return rb.velocity;
    }

    public void GetSpeedSound() {
        if (!sleeping) {
            float speed = rb.velocity.magnitude;
            if (speed < 0)
                speed = 0;
            if (speed > 15)
                speed = 15;

            speedSound =((speed / maxSpeedSound) * 100);
        } else {
            speedSound = 0;
        }
    }

    public void StopSound() {
        AudioManager.Ball_Air_Stop(gameObject);
    }

    private void OnDrawGizmos() {
        //Gizmos.color = Color.green;


        //Gizmos.DrawWireSphere((Vector2)transform.position, collisionRadius);
    }

    public void SetSpeedWhenFreeFromDash() {
        SetSpeed(speedWhenFreeFromDash);

        SetBallColor(ballColor);
    }

    void OnCollisionEnter2D(Collision2D collision) {


        if (!sleeping) {

            Vector3 normal = collision.contacts[0].normal;

            if ((collision.gameObject.tag == "plateform") && collision.GetContact(0).normalImpulse != 0) {
                AudioManager.Ball_Bounce(gameObject);
                Hit(false);
            }
            if (collision.gameObject.tag == "wall") {
                if (charged) {
                    Hit(false);
                }
                AudioManager.Ball_Bounce(gameObject);
            }
            
        }
        
    }

}
