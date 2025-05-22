package lab4.java.solution4;

import java.util.concurrent.Semaphore;

public class Table {
  private final Semaphore[] forks = new Semaphore[5];

  public Table() {
    for (int i = 0; i < forks.length; i++) {
      forks[i] = new Semaphore(1);
    }
  }

  public synchronized boolean pickBothForks(int left, int right) {
    if (forks[left].availablePermits() > 0 && forks[right].availablePermits() > 0) {
      try {
        forks[left].acquire();
        forks[right].acquire();
        return true;
      } catch (InterruptedException e) {
        e.printStackTrace();
        return false;
      }
    }
    return false;
  }

  public void putDownForks(int left, int right) {
    forks[left].release();
    forks[right].release();
  }
}
