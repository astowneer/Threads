package lab2.java;

public class ThreadMin extends Thread {
  private final int startIndex;
  private final int endIndex;
  private final ArrClass arrClass;

  public ThreadMin(int startIndex, int endIndex, ArrClass arrClass) {
    this.startIndex = startIndex;
    this.endIndex = endIndex;
    this.arrClass = arrClass;
  }

  @Override
  public void run() {
    int[] minData = arrClass.partMin(startIndex, endIndex);
    arrClass.putMin(minData);
    arrClass.incThreadCount();
  }
}