interface StateInterface {
    void WalkSignal(float x);
    bool JumpSignal();
    bool ActionSignal();
    bool FastFallSignal();
    void HitSignal();
    void Stop();
    void Start();
    void Update();
    void BallEntered(Ball ball);
    string GetName();
}
