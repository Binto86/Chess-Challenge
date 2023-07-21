using ChessChallenge.API;
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

    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();
        bool isWhite = board.IsWhiteToMove;
        var bestMove = moves[0];
        var bestEval = -100000;
        foreach (var move in moves)
        {
            board.MakeMove(move);
            var eval = Evaluate(board, isWhite);
            if (eval > bestEval)
            {
                bestEval = eval;
                bestMove = move;
            }
            board.UndoMove(move);
        }
        return bestMove;
    }

    int Evaluate(Board board, bool isWhite)
    {
        var result = 0;
        foreach (var pieceValue in PieceValues)
        {
            result += board.GetPieceList(pieceValue.Type, true).Count * pieceValue.Value;
            result += board.GetPieceList(pieceValue.Type, false).Count * pieceValue.Value;
        }

        return isWhite ? result : -result;
    }
}