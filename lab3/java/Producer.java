package lab3.java;

public class Producer extends Thread {
  private final int id;
  private final Storage manager;
  private final int itemNumbers;

  public Producer(int id, Storage manager, int itemNumbers) {
    this.id = id;
    this.manager = manager;
    this.itemNumbers = itemNumbers;

    start();
  }

  @Override
  public void run() {
    for (int i = 0; i < itemNumbers; i++) {
      try {
        System.out.println("Producer" + id + " near the storage");
        manager.full.acquire();
        System.out.println("Producer" + id + " in order");
        manager.access.acquire();

        Item item = new Item("item-" + id, false);
        manager.addItem(item);
        System.out.println("Producer" + id + " put item " + item + " in the storage");

        manager.access.release();
        manager.empty.release();
        System.out.println("Producer" + id + " left the storage");
      } catch (InterruptedException e) {
        e.printStackTrace();
      }
    }
  }
}