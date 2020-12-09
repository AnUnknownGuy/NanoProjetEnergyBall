using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour

    
{
    public static bool exist = false;
    public static AudioManager instance;

    [Space(10)]
    [Header("Events")]
    
    //Wwise Events
    public AK.Wwise.Event dash;
    public AK.Wwise.Event jump;
    public AK.Wwise.Event battle_Scene;
    public AK.Wwise.Event ball_get;
    public AK.Wwise.Event throw_ball;
    public AK.Wwise.Event dash_hit;
    public AK.Wwise.Event ball_hit;
    public AK.Wwise.Event ball_bounce;
    public AK.Wwise.Event ball_air;

    [Space(10)]
    [Header("Game Parameters")]
    
    //Wwise Game Parameters
    public AK.Wwise.RTPC ball_velocity;
    public AK.Wwise.State ball_idle;
    public AK.Wwise.State ball_charged;


    // Start is called before the first frame update
    void Start()
    {
        exist = true;
        instance = this;

        Battle_Scene(gameObject);
    }

    public static void Dash(GameObject gameObject) {
        if (exist) instance.dash.Post(gameObject);
    }
    public static void Jump(GameObject gameObject) {
        if (exist) instance.jump.Post(gameObject);
    }
    public static void Battle_Scene(GameObject gameObject) {
        if (exist) instance.battle_Scene.Post(gameObject);
    }
    public static void Ball_Get(GameObject gameObject) {
        if (exist) instance.ball_get.Post(gameObject);
    }
    public static void Throw_Ball(GameObject gameObject) {
        if (exist) instance.throw_ball.Post(gameObject);
    }
    public static void Dash_Hit(GameObject gameObject) {
        if (exist) instance.dash_hit.Post(gameObject);
    }
    public static void Ball_Hit(GameObject gameObject) {
        if (exist) instance.ball_hit.Post(gameObject);
    }
    public static void Ball_Bounce(GameObject gameObject) {
        if (exist) instance.ball_bounce.Post(gameObject);
    }
    public static void Ball_Air(GameObject gameObject) {
        if (exist) instance.ball_air.Post(gameObject);
    }



    void update()
    {
        ball_velocity.SetGlobalValue(0);
    }
}

