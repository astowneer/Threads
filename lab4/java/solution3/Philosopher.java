package lab4.java.solution3;

public class Philosopher extends Thread {
  private final int id;
  private final int left, right;
  private final Table table;

  public Philosopher(int id, Table table) {
    this.id = id;
    this.table = table;
    this.left = id;
    this.right = (id + 1) % 5;
    start();
  }

  @Override
  public void run() {
    for (int i = 0; i < 10; i++) {
      System.out.println("Philosopher " + id + " is thinking");
      table.getFork(left);
      if (!table.tryGetFork(right)) {
        table.putFork(left);
        continue;
      }
      System.out.println("Philosopher " + id + " is eating " + (i + 1));
      table.putFork(right);
      table.putFork(left);
    }
  }
}
