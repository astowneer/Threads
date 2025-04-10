public class BreakFlag
{
  private volatile bool stop = false;

  public bool ShouldStop()
  {
    return stop;
  }

  public void Stop()
  {
    this.stop = true;
  }
}