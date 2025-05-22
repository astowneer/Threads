package lab4.java.solution2;

public class Main {
    public static void main(String[] args) {
        Table table = new Table();
        Waiter waiter = new Waiter();
        for (int i = 0; i < 5; i++) {
            new Philosopher(i, table, waiter);
        }
    }
}