package lab2.java;

import java.util.Random;

public class ArrClass {
  private final int dim;
  private final int threadNum;
  private final int[] arr;

  private int MIN_ARRAY_VALUE = 0;
  private int MAX_ARRAY_VALUE = 10000000;

  public ArrClass(int dim, int threadNum) {
    this.dim = dim;
    this.threadNum = threadNum;
    this.arr = new int[dim];

    generateByArrayLength();
    // generateRandomArray(MIN_ARRAY_VALUE, MAX_ARRAY_VALUE);
  }

  private void generateRandomArray(int min, int max) {
    Random random = new Random();

    for (int i = 0; i < arr.length; i++) {
      arr[i] = random.nextInt(max - min + 1) + min;
    }
  }

  private void generateByArrayLength() {
    for (int i = 0; i < arr.length; i++) {
      arr[i] = i;
    }
  }

  public void changeElementByIndex(int index, int value) {
    if (index < 0 || index >= this.arr.length) {
      throw new IndexOutOfBoundsException(
          "Index " + index + " is out of bounds for array of length " + this.arr.length);
    }
    this.arr[index] = value;
  }

  public int[] partMin(int startIndex, int endIndex) {
    int min = arr[startIndex];
    int indexMin = startIndex;
    for (int i = startIndex + 1; i < endIndex; i++) {
      if (min > arr[i]) {
        min = arr[i];
        indexMin = i;
      }
    }
    return new int[] { min, indexMin };
  }

  private synchronized int[] getMin() {
    while (getThreadCount() < threadNum) {
      try {
        wait();
      } catch (InterruptedException e) {
        e.printStackTrace();
      }
    }
    return new int[] { min, indexMin };
  }

  private int min = Integer.MAX_VALUE;
  private int indexMin = 0;

  public synchronized void putMin(int[] minData) {
    int min = minData[0];
    int indexMin = minData[1];

    if (this.min > min) {
      this.min = min;
      this.indexMin = indexMin;
    }
  }

  private int threadCount = 0;

  public synchronized void incThreadCount() {
    threadCount += 1;
    notify();
  }

  public int getThreadCount() {
    return threadCount;
  }

  public int[][] getSplitIndexes() {
    int[][] result = new int[threadNum][2];
    int baseSize = dim / threadNum;
    int remainder = dim % threadNum;

    int startIndex = 0;

    for (int i = 0; i < threadNum; i++) {
      int partSize = baseSize + (i < remainder ? 1 : 0);
      int endIndex = startIndex + partSize - 1;
      result[i][0] = startIndex;
      result[i][1] = endIndex;
      startIndex = endIndex + 1;
    }

    return result;
  }

  public int[] threadMin() {
    int[][] indexes = getSplitIndexes();
    // for (int i = 0; i < indexes.length; i++) {
    //   System.out.println("Part " + (i + 1) + ": start = " + indexes[i][0] + ", end = " + indexes[i][1]);
    // }

    changeElementByIndex(9999999, -52);
    changeElementByIndex(3, -51);
    changeElementByIndex(9, -50);
 
    Thread[] threads = new Thread[threadNum];

    for (int i = 0; i < threadNum; i++) {
      threads[i] = new ThreadMin(indexes[i][0], indexes[i][1] + 1, this);
      threads[i].start();
    }

    for (int i = 0; i < threadNum; i++) {
      try {
        threads[i].join();
      } catch (InterruptedException e) {
        e.printStackTrace();
      }
    }
    return getMin();
  }
}

// public int partMin(int startIndex, int endIndex) {
// int min = arr[startIndex];
// for (int i = startIndex + 1; i < endIndex; i++) {
// if (min > arr[i]) {
// min = arr[i];
// }
// }
// return min;
// }

// private synchronized int getMin() {
// while (getThreadCount() < threadNum) {
// try {
// wait();
// } catch (InterruptedException e) {
// e.printStackTrace();
// }
// }
// return min;
// }

// public synchronized void putMin(int min) {
// if (this.min > min) {
// this.min = min;
// }
// }

// public int threadMin() {
// int[][] indexes = getSplitIndexes();
// // for (int i = 0; i < indexes.length; i++) {
// // System.out.println("Part " + (i + 1) + ": start = " + indexes[i][0] + ",
// end
// // = " + indexes[i][1]);
// // }
// System.out.println(34);
// // changeElementByIndex(3, -50);
// // changeElementByIndex(9, -51);
// Thread[] threads = new Thread[threadNum];

// for (int i = 0; i < threadNum; i++) {
// threads[i] = new ThreadMin(indexes[i][0], indexes[i][1] + 1, this);
// threads[i].start();
// }

// for (int i = 0; i < threadNum; i++) {
// try {
// threads[i].join();
// } catch (InterruptedException e) {
// e.printStackTrace();
// }
// }
// return getMin();
// }