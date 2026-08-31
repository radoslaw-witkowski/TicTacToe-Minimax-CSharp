using System;

class TicTacToe
{
    static char[] board = Array.Empty<char>();
    static int boardSize;

    static readonly char player = 'X';
    static readonly char ai = 'O';

    static string difficulty = "";

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Witaj w grze Kółko i Krzyżyk!");
        Console.WriteLine("Wybierz rozmiar planszy: 3 lub 4");

        while (!int.TryParse(Console.ReadLine(), out boardSize)
               || (boardSize != 3 && boardSize != 4))
        {
            Console.WriteLine("Niepoprawny wybór. Wpisz 3 lub 4:");
        }

        ReadDifficulty();

        ResetBoard();
        DrawBoard();

        while (true)
        {
            PlayerMove();

            if (CheckGameOver(player))
                break;

            AIMove();

            if (CheckGameOver(ai))
                break;
        }
    }

    static void ReadDifficulty()
    {
        while (true)
        {
            Console.WriteLine("Wybierz poziom trudności: latwy, sredni, trudny");

            difficulty = (Console.ReadLine() ?? "")
                .Trim()
                .ToLowerInvariant();

            if (difficulty == "latwy"
                || difficulty == "sredni"
                || difficulty == "trudny")
            {
                return;
            }

            Console.WriteLine("Niepoprawny poziom trudności.");
        }
    }

    static void ResetBoard()
    {
        board = new char[boardSize * boardSize];
    }

    static void DrawBoard()
    {
        Console.Clear();

        for (int row = 0; row < boardSize; row++)
        {
            for (int column = 0; column < boardSize; column++)
            {
                int index = row * boardSize + column;

                string value = IsEmpty(index)
                    ? (index + 1).ToString()
                    : board[index].ToString();

                Console.Write($" {value,2} ");

                if (column < boardSize - 1)
                    Console.Write("|");
            }

            Console.WriteLine();

            if (row < boardSize - 1)
            {
                Console.WriteLine(
                    new string('-', boardSize * 5 - 1)
                );
            }
        }

        Console.WriteLine();
    }

    static void PlayerMove()
    {
        int move;

        while (true)
        {
            Console.Write($"Wybierz pole (1-{board.Length}): ");

            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out move)
                && move >= 1
                && move <= board.Length
                && IsEmpty(move - 1))
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
        int maxDepth = GetMaxDepth();

        int bestScore = int.MinValue;
        int bestMove = -1;

        for (int i = 0; i < board.Length; i++)
        {
            if (!IsEmpty(i))
                continue;

            board[i] = ai;

            int score = RunMinimax(
                depth: 0,
                isAITurn: false,
                maxDepth: maxDepth,
                alpha: int.MinValue,
                beta: int.MaxValue
            );

            board[i] = '\0';

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = i;
            }
        }

        if (bestMove >= 0)
        {
            board[bestMove] = ai;

            Console.WriteLine(
                $"AI wybrało pole {bestMove + 1}"
            );

            DrawBoard();
        }
    }

    static int GetMaxDepth()
    {
        if (difficulty == "latwy")
            return 1;

        if (difficulty == "sredni")
            return 3;

        // Full search is practical for a 3x3 board.
        if (boardSize == 3)
            return 9;

        // Full Minimax on 4x4 would be extremely expensive.
        return 5;
    }

    static int RunMinimax(
        int depth,
        bool isAITurn,
        int maxDepth,
        int alpha,
        int beta)
    {
        if (IsWinner(ai))
            return 10000 - depth;

        if (IsWinner(player))
            return depth - 10000;

        if (IsBoardFull())
            return 0;

        if (depth >= maxDepth)
            return EvaluateBoard();

        if (isAITurn)
        {
            int bestScore = int.MinValue;

            for (int i = 0; i < board.Length; i++)
            {
                if (!IsEmpty(i))
                    continue;

                board[i] = ai;

                int score = RunMinimax(
                    depth + 1,
                    false,
                    maxDepth,
                    alpha,
                    beta
                );

                board[i] = '\0';

                bestScore = Math.Max(bestScore, score);
                alpha = Math.Max(alpha, bestScore);

                if (beta <= alpha)
                    break;
            }

            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;

            for (int i = 0; i < board.Length; i++)
            {
                if (!IsEmpty(i))
                    continue;

                board[i] = player;

                int score = RunMinimax(
                    depth + 1,
                    true,
                    maxDepth,
                    alpha,
                    beta
                );

                board[i] = '\0';

                bestScore = Math.Min(bestScore, score);
                beta = Math.Min(beta, bestScore);

                if (beta <= alpha)
                    break;
            }

            return bestScore;
        }
    }

    static int EvaluateBoard()
    {
        int score = 0;

        for (int i = 0; i < boardSize; i++)
        {
            score += EvaluateLine(
                EnumerableRow(i)
            );

            score += EvaluateLine(
                EnumerableColumn(i)
            );
        }

        char[] diagonal1 = new char[boardSize];
        char[] diagonal2 = new char[boardSize];

        for (int i = 0; i < boardSize; i++)
        {
            diagonal1[i] =
                board[i * boardSize + i];

            diagonal2[i] =
                board[i * boardSize
                      + (boardSize - i - 1)];
        }

        score += EvaluateLine(diagonal1);
        score += EvaluateLine(diagonal2);

        return score;
    }

    static char[] EnumerableRow(int row)
    {
        char[] result = new char[boardSize];

        for (int i = 0; i < boardSize; i++)
        {
            result[i] =
                board[row * boardSize + i];
        }

        return result;
    }

    static char[] EnumerableColumn(int column)
    {
        char[] result = new char[boardSize];

        for (int i = 0; i < boardSize; i++)
        {
            result[i] =
                board[i * boardSize + column];
        }

        return result;
    }

    static int EvaluateLine(char[] line)
    {
        int aiCount = 0;
        int playerCount = 0;

        foreach (char cell in line)
        {
            if (cell == ai)
                aiCount++;

            if (cell == player)
                playerCount++;
        }

        // Blocked line.
        if (aiCount > 0 && playerCount > 0)
            return 0;

        if (aiCount > 0)
            return GetLineScore(aiCount);

        if (playerCount > 0)
            return -GetLineScore(playerCount);

        return 0;
    }

    static int GetLineScore(int count)
    {
        return count switch
        {
            1 => 1,
            2 => 10,
            3 => 100,
            4 => 1000,
            _ => 0
        };
    }

    static bool IsEmpty(int index)
    {
        return board[index] == '\0';
    }

    static bool IsBoardFull()
    {
        for (int i = 0; i < board.Length; i++)
        {
            if (IsEmpty(i))
                return false;
        }

        return true;
    }

    static bool CheckGameOver(char symbol)
    {
        if (IsWinner(symbol))
        {
            Console.WriteLine(
                symbol == player
                    ? "Wygrałeś!"
                    : "AI wygrało!"
            );

            return true;
        }

        if (IsBoardFull())
        {
            Console.WriteLine("Remis!");
            return true;
        }

        return false;
    }

    static bool IsWinner(char symbol)
    {
        for (int i = 0; i < boardSize; i++)
        {
            bool rowWin = true;
            bool columnWin = true;

            for (int j = 0; j < boardSize; j++)
            {
                if (board[i * boardSize + j] != symbol)
                    rowWin = false;

                if (board[j * boardSize + i] != symbol)
                    columnWin = false;
            }

            if (rowWin || columnWin)
                return true;
        }

        bool diagonal1 = true;
        bool diagonal2 = true;

        for (int i = 0; i < boardSize; i++)
        {
            if (board[i * boardSize + i] != symbol)
                diagonal1 = false;

            if (board[
                    i * boardSize
                    + (boardSize - i - 1)
                ] != symbol)
            {
                diagonal2 = false;
            }
        }

        return diagonal1 || diagonal2;
    }
}
