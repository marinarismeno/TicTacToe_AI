using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class GameControllerScript : MonoBehaviour
{
    public int whoseTurn; // 0 for x and 1 for o
    public int board_size = 9;
    public Button[] board; // all the playable positions on the board
    public int[] markedBoard;
    public Image[] turnPointers; // point who's turn it is
    public Sprite[] xoImages;   
    public TextMeshProUGUI[] scoreText;
    public AudioSource buttonClickAudio;

    public GameObject resultPanel;
    public GameObject StartMenu;
    
    public int AI_game = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        markedBoard = new int[board_size];
        ResetGame();
        StartMenu.GetComponent<StartMenu>().StartPanelEnable(true);
    }

    /**
     * 
     * Resets the "who won" panel, all the variables used, the virtual board and the array board, and the winner lines
     * does not reset the score so that it keeps counting in every game
     */
    public void ResetGame()
    {
        resultPanel.SetActive(false); // who won panel disappears
        whoseTurn = 0; // x always starts first
        turnPointers[0].enabled = true;
        turnPointers[1].enabled = false;
        for (int i = 0; i < board.Length; i++)
        {
            board[i].interactable = true;
            board[i].image.sprite = xoImages[2]; // transparent background
            markedBoard[i] = -10; // reset board values
        }
        resultPanel.transform.GetChild(2).GetComponent<WinnerLines>().ResetWinnerLines();
    }

    /**
     * Deactivates the result Panel so that a new game can be played
     */
    public void NewGame()
    {
        resultPanel.SetActive(false);
    }

    public void ResetScore()
    {
        scoreText[0].text = 0.ToString();
        scoreText[1].text = 0.ToString();
    }

    /**
     * Toggles whose turn it is
     */
    private IEnumerator ToggleTurn()
    {
        if (whoseTurn == 0) //was Xs turn (human played)
        {
            whoseTurn = 1;
            turnPointers[0].enabled = false;
            turnPointers[1].enabled = true;
            if (AI_game != 0) //now the AI should play
            {
                yield return new WaitForSeconds(1); //make it wait for 1 second
                AI_turn(AI_game);
                
            }
        }           
        else // was Os turn
        {
            whoseTurn = 0;
            turnPointers[0].enabled = true;
            turnPointers[1].enabled = false;
        }
    }
    /**
     * sets the corresponding value to the array board and to the virtual board
     * uses the whichButton integet to show in which place of the board and array the value should be placed.
     * Then checks if the game is over by calling CheckWinner() and who won and changes whose turn it is by calling ToggleTurn().
     */
    public void SetValueOnBoard(int whichButton)
    {
        buttonClickAudio.Play();
        int result;
        board[whichButton].image.sprite = xoImages[whoseTurn];
       
        
        // Play Animation
        board[whichButton].image.color = new Color(255, 255, 255, 0);
        board[whichButton].image.gameObject.transform.DOPunchScale(new Vector3(.2f, .2f, .2f), 1);
        board[whichButton].image.DOFade(1, 0.5f);



        board[whichButton].interactable = false;
        markedBoard[whichButton] = whoseTurn +1;  // set a value on the board Array 1 for x and 2 for o

        result = CheckWinner(markedBoard);
        if (result != -1)
            GameOver(result);

        StartCoroutine(ToggleTurn());
        //ToggleTurn();
    }

    /**
     * Checks all the possible combinations that a victory could take place
     * and returns: 
     * 0 if X xon, 
     * 1 if O won,
     * 2 if there is a tie,
     * -1 if the game is not over yet
     * 
     */
    public int CheckWinner(int[] board)
    {
        int[] sum = new int[8];
        int winner = -1;
        int winningLine = -1;
        int emptySpots = 0;
        //horizontally 3 in a row
        sum[0] = board[0] + board[1] + board[2];
        sum[1] = board[3] + board[4] + board[5];
        sum[2] = board[6] + board[7] + board[8];
        //vertically 3 in a row
        sum[3] = board[0] + board[3] + board[6];
        sum[4] = board[1] + board[4] + board[7];
        sum[5] = board[2] + board[5] + board[8];
        //diagonally 3 in a row
        sum[6] = board[0] + board[4] + board[8];
        sum[7] = board[2] + board[4] +board[6];

        for (int i=0; i<sum.Length; i++)
        {
            if (sum[i] == 3) // since Xs -> 1+1+1
            {
                winner = 0; // Xs won  
                winningLine = i;
            }                           
            else if (sum[i] == 6) // since Os -> 2+2+2
            {
                winner = 1; // Os won 
                winningLine = i;
            }                             
        }
        for (int i = 0; i < board.Length; i++)
        {
            if(board[i] == -10)
            {
                emptySpots++;
            }
        }
        if (winner == -1 && emptySpots == 0)
            return 2; //tie
        else if (winner == 0 || winner == 1)
        {
            resultPanel.transform.GetChild(2).GetComponent<WinnerLines>().ShowWinnerLine(winningLine);
            return winner; //we have a winner
        }            
        else
            return -1; //keep playing, the game isn't over
    }

    /**
     * Is is called when the game is over
     * It decides whether someone won or there is a tie
     * and shows who won by calling the ShowWinner function
     */
    private void GameOver(int result)
    {
        // up the score
        if (result != 2) // not a tie
        {
            int prev_score = int.Parse(scoreText[result].text);
            scoreText[result].text = (prev_score + 1).ToString();
        }
        else // tie
            for (int i = 0; i < 2; i++) 
            {
                int prev_score = int.Parse(scoreText[i].text);
                scoreText[i].text = (prev_score + 1).ToString();
            }       
        ShowWinner(result); // show who won
    }

    /**
     * Activates the resultPanel that Shows the Winner
     */
    private void ShowWinner(int winner)
    {
        resultPanel.GetComponent<ShowResult>().ShowWinner(winner);
        resultPanel.SetActive(true);
    }

    /**
     * Calls the respective function depending on the difficulty the player chose 
     */
    public void AI_turn(int difficulty)
    {
        if (CheckWinner(markedBoard) == -1) // the game hasn't finished
        {
            if (difficulty == 1)// easy mode 
                Easy_mode();
            else if (difficulty == 2)// medium mode 
                Smart_mode(4); // minimax with maximum depth = 4
            else if (difficulty == 3)
                Smart_mode(1000); // hard mode calls minimax with no maximum depth (maximum depth = 1000)
        }
    }

    /**
     * The AI finds all the available spots 
     * and chooses to play randomly on one of them
     */
    public void Easy_mode()
    {
        int avSpCounter=0;
        int spot;
        for (int i = 0; i < board_size; i++)
            if (markedBoard[i] == -10)
                avSpCounter++; //count the empty spots on the board
        int[] availableSpots = new int[avSpCounter];

        int j = 0;
        for (int i = 0; i < board_size; i++)
        {
            if (markedBoard[i] == -10) // empty spot on board
            {
                availableSpots[j] = i;
                j++;
            }
        }
        spot = Random.Range(0, avSpCounter); //random available spot
        SetValueOnBoard(availableSpots[spot]); 
    }

    /**
     * The Starting call of the minimax algorithm in each empty spot
     * 
     */
    public void Smart_mode(int maxDepth)
    {
        int bestMove = -1;
        int score;
        int bestScore = -1000;

        for (int i = 0; i < board_size; i++)
        {
            if (markedBoard[i] == -10) // empty spot on board
            {
                markedBoard[i] = whoseTurn +1; // trial change
                score = minimax(markedBoard, 0, true, maxDepth); // starting call of minimax
                Debug.Log("best move : " + i + " and score: " + score);
                markedBoard[i] = -10; //undo the trial change
                if (score >= bestScore) // best move is max
                {
                    bestScore = score;
                    bestMove = i;
                    
                    Debug.Log("best move for position: " + bestMove + " and score: " + bestScore);

                }
            }
        }
        SetValueOnBoard(bestMove);
    }

    /**
     * Implementing the minimax algorithm
     * with the maximizing player being the AI
     * returns the optimal score
     * 
     */
   public int minimax(int[] board, int depth, bool isMaximizing, int maxDepth)
    {
        int bestScore;
        int score;        
        int result = CheckWinner(board);

        if (result != -1 || depth > maxDepth) // the game has finished OR we don't want to check further
        {
            resultPanel.transform.GetChild(2).GetComponent<WinnerLines>().ResetWinnerLines();

            if (result == 0)//x won (player)
                return -10; // + depth;
            else if (result == 1)// o won (AI)
                return 10; // - depth;
            else //tie OR reached max depth
                return 0;
        }
        
        if (isMaximizing) // AI player i.e. maximizing player
        {
            bestScore = -1000; //initialize bestScore to sth very low
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == -10) // empty spot on board
                {
                    markedBoard[i] = 1; //set a trial x in this spot (0+1..x)
                    score = minimax(markedBoard, depth +1, !isMaximizing, maxDepth); // call minimax recursively
                    //Debug.Log("Maximizing " + depth + " position: " + i + " score: " + score);
                    markedBoard[i] = -10; //undo the trial change

                    bestScore = Mathf.Max(score, bestScore); // choose the maximum score
                }   
            }
            return bestScore;
        }
        else // player ie minimizing player
        {
            bestScore = 1000; //initialize bestScore to sth very high
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == -10) // empty spot on board
                {
                    markedBoard[i] = 2; // set a trial o in this spot
                    score = minimax(markedBoard, depth +1, !isMaximizing, maxDepth); // call minimax recursively
                    //Debug.Log("Maximizing " + depth + " position: " + i + " score: " + score);
                    markedBoard[i] = -10; //undo the trial change

                    bestScore = Mathf.Min(score, bestScore); // choose the minimum score
                }
            }
            return bestScore;
        }
    }

}
