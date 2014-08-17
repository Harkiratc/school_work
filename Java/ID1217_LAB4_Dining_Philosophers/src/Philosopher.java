
import java.util.Random;


/**
 *
 * @author Kim Persson
 */
public class Philosopher extends Thread {

    private int id, rounds, maxEatingTime, maxThinkingTime;
    private Table table;
    private Random r;
    

    public Philosopher(int id, int rounds, Table table, int maxEatingTime, int maxThinkingTime,long randomSeed) {
        this.id = id;
        this.rounds = rounds;
        this.table = table;
        this.r = new Random(randomSeed);
        this.maxEatingTime = maxEatingTime;
        this.maxThinkingTime = maxThinkingTime;
       
    }

    @Override
    public void run() {
        for (int i = 0; i < rounds; i++) {
            try {
                // Think
                System.out.println(System.currentTimeMillis() + " : Philosopher " + id + " is has started thinking");
                Thread.sleep(r.nextInt(maxThinkingTime));
                System.out.println(System.currentTimeMillis() + " : Philosopher " + id + " is has finished thinking");
                // Eat
                table.getForks(id);
                System.out.println(System.currentTimeMillis() + " : Philosopher " + id + " has started eating");
                Thread.sleep(r.nextInt(maxEatingTime));
                table.relForks(id);
                System.out.println(System.currentTimeMillis() + " : Philosopher " + id + " is has finished eating");
                // Done eating

            } catch (InterruptedException ex) {
                System.err.println("Sleep failed");
            }
        }
        System.out.println(System.currentTimeMillis() + " : Philosopher " + id + " done");
    }

}
