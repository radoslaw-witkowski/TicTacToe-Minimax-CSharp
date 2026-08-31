# Tic Tac Toe with Minimax AI

Console-based Tic Tac Toe game written in C# and .NET.

The project features an AI opponent based on the **Minimax algorithm**, with alpha-beta pruning and multiple difficulty levels. The game supports both classic 3x3 and extended 4x4 boards.

## Features

- Play against an AI opponent in the console
- Minimax-based decision making
- Alpha-beta pruning for improved search performance
- Three difficulty levels
- 3x3 and 4x4 board support
- Input validation
- Win and draw detection
- Heuristic board evaluation for larger search spaces

## Technologies

- C#
- .NET 9
- Minimax algorithm
- Alpha-beta pruning

## Difficulty Levels

- **Easy** – shallow Minimax search
- **Medium** – deeper Minimax search
- **Hard**
  - full search on a 3x3 board
  - depth-limited search with heuristic evaluation on a 4x4 board

The depth limit on the 4x4 board prevents the search tree from becoming computationally impractical.

## How to Run

### Requirements

- .NET 9 SDK

Clone the repository:

```bash
git clone https://github.com/radoslaw-witkowski/TicTacToe-Minimax-CSharp.git
cd TicTacToe-Minimax-CSharp
```

Run the project:

```bash
dotnet run --project Tic-tac-toe/kolkoikrzyzyk.csproj
```

## Gameplay

After starting the application:

1. Choose the board size: `3` or `4`.
2. Choose the difficulty level:
   - `latwy`
   - `sredni`
   - `trudny`
3. Select a numbered field to place your `X`.
4. The AI plays as `O`.

Example 3x3 board:

```text
  1 |  2 |  3
--------------
  4 |  5 |  6
--------------
  7 |  8 |  9
```

## Author

Radosław Witkowski

