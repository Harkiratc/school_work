import java.math.BigDecimal;
import java.math.BigInteger;
import java.math.RoundingMode;
import java.util.LinkedList;
import java.util.Timer;
import java.util.TimerTask;

/**
 * Factorizes a number N using Pollard Brent Algorithm 
 * @author Kim
 * @author Villiam 
 */
public class Factorization {
	public static final int FAIL = -1;	
	private Pollard r = new Pollard();
	public Factorization()
	{

	}
	public LinkedList<BigInteger> factorNumber(BigInteger number, long deadline) throws FactorizationException
	{
		
		LinkedList<BigInteger> factors = new LinkedList<BigInteger>();
		factorNumber(number, factors, deadline);

		if(factors.isEmpty()){
			long start = System.currentTimeMillis();
			BigInteger[] res = perfectPower(number, deadline);


			if (res[0].compareTo(C.FAIL) != 0)
			{
				LinkedList<BigInteger> tmp = new LinkedList<BigInteger>(); 
				factorNumber(res[0], tmp, deadline);
				for (int i = 0; i < res[1].intValue(); i++) {
					factors.addAll(tmp);
				}
				System.err.println("Found perfect power in "+ (System.currentTimeMillis() - start) + " : " + res[0].toString() + "^" + " " +res[1].toString() + "=" + number.toString());
				return factors;
			}
			System.err.println("Wasted " + (System.currentTimeMillis() - start) + " on searching for perfect powers");

			return null;
		}
		else
			return factors;
	}
	private void factorNumber(BigInteger number, LinkedList<BigInteger> outFactors,long deadline) throws FactorizationException
	{
		
		if(number.compareTo(C.ONE) == 0)
		{
			return;
		}
		if(number.isProbablePrime(10))
		{
			outFactors.add(number);
			return;
		}

		BigInteger div = r.pollardBrent(number, deadline);
		if(div.compareTo(C.FAIL) == 0)
		{
			outFactors.clear();
			return;
		}
		factorNumber(div, outFactors,deadline);
		factorNumber(number.divide(div), outFactors,deadline);

	}

	
	private BigInteger[] perfectPower(BigInteger n, long deadline) throws FactorizationException
	{
		BigInteger a,c,m,p;
		int b = 2;
		int it = 0;
		while(C.TWO.pow(b).compareTo(n) <= 0)
		{
			a = C.ONE;
			c = n;
			while(c.subtract(a).compareTo(C.TWO) >= 0)
			{
				if(System.currentTimeMillis() >= deadline)
					throw new FactorizationException();
				if (it > 5500)
					return new BigInteger[] {C.FAIL,C.FAIL};
				m = a.add(c).divide(C.TWO);
				p = m.pow(b).min(n.add(C.ONE));
				if(p.compareTo(n) == 0)
				{
					return new BigInteger[] {m,new BigInteger(b + "")};
				}
				if (p.compareTo(n) < 0)
					a = m;
				else
					c = m;
				it++;
			}
			b++;
		}
		return new BigInteger[] {C.FAIL,C.FAIL};
		
	}
	

	public static BigInteger sqrt(BigInteger n) {
		BigInteger r = BigInteger.ZERO;
		BigInteger m = r.setBit(2 * n.bitLength());
		BigInteger nr;

		do {
			nr = r.add(m);
			if (nr.compareTo(n) != 1) {
				n = n.subtract(nr);
				r = nr.add(m);
			}
			r = r.shiftRight(1);
			m = m.shiftRight(2);
		} while (m.bitCount() != 0);

		return r;
	}
}
