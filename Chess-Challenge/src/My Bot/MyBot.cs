using ChessChallenge.API;
using System;
using System.Collections.Generic;

public class MyBot : IChessBot
{
    List<(PieceType Type, int Value)> PieceValues = new()
    {
        (PieceType.Queen, 9),
        (PieceType.Rook, 5),
        (PieceType.Bishop, 3),
        (PieceType.Knight, 3),
        (PieceType.Pawn, 1)
    };

    int SearchDepth = 2;
    Random rng = new();

    public Move Think(Board board, Timer timer)
    {
        var moves = board.GetLegalMoves();
        bool isWhite = board.IsWhiteToMove;
        var bestEval = -100000;
        var bestMoves = new List<Move>();
        foreach (var move in moves)
        {
            var eval = MiniMax(board, isWhite, move, 0);
            if (eval > bestEval)
            {
                bestEval = eval;
                bestMoves.Clear();
                bestMoves.Add(move);
            }
            if(eval == bestEval) bestMoves.Add(move);
        }
        return bestMoves[rng.Next(bestMoves.Count)];
    }

    int MiniMax(Board board, bool isWhite, Move move, int depth)
    {
        if (depth == SearchDepth)
        {
            board.MakeMove(move);
            var result = Evaluate(board, isWhite);
            board.UndoMove(move);
            return result;
        }
        else
        {
            board.MakeMove(move);
            var bestEval = 10000;
            foreach (var mov in board.GetLegalMoves())
            {
                var eval = MiniMax(board, !isWhite, mov, depth + 1);
                if (eval < bestEval) bestEval = eval;
            }
            board.UndoMove(move);
            return bestEval;
        }
    }

    int Evaluate(Board board, bool isWhite)
    {
        var result = 0;
        foreach (var pieceValue in PieceValues)
        {
            result += board.GetPieceList(pieceValue.Type, true).Count * pieceValue.Value;
            result -= board.GetPieceList(pieceValue.Type, false).Count * pieceValue.Value;
        }
        
        result = isWhite ? result : -result;
        if (board.IsInCheck()) result += 2;
        return result;
    }
}