package lab1.java;

public class MainThread extends Thread {
  private final int id;
  private final int step;
  private final BreakFlag breakFlag;

  public MainThread(int id, int step, BreakFlag breakFlag) {
    this.id = id;
    this.step = step;
    this.breakFlag = breakFlag;
  }

  @Override
  public void run() {
    long sum = 0;
    long count = 0;
    while (!breakFlag.shouldStop()) {
      sum += step;
      count++;
    }
    System.out.println("Thread" + id + " Sum: " + sum + " Count: " + count);
  }
}