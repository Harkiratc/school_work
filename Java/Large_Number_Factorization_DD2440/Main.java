import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.math.BigInteger;
import java.util.Collections;
import java.util.HashMap;
import java.util.Iterator;
import java.util.LinkedList;
import java.util.Set;


/**
 * Reads numbers from input, sorts and factorizes them. Returns results within 15 seconds 
 * @author Kim
 * @author Villiam
 *
 */
public class Main {

	private static HashMap<BigInteger, Integer> readNumbers = new HashMap<BigInteger, Integer>();
	private static LinkedList<BigInteger> alreadyPrimes = new LinkedList<BigInteger>();
	public static void main(String[] args) throws IOException {
		Factorization f = new Factorization();
		long start = System.currentTimeMillis();
		int index = 0;
		LinkedList<BigInteger> ls = readAndSortInput();
		LinkedList<BigInteger>[] factorList = new LinkedList[readNumbers.size()];
		LinkedList<BigInteger> tmp;
		for(BigInteger b : alreadyPrimes)
		{
			index = readNumbers.get(b);
			tmp = new LinkedList<BigInteger>();
			tmp.add(b);
			factorList[index] = tmp;
		}
		try {
			for(BigInteger b : ls){
				index = readNumbers.get(b);
				factorList[index] = f.factorNumber(b,start + 14700);
			}
		} catch (FactorizationException e) {
			
		}
		for (int i = 0; i < factorList.length; i++) {
			if(factorList[i] == null){
				System.out.println("fail");
			}
			else{
				for(BigInteger b : factorList[i])
					System.out.println(b);
			}
			System.out.println();
		}
		System.exit(0);
	}
	// Sort imput by size, smallest numbers first except if number is already a prime, then put it first
	// use hashmap to store calculations already done
	// datastructure to remember which position number was in during readout
	private static LinkedList<BigInteger> readAndSortInput() throws IOException
	{
		LinkedList<String> lines = new LinkedList<String>();
		BufferedReader br = new BufferedReader(
				new InputStreamReader(System.in));
		LinkedList<BigInteger> factors;
		String line;
		int i = 0;
		BigInteger num;
		while ((line = br.readLine()) != null) {
			num = new BigInteger(line.trim());
			readNumbers.put(num, i);
			i++;
		}
		Set<BigInteger> keySet = readNumbers.keySet();
		LinkedList<BigInteger> ls = new LinkedList<BigInteger>();
		for(BigInteger b : keySet){
			ls.add(b);
		}
		Collections.sort(ls);
		BigInteger b;
		Iterator<BigInteger> it = ls.iterator();
		while(it.hasNext()){
			b = it.next();
			if(b.isProbablePrime(10)){
				it.remove();
				alreadyPrimes.add(b);
			}
		}
		br.close();
		return ls;
	}
}
