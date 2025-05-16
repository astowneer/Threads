package lab3.java;

public class Main {
    public static void main(String[] args) {
        int size = 3;
        Storage storage = new Storage(size);

        int numProducers = 2;
        int producerItems = 12;
        Producer[] producers = new Producer[numProducers];
        for (int i = 0; i < numProducers; i++) {
            producers[i] = new Producer(i + 1, storage, producerItems / numProducers);
        }

        int numConsumers = 3;
        int consumerItems = 15;
        Consumer[] consumers = new Consumer[numConsumers];
        for (int i = 0; i < numConsumers; i++) {
            consumers[i] = new Consumer(i + 1, storage, consumerItems / numConsumers);
        }

        for (Producer producer : producers) {
            try {
                producer.join();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }

        for (int i = 0; i < numConsumers; i++) {
            storage.addItem(new Item(null, true));
            storage.empty.release();
        }
    }
}
