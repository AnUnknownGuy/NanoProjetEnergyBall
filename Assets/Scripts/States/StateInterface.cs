using UnityEngine;

interface StateInterface {
    void WalkSignal(float x);
    bool JumpSignal();
    bool ActionSignal();
    bool FastFallSignal();
    void HitSignal();
    void Stop();
    void Start();
    void Start(float param);
    void Update();
    void BallEntered(Ball ball);
    void DashEntered(Player player);
    void WallCollided(Vector2 collisionDirection);
    string GetName();
}
