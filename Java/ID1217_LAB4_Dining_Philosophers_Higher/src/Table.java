
import java.util.LinkedList;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author Kim Persson
 */
public class Table {

    private final Lock lock = new ReentrantLock();
    private final State[] states = new State[5];
    private final Condition philos[] = new Condition[5];
    LinkedList<Condition> ticketCondList = new LinkedList<>();
    private static final int LEFT = 0, RIGHT = 1;


    private enum State {

        THINKING, EATING, HUNGRY;
    }

    public Table() {
        for (int i = 0; i < states.length; i++) {
            states[i] = State.THINKING;
            philos[i] = lock.newCondition();
        }
    }

    public void getForks(int id) {
        System.out.println(System.currentTimeMillis() + " : Philosopoher " + id + " called get forks");
        lock.lock();
        try {
            
            waitForMyTicket(id);
            // It is our turn in line now proceed to try to get forks
            states[id] = State.HUNGRY;

            // Wait until both our neighbours aren't eating 
            while (states[id] != State.EATING) {
                testForks(id);
                if (states[id] != State.EATING) {
                    philos[id].await();
                }
            }
            // We have status EATING now, let next in line try to eat
            if(ticketCondList.size() > 0)
                ticketCondList.pollFirst().signal();

        } catch (InterruptedException ex) {
            Logger.getLogger(Table.class.getName()).log(Level.SEVERE, null, ex);
        } finally {

            lock.unlock();
        }
    }

    public void relForks(int id) {
        long time;
        lock.lock();
        try {
            states[id] = State.THINKING;
            time = System.currentTimeMillis();
            testForks((id + 1) % 5);
            testForks((id + 5 - 1) % 5);
        } finally {
            lock.unlock();
        }
    }

    private void testForks(int id) {

        int[] forks = getForkIds(id);
        if (states[forks[RIGHT]] != State.EATING && states[forks[LEFT]] != State.EATING && states[id] == State.HUNGRY) {

            // We can eat
            states[id] = State.EATING;
            // Signal ourselves incase this is called from relforks to 
            // notify neighbours that one philosopher has stopped eating
            philos[id].signal();
        }
    }

    private void waitForMyTicket(int myId) throws InterruptedException {

        if (ticketCondList.size() > 0) {
            ticketCondList.add(philos[myId]);
            philos[myId].await();

        }
    }

    private void printStates(int id) {
        StringBuilder sb = new StringBuilder();
        sb.append(System.currentTimeMillis()).append(" : ");
        for (int i = 0; i < states.length; i++) {
            if (i == id) {
                sb.append("_ME_ ");
            } else {
                sb.append(states[i].toString()).append(" ");
            }
        }
        System.out.println(sb.toString());
    }

    private int[] getForkIds(int myId) {
        int left = (5 + myId - 1) % 5;
        int right = (myId + 1) % 5;
        return new int[]{left, right};
    }

 
}
