package lab4.java.solution2;

public class Philosopher extends Thread {
  private final int id;
  private final int left, right;
  private final Table table;
  private final Waiter waiter;

  public Philosopher(int id, Table table, Waiter waiter) {
    this.id = id;
    this.table = table;
    this.waiter = waiter;
    this.left = id;
    this.right = (id + 1) % 5;
    start();
  }

  @Override
  public void run() {
    for (int i = 0; i < 10; i++) {
      System.out.println("Philosopher " + id + " is thinking");
      waiter.requestPermission();
      table.getFork(left);
      table.getFork(right);
      System.out.println("Philosopher " + id + " is eating " + (i + 1));
      table.putFork(right);
      table.putFork(left);
      waiter.releasePermission();
    }
  }
}