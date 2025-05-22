package lab4.java.solution2;

import java.util.concurrent.Semaphore;

public class Waiter {
  private final Semaphore tokens = new Semaphore(2);

  public void requestPermission() {
    try {
      tokens.acquire();
    } catch (InterruptedException e) {
      e.printStackTrace();
    }
  }

  public void releasePermission() {
    tokens.release();
  }
}