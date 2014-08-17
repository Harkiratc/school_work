
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;
import java.util.logging.Level;
import java.util.logging.Logger;


/**
 * Basic version of Dining Philosophers
 * @author Kim Persson
 */
public class Table {

    private Lock lock = new ReentrantLock();
    private Condition philos[] = new Condition[5];
    private State[] states = new State[5];
    private static final int LEFT = 0, RIGHT = 1;

    private enum State {

        THINKING, EATING, HUNGRY;
    }

    public Table() {
        for (int i = 0; i < philos.length; i++) {
            philos[i] = lock.newCondition();
            states[i] = State.THINKING;
        }
    }

    public void getForks(int id) {
        System.out.println(System.currentTimeMillis() + " : Philosopoher " + id + " called get forks");
        lock.lock();
        try {
            states[id] = State.HUNGRY;

            while (states[id] != State.EATING) {
                testForks(id);
                if (states[id] != State.EATING) {
                    philos[id].await();
                }
            }

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
            states[id] = State.EATING;
            philos[id].signal();
        }
    }
    private void printStates(int id){
        StringBuilder sb = new StringBuilder();
        sb.append(System.currentTimeMillis()).append(" : ");
        for(int i = 0; i < states.length; i++) {
            if(i == id)
                sb.append("_ME_ ");
            else
                sb.append(states[i].toString()).append(" ");
        }
        System.out.println(sb.toString());
    }

    private int[] getForkIds(int myId) {
        int left = (5 + myId - 1) % 5;
        int right = (myId + 1) % 5;
        return new int[]{left, right};
    }
}
