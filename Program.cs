using System;
using System.Collections.Generic;

class TicTacToe
{
    static char[] board;
    static int boardSize;
    static char player = 'X';
    static char ai = 'O';
    static string difficulty;

    static void Main()
    {
        Console.WriteLine("Witaj w grze Kółko i Krzyżyk!");
        Console.WriteLine("Wybierz rozmiar planszy: 3 lub 4");
        while (!int.TryParse(Console.ReadLine(), out boardSize) || (boardSize != 3 && boardSize != 4))
        {
            Console.WriteLine("Niepoprawny wybór. Wpisz 3 lub 4:");
        }

        Console.WriteLine("Wybierz poziom trudności: latwy, sredni, trudny");
        difficulty = Console.ReadLine().ToLower();

        ResetBoard();
        DrawBoard();

        while (true)
        {
            PlayerMove();
            if (CheckGameOver(player)) break;

            AIMove();
            if (CheckGameOver(ai)) break;
        }
    }

    static void ResetBoard()
    {
        board = new char[boardSize * boardSize];
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = (char)('1' + i);
        }
    }

    static void DrawBoard()
    {
        Console.Clear();
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                Console.Write(" {0} ", board[i * boardSize + j]);
                if (j < boardSize - 1) Console.Write("|");
            }
            Console.WriteLine();
            if (i < boardSize - 1)
            {
                Console.WriteLine(new string('-', boardSize * 4 - 1));
            }
        }
    }

    static void PlayerMove()
    {
        int move;
        while (true)
        {
            Console.Write("Wybierz pole (1-{0}): ", board.Length);
            string input = Console.ReadLine();
            if (int.TryParse(input, out move) && move >= 1 && move <= board.Length && board[move - 1] != 'X' && board[move - 1] != 'O')
            {
                board[move - 1] = player;
                break;
            }
            Console.WriteLine("Niepoprawny ruch. Spróbuj ponownie.");
        }
        DrawBoard();
    }

    static void AIMove()
    {
        int maxDepth;
        if (difficulty == "latwy") maxDepth = 1;
        else if (difficulty == "sredni") maxDepth = 3;
        else maxDepth = int.MaxValue;

        int bestScore = int.MinValue;
        int move = -1;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] != 'X' && board[i] != 'O')
            {
                char backup = board[i];
                board[i] = ai;

                int score = RunMinimax(0, false, maxDepth);

                board[i] = backup;

                if (score > bestScore)
                {
                    bestScore = score;
                    move = i;
                }
            }
        }

        board[move] = ai;
        Console.WriteLine("AI wybrało pole {0}", move + 1);
        DrawBoard();
    }

    static int RunMinimax(int depth, bool isAI, int maxDepth)
    {
        if (IsWinner(ai)) return 10 - depth;
        if (IsWinner(player)) return depth - 10;
        if (IsBoardFull() || depth >= maxDepth) return 0;

        int bestScore = isAI ? int.MinValue : int.MaxValue;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] != 'X' && board[i] != 'O')
            {
                char backup = board[i];
                board[i] = isAI ? ai : player;

                int score = RunMinimax(depth + 1, !isAI, maxDepth);

                board[i] = backup;

                if (isAI)
                    bestScore = Math.Max(score, bestScore);
                else
                    bestScore = Math.Min(score, bestScore);
            }
        }

        return bestScore;
    }

    static bool IsBoardFull()
    {
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] != 'X' && board[i] != 'O') return false;
        }
        return true;
    }

    static bool CheckGameOver(char symbol)
    {
        if (IsWinner(symbol))
        {
            Console.WriteLine(symbol == player ? "Wygrales!" : "AI wygralo!");
            return true;
        }
        if (IsBoardFull())
        {
            Console.WriteLine("Remis!");
            return true;
        }
        return false;
    }

    static bool IsWinner(char s)
    {
        for (int i = 0; i < boardSize; i++)
        {
            bool rowWin = true;
            bool colWin = true;
            for (int j = 0; j < boardSize; j++)
            {
                if (board[i * boardSize + j] != s) rowWin = false;
                if (board[j * boardSize + i] != s) colWin = false;
            }
            if (rowWin || colWin) return true;
        }

        bool diag1 = true, diag2 = true;
        for (int i = 0; i < boardSize; i++)
        {
            if (board[i * boardSize + i] != s) diag1 = false;
            if (board[i * boardSize + (boardSize - i - 1)] != s) diag2 = false;
        }
        return diag1 || diag2;
    }
}

