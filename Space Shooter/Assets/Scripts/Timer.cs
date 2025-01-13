public class Timer 
{
    private float m_CurrentTime;

    public bool IsFinished => m_CurrentTime <= 0;

    public Timer(float startTime)
    {
        Start(startTime);
    }

    public void Start(float startTime)
    {
        m_CurrentTime = startTime;
    }

    public void RemoveTime(float deltaTime)
    {
        if (m_CurrentTime <= 0) return;

        m_CurrentTime -= deltaTime;
    }

}
