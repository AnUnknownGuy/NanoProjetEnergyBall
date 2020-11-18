interface StateInterface {
    void WalkSignal(float x);
    void JumpSignal();
    void FastFallSignal();
    void HitSignal();
    void Stop();
    void Start();
    void Update();
    void BallEntered(Ball ball);
    string GetName();
}
