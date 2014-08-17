

/*
 * Dining Philosophers main class
 */

/**
 *
 * @author Kim Persson
 */
public class DiningPhilosophers {
    
    public static void main(String[] args) {
        int rounds, maxEatingTime, maxThinkingTime;
        long seed;
        
        if(args.length == 0)
            System.out.println("USAGE:  java DiningPhilosophers #rounds [default = 3] maxEatingTime [default = 1000] maxThinkingTime [default = 1000] random_seed [default = currentTimeMillis()]");
        
        rounds = args.length >= 1 ? Integer.parseInt(args[0]) : 3;
        maxEatingTime = args.length >= 2 ? Integer.parseInt(args[1]) : 1000;
        maxThinkingTime =  args.length >= 3 ? Integer.parseInt(args[2]) : 1000;
        seed = args.length >= 4 ? Long.parseLong(args[3]) : System.currentTimeMillis();
        
        

        Table table = new Table();
        Philosopher philoArr[] = new Philosopher[5];
        for (int i = 0; i < 5; i++) {
            philoArr[i] = new Philosopher(i, rounds, table, maxEatingTime, maxThinkingTime, seed);
            philoArr[i].start();
            System.out.println(System.currentTimeMillis() + " : Philosopher " + i + " has started");
        }
    }
    
}
