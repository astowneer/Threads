package lab1.java1;

public class Main {
  public static void main(String[] args) {
    int numberOfThreads = 4;
    int[] delayPerThread = new int[] { 2000, 4000, 1000, 8000 };

    BreakFlag[] flags = new BreakFlag[numberOfThreads];
    for (int i = 0; i < numberOfThreads; i++) { 
      flags[i] = new BreakFlag();
    }

    BreakControllerThread controllerThread = new BreakControllerThread(flags, delayPerThread);

    int[] stepPerThread = new int[] { 1, 2, 3, 4 };
    for (int i = 0; i < numberOfThreads; i++) {
      WorkerThread workerThread = new WorkerThread(i, stepPerThread[i], flags[i]);
      workerThread.start();
    }

    controllerThread.start();
  }
}