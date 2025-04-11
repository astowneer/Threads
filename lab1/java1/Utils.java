package lab1.java1;

import java.util.Arrays;

public final class Utils {
  private Utils() {}

  public static Integer[] generateArraySequence(int amount) {
    return generateArraySequence(amount, 1);
  }

  public static Integer[] generateArraySequence(int amount, int step) {
    Integer[] indices = new Integer[amount];
    for (int i = 0; i < amount; i++) {
      indices[i] = i * step;
    }
    return indices;
  }

  public static Integer[] sortOrderIndices(int[] array) {
    Integer[] indices = generateArraySequence(array.length);
    Arrays.sort(indices, (a, b) -> Integer.compare(array[a], array[b]));
    return indices;
  }
}
