using ChessChallenge.API;

public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();
        bool isWhite = board.IsWhiteToMove;
        var bestMove = moves[0];
        var bestEval = -100000;
        foreach (var move in moves)
        {
            board.MakeMove(move);
            var eval = Evaluation(board, isWhite);
            if (eval > bestEval)
            {
                bestEval = eval;
                bestMove = move;
            }
            board.UndoMove(move);
        }
        return bestMove;
    }

    int Evaluation(Board board, bool isWhite)
    {
        var result = 0;
        result += board.GetPieceList(PieceType.Queen, true).Count * 9;
        result += board.GetPieceList(PieceType.Rook, true).Count * 5;
        result += board.GetPieceList(PieceType.Bishop, true).Count * 3;
        result += board.GetPieceList(PieceType.Knight, true).Count * 3;
        result += board.GetPieceList(PieceType.Pawn, true).Count;

        result -= board.GetPieceList(PieceType.Queen, false).Count * 9;
        result -= board.GetPieceList(PieceType.Rook, false).Count * 5;
        result -= board.GetPieceList(PieceType.Bishop, false).Count * 3;
        result -= board.GetPieceList(PieceType.Knight, false).Count * 3;
        result -= board.GetPieceList(PieceType.Pawn, false).Count;

        return isWhite ? result : -result;
    }
}