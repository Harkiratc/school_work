import java.math.BigInteger;
import java.util.Random;

/**
 * Factorizes a given number with the Pollard Brent algorithm 
 * Trows FactorizationException if time limit is exceeded
 * Returns a factor of number N or a fail constant if calculations failed 
 * @author Kim
 * @author Villiam
 *
 */
public class Pollard{
	private Random r;
	public Pollard()
	{
		r = new Random();
	}
	public BigInteger pollardRho(BigInteger n, long deadline) throws FactorizationException{
		BigInteger div;
		BigInteger additive = new BigInteger(n.bitLength(), r);
		BigInteger xi = new BigInteger(n.bitLength(), r);
		BigInteger x2i = new BigInteger(n.bitLength(), r);
		if (n.mod(C.TWO).compareTo(C.ZERO) == 0)
			return C.TWO;
		
		do
		{
			if (System.currentTimeMillis() >= deadline)
				throw new FactorizationException();
			
			xi = xi.pow(2).mod(n).add(additive).mod(n);
			x2i = x2i.pow(2).mod(n).add(additive).mod(n);
			x2i = x2i.pow(2).mod(n).add(additive).mod(n);
			div = xi.subtract(x2i).gcd(n);
		}while(div.compareTo(C.ONE) == 0);
		
		return div;
	}
	
	public BigInteger pollardBrent(BigInteger n, long deadline) throws FactorizationException{
		if (n.mod(C.TWO).compareTo(C.ZERO) == 0)
			return C.TWO;

		BigInteger y = getRandomNumber(n);
		BigInteger c = getRandomNumber(n);
		int bitLength = n.bitLength();
		BigInteger x = y;
		BigInteger result = C.ONE,q =C.ONE;
		int j = 0, fast =1;

		while(result.compareTo(C.ONE) == 0){
			x = y;
			for(int i = 0; i < fast ;i++){
				y = y.multiply(y).add(c).mod(n);
			}
			j = 0;
			while(j < fast && result.compareTo(C.ONE) == 0){
				for(int k = 0; k < Math.min(bitLength, fast-j);k++){
					y = y.multiply(y).add(c).mod(n);
					q = q.multiply(x.subtract(y).abs()).mod(n);
				}
				result = q.gcd(n);
				j += bitLength;
				if (System.currentTimeMillis() >= deadline)
					throw new FactorizationException();
				if(j > 27000){
					result = C.FAIL;
					break;
				}
			}
			fast *=2;
		}
		return result;
	}
	private BigInteger getRandomNumber(BigInteger n){
		BigInteger bi;
		do{
			bi = new BigInteger(n.bitLength()-1,new Random()).mod(n);
		}
		while(bi.compareTo(n) == 0);
		return bi;
	}
}
