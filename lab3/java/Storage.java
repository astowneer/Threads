package lab3.java;

import java.util.ArrayList;
import java.util.concurrent.Semaphore;

public class Storage {
  private final ArrayList<Item> items;
  public Semaphore access;
  public Semaphore full;
  public Semaphore empty;

  public Storage(int size) {
    access = new Semaphore(1);
    full = new Semaphore(size);
    empty = new Semaphore(0);
    items = new ArrayList<>();
  }

  public void addItem(Item item) {
    items.add(item);
  }

  public Item getItem() {
    return items.remove(0);
  }

  public int getItemCount() {
    return items.size();
  }
}