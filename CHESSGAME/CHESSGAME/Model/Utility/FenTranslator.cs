using System;
using CHESSGAME.Model.AModel;
using CHESSGAME.Model.AModel.Pieces;
using Type = CHESSGAME.Model.AModel.Pieces.Type;

namespace CHESSGAME.Model.Utility
{
    public class FenTranslator
    {
        public static string FenNotation(Container container)
        {
            Board board = container.Board;

            string result = ""; // Lưu trữ chuỗi FEN

            // Lặp qua các cô trong bàn cờ
            for (int i = 0; i < board.Size; i++)
            {
                int emptySquareNumber = 0; // Biến lưu trữ số ô trống liên tiếp
                for (int j = 0; j < board.Size; j++)
                {
                    Square square = board.Squares[j, i];
                    if (square.Piece != null) // Nếu có quân cờ
                    {
                        if (emptySquareNumber != 0)
                        {
                            result += emptySquareNumber;
                            emptySquareNumber = 0;
                        }
                        char c = ' ';

                        switch (square.Piece.Type)
                        {
                            case Type.Bishop:
                                c = 'b';
                                break;
                            case Type.King:
                                c = 'k';
                                break;
                            case Type.Queen:
                                c = 'q';
                                break;
                            case Type.Pawn:
                                c = 'p';
                                break;
                            case Type.Knight:
                                c = 'n';
                                break;
                            case Type.Rook:
                                c = 'r';
                                break;
                        }

                        // Thêm ký tự đại diện quân cờ vào result
                        result += square.Piece.Color == Color.White ? c.ToString().ToUpper() : c.ToString();
                    }
                    else
                    {
                        emptySquareNumber++;
                    }
                }
                if (emptySquareNumber != 0)
                    result += emptySquareNumber;
                result += '/'; // Thêm dấu "/" để phân tách các hàng
            }

            // Lượt chơi hiện tại
            result += ' ';
            result += container.Moves[container.Moves.Count - 1].PieceColor == Color.White ? 'b' : 'w';
            result += ' ';

            // Xác định các quân xe và vua để thực hiện nhập thành
            Piece blackRookQueen = null;
            Piece blackRookKing = null;
            Piece whiteRookQueen = null;
            Piece whiteRookKing = null;

            Piece blackKing = null;
            Piece whiteKing = null;

            Square enPassant = null;

            // Xác định vị trí của quân vua và quân xe
            foreach (Square square in board.Squares)
                if (square?.Piece?.Type == Type.King)
                    if (square.Piece.Color == Color.White)
                        whiteKing = square.Piece;
                    else
                        blackKing = square.Piece;
                else if (square?.Piece?.Type == Type.Rook)
                    if (square.X == 0)
                    {
                        if (square.Piece.Color == Color.White)
                            whiteRookQueen = square.Piece;
                        else
                            blackRookQueen = square.Piece;
                    }
                    else
                    {
                        if (square.Piece.Color == Color.White)
                            whiteRookKing = square.Piece;
                        else
                            blackRookKing = square.Piece;
                    }

                // Kiểm tra enPassant
                else if (square?.Piece?.Type == Type.Pawn)
                    if ((square.Piece as Pawn)?.EnPassant == true)
                        if (square?.Piece.Color == container.Moves[container.Moves.Count - 1].PieceColor)
                            enPassant =
                                board.Squares[square.X, square.Piece.Color == Color.White ? square.Y + 1 : square.Y - 1];

            // Kiểm tra quyền nhập thành
            var bRQ = !blackRookQueen?.HasMoved == true;
            var bRK = !blackRookKing?.HasMoved == true;
            var wRQ = !whiteRookQueen?.HasMoved == true;
            var wRK = !whiteRookKing?.HasMoved == true;

            var wK = !whiteKing.HasMoved;
            var bK = !blackKing.HasMoved;

            if (wK)
            {
                if (wRK) result += 'K'; // Quân trắng có thể nhập thành bên vua
                if (wRQ) result += 'Q'; // Quân trắng có thể nhập thành bên hậu
            }
            if (bK)
            {
                if (bRK) result += 'k'; // Quân đen có thể nhập thành bên vua
                if (bRQ) result += 'q'; // Quân đen có thể nhập thành bên hậu
            }

            if (!(bK && (bRK || bRQ))
                && !(wK && (wRK || wRQ)))
                result += '-'; // Không có quyền nhập thành

            result += ' ';

            //Tọa độ nước đi

            if (enPassant != null)
                result += enPassant.ToString().ToLower();
            else
                result += '-';

            result += ' ';

            // Số nước đi không bắt được quân
            result += container.HalfMoveSinceLastCapture;

            result += ' ';

            // Số nước đi tổng cộng
            result += (int)Math.Ceiling((double)(container.Moves.Count / 2));

            return result;
        }
    }
}