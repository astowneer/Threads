package lab3.java;

public class Consumer extends Thread {
  private final int id;
  private final Storage manager;
  private final int itemNumbers;

  public Consumer(int id, Storage manager, int itemNumbers) {
    this.id = id;
    this.manager = manager;
    this.itemNumbers = itemNumbers;

    start();
  }

  @Override
  public void run() {
    for (int i = 0; i < itemNumbers; i++) {
      try {
        System.out.println("Consumer" + id + " near the storage");
        manager.empty.acquire();
        manager.access.acquire();

        Item item = manager.getItem();
        System.out.println("Consumer" + id + " took " + item + " from the storage");

        manager.access.release();
        manager.full.release();
        System.out.println("Consumer" + id + " left the storage");

        if (item.isPoisonPill()) {
          System.out.println("Consumer" + id + " received a poison pill and is terminating.");
          break;
        }
      } catch (InterruptedException e) {
        e.printStackTrace();
      }
    }
  }
}
