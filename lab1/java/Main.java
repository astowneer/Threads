package lab1.java;

public class Main {
  public static void main(String[] args) {
    int numberOfThreads = 5;
    int step = 1;

    for (int i = 0; i < numberOfThreads; i++) {
      BreakFlag flag = new BreakFlag();
      MainThread workerThread = new MainThread(i, step, flag);
      BreakControllerThread controllerThread = new BreakControllerThread(flag, i * 5000);

      workerThread.start();
      controllerThread.start();
    }
  }
}
