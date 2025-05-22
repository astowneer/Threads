package lab4.java.solution3;

import java.util.concurrent.Semaphore;

public class Table {
  private final Semaphore[] forks = new Semaphore[5];

  public Table() {
    for (int i = 0; i < forks.length; i++) {
      forks[i] = new Semaphore(1);
    }
  }

  public void getFork(int id) {
    try {
      forks[id].acquire();
    } catch (InterruptedException e) {
      e.printStackTrace();
    }
  }

  public boolean tryGetFork(int fork) {
    return forks[fork].tryAcquire();
  }

  public void putFork(int id) {
    forks[id].release();
  }
}
