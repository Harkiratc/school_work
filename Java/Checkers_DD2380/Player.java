

import java.util.*;

public class Player {
	/**
	 * Performs a move
	 * 
	 * @param pState
	 *            the current state of the board
	 * @param pDue
	 *            time before which we must have returned
	 * @return the next state the board is in after our move
	 */
	private static final int MIN = 0, MAX = 1;
	private static int player;
	private static final int tablePosValue[] = 
		{ 4, 4, 4, 4,
		4, 3, 3, 3,
		3, 2, 2, 4,
		4, 2, 2, 3,
		3, 2, 2, 4,
		4, 2, 2, 3,
		3, 3, 3, 4,
		4, 4, 4, 4};
	public GameState play(final GameState pState, final Deadline pDue) {

		Vector<GameState> lNextStates = new Vector<GameState>();
		pState.findPossibleMoves(lNextStates);
		if (lNextStates.size() == 0) {
			// Must play "pass" move if there are no other moves possible.
			return new GameState(pState, new Move());
		} // End if

		return doMiniMaxi(pState,pDue);
		/**
		 * Here you should write your algorithms to get the best next move, i.e.
		 * the best next state. This skeleton returns a random move instead.
		 */
	} // End Play
	private GameState doMiniMaxi(final GameState pState, final Deadline pDue)
	{
		player = pState.getNextPlayer();
		Vector<GameState> lNextStates = new Vector<GameState>();
		pState.findPossibleMoves(lNextStates);
		int bestScore = Integer.MIN_VALUE, score;
		GameState bestMove = pState;
		for (GameState gs : lNextStates)
		{
			score = miniMax(gs, pDue, MIN,9, 1, Integer.MIN_VALUE, Integer.MAX_VALUE);
			if(score > bestScore)
			{
				bestScore = score;
				bestMove = gs;
			}
		}
		return bestMove;
	}
	private int calculateNumberOfCheckers(char[] board)
	{
		int counter = 0;
		for (int i = 0; i < board.length; i++)
		{
			if(board[i] == 'r' || board[i] == 'R' || board[i] == 'w' || board[i] == 'W')
				counter++;
		}
		return counter;
	}
	private int miniMax(final GameState pState, final Deadline pDue, int minOrMax, int maxDepth, int depth, int alpha, int beta)
	{		
		Vector<GameState> lNextStates = new Vector<GameState>();
		pState.findPossibleMoves(lNextStates);
		char[] board = pState.toMessage().substring(0, 32).toCharArray();
		int checkersLeft = calculateNumberOfCheckers(board);
		if (checkersLeft <= 2)
			maxDepth = 12;


		if(depth == maxDepth || pState.isEOG())
			return evaluate(pState, board, lNextStates.size(), checkersLeft);

		int score;
		if (minOrMax == MAX)
		{
			score = Integer.MIN_VALUE;
			for (GameState gs : lNextStates)
			{
				score = Math.max(score, miniMax(gs,pDue, MIN, maxDepth, depth +1 , alpha , beta));
				if (score >= beta)
					return score;

				alpha = Math.max(score, alpha);
			}
		} else
		{ //MIN
			score = Integer.MAX_VALUE;
			for(GameState gs : lNextStates)
			{
				score = Math.min(score, miniMax(gs,pDue, MAX, maxDepth, depth +1 , alpha , beta));
				if (score <= alpha)
					return score;
				
				beta = Math.min(score, beta);
			}
		}
		return score;

	}
	private int calcPosValue(int pos, int color)
	{
		int value;
		if (color == Constants.CELL_WHITE)
		{
			if (pos >= 4 && pos <=7)
				value = 7;
			else
				value = 5;
		}else 
		{
			if (pos >= 24 && pos <= 27)
				value = 7;
			else
				value = 5;
		}
		return value * tablePosValue[pos];
	}
	private int evaluate(final GameState pState,char[] board, int numberOfMoves, int checkersLeft)
	{
		if (pState.isEOG() && pState.getNextPlayer() == player)
			return 1000;

		int score = 0;

		for(int i = 0; i < board.length; i++)
		{
			if (player == Constants.CELL_RED && board[i] == 'r')
				score += (2 * calcPosValue(i, Constants.CELL_RED));
			else if (player == Constants.CELL_WHITE && board[i] == 'w')
				score += (2 * calcPosValue(i, Constants.CELL_WHITE));
			else if (player == Constants.CELL_RED && board[i] == 'R')
				score += ((checkersLeft <= 5 ? 7 : 6) * calcPosValue(i, Constants.CELL_RED));
			else if ( player == Constants.CELL_WHITE && board[i] == 'W')
				score += ((checkersLeft <= 5 ? 7 : 6) * calcPosValue(i, Constants.CELL_WHITE));
		}

		if(pState.getMove().isJump())
			score += 4;


		score += 2 * numberOfMoves;
		return score;
	}



} // End class Player
