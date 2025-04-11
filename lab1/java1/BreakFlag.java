package lab1.java1;

public class BreakFlag {
  private volatile boolean stop = false;

  public boolean shouldStop() {
    return stop;
  }

  public void stop() {
    this.stop = true;
  }
}