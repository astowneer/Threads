package lab1.java;

public class BreakControllerThread extends Thread {
  private final BreakFlag flag;
  private final int delayMs;

  public BreakControllerThread(BreakFlag flag, int delayMs) {
    this.flag = flag;
    this.delayMs = delayMs;
  }

  @Override
  public void run() {
    try {
      Thread.sleep(delayMs);
    } catch (InterruptedException e) {
      e.printStackTrace();
    }
    flag.stop();
  }
}