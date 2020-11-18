interface StateInterface {
    void WalkSignal();
    void JumpSignal();
    void HitSignal();
    void Stop();
    void Start();
    void Update();
    void BallEntered(Ball ball);
    string GetName();
}
