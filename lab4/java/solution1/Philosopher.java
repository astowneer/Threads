package lab4.java.solution1;

public class Philosopher extends Thread {
  private final Table table;
  private final int leftFork, rightFork;
  private final int id;

  public Philosopher(int id, Table table) {
    this.id = id;
    this.table = table;

    if (id < 4) {
      rightFork = id;
      leftFork = (id + 1) % 5;
    } else {
      leftFork = id;
      rightFork = (id + 1) % 5;
    }

    start();
  }

  @Override
  public void run() {
    for (int i = 0; i < 10; i++) {
      System.out.println("Philosopher" + id + " is thinking " + (i + 1) + " times");
      table.getFork(rightFork);
      table.getFork(leftFork);
      System.out.println("Philosopher" + id + " is eating" + (i + 1) + " times");
      table.putFork(leftFork);
      table.putFork(rightFork);
    }
  }
}