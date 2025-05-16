package lab3.java;

public class Item {
  private final String id;
  private final boolean isPoisonPill;

  public Item(String id, boolean isPoisonPill) {
    this.id = id;
    this.isPoisonPill = isPoisonPill;
  }

  public boolean isPoisonPill() {
    return isPoisonPill;
  }

  @Override
  public String toString() {
    return id;
  }
}
