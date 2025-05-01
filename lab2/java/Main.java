package lab2.java;

public class Main {
  public static void main(String[] args) {
    int dim = 10000000;
    int threadNum = 3;
    ArrClass arrClass = new ArrClass(dim, threadNum);
    // arrClass.changeElementByIndex(9999998, -39);
    // arrClass.changeElementByIndex(6666667, -59);
    int[] minData = arrClass.partMin(0, dim);
    System.out.println("Min value " + minData[0] + " index " + minData[1]);

    int[] res = arrClass.threadMin();
    int min = res[0];
    int indexMin = res[1];
    System.out.println("Min value " + min + " index " + indexMin);
  }
}