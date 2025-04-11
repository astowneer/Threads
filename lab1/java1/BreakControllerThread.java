package lab1.java1;

public class BreakControllerThread extends Thread {
  private BreakFlag[] flags;
  private int[] delays;

  public BreakControllerThread(BreakFlag[] flags, int[] delays) {
    this.flags = flags;
    this.delays = delays;
  }

  @Override
  public void run() {
    Integer[] sortedIndices = getSortedDelayIndices();
    reorderDelaysAndFlags(sortedIndices);
    normalizeDelaysToDifferences();

    for (int i = 0; i < delays.length; i++) {
      try {
        Thread.sleep(delays[i]);
      } catch (InterruptedException e) {
        e.printStackTrace();
      }

      flags[i].stop();
    }
  }

  private void normalizeDelaysToDifferences() {
    for (int i = 1; i < delays.length; i++) {
      delays[i] -= delays[i - 1];
    }
  }

  private Integer[] getSortedDelayIndices() {
    return Utils.sortOrderIndices(delays);
  }

  private void reorderDelaysAndFlags(Integer[] sortedIndices) {
    int[] sortedDelays = delays.clone();
    BreakFlag[] sortedFlags = flags.clone();

    for (int i = 0; i < sortedIndices.length; i++) {
      delays[i] = sortedDelays[sortedIndices[i]];
      flags[i] = sortedFlags[sortedIndices[i]];
    }
  }
}
