using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PlayerType { PlayerOne, PlayerTwo }
public enum GameState
{
    None,
    PlayerTurn,
    OpponentTurn,
    GameOver,
    OpponentDisconnect
}
public enum Player { Player, Opponent } 

public class TicTacToeManager : MonoBehaviour {
    
    public Sprite circleSprite;  
    public Sprite crossSprite;
    public GameObject[] cells;
    public RectTransform gameOverPanel;

    enum Mark { Circle, Cross }

    PlayerType playerType;              
    int[] cellStates = new int[9];   
    TicTacToeNetwork networkManager;

    //게임 승패
    enum Winner {None, Player, Opponent,Tie }
    int rowNum = 3;

    GameState gameState;   

    private void Awake()
    {
        gameState = GameState.None;       //원본 코드
        //gameState = GameState.PlayerTurn;   //*테스트를 위해 임시 하드코딩용*/
        //playerType = PlayerType.PlayerOne;  //*테스트를 위해 임시 하드코딩용*/
    }
    private void Start()
    {
        networkManager = GetComponent<TicTacToeNetwork>();

        for(int i = 0;  i <cellStates.Length; i++)
        {
            cellStates[i] = -1;
        }
    }

    private void Update()
    {
        if(gameState == GameState.PlayerTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = 
                    new Vector2(Input.mousePosition.x, 
                    Input.mousePosition.y);
                RaycastHit2D hitInfo = 
                    Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), 
                    Vector2.zero);
                
                if (hitInfo)
                {
                    GameObject cell = hitInfo.transform.gameObject;
                    int cellIndex = int.Parse(cell.name);

                    //서버에 cell index 전달.
                    networkManager.DoPlayer(cellIndex);

                                       
                    //Todo : 셀에 o 혹은 x 표시하기 
                    DrawMark(cellIndex, Player.Player);     //*테스트를 위해 임시 하드코딩용*/

                }
            }
        }
    }
    public void DrawMark(int cellIndex, Player player)
    {
        Sprite markSprite;
        if(player == Player.Player)
        {
            markSprite = 
                playerType == PlayerType.PlayerOne ? circleSprite : crossSprite;
            cellStates[cellIndex] = 
                playerType == PlayerType.PlayerOne ? 0 : 1;
        }
        else {  
            markSprite = 
                playerType == PlayerType.PlayerOne ? crossSprite : circleSprite;
            cellStates[cellIndex] = 
                playerType == PlayerType.PlayerOne ? 1 : 0;
        }

        /*
        cells[cellIndex].GetComponent<SpriteRenderer>().sprite = markSprite;
        cells[cellIndex].GetComponent<BoxCollider2D>().enabled = false;
        */

        GameObject go = cells[cellIndex];
        go.GetComponent<SpriteRenderer>().sprite = markSprite;
        go.GetComponent<BoxCollider2D>().enabled = false;
        
        go.transform.DOScale(2, 0).OnComplete(
            () =>
            {
                go.transform.DOScale(1, 1).SetEase(Ease.OutBounce);
                go.GetComponent<SpriteRenderer>().DOFade(1, 1);
                Camera.main.DOShakePosition(0.5f, 1, 15);
            }
        );

        //턴 변경
        //1 게임이 계속 진행중이어야 한다.
        Winner result = CheckWinner();
        if(result == Winner.None)
        {
            if (gameState == GameState.PlayerTurn)
            {
                gameState = GameState.OpponentTurn;
            }
            else if(gameState == GameState.OpponentTurn)
            {
                gameState = GameState.PlayerTurn;
            }
        }
        else
        {
            gameState = GameState.GameOver;
            ShowGameOverPanel();
           // gameOverPanel.anchoredPosition = new Vector2(0, 0);
            //TODO : 게임의 결과 화면에 표시

            if (result == Winner.Player)
            {
                Debug.Log("Player Win");
                
            }
            else if (result == Winner.Opponent)
            {
                Debug.Log("Opponent Win");
            }
            else if (result == Winner.Tie)
            {
                Debug.Log("Tie");
            }
        }
        
    }

    Winner CheckWinner()
    {
        //return Winner.None;

        int playerTypeValue = (int)playerType;
        //가로 체크
        for (int y = 0; y < rowNum; ++y)
        {
            int mark = cellStates[y + rowNum];
            int num = 0;
            for (int x = 0; x < rowNum; ++x)
            {
                int index = y * rowNum + x;
                if (mark == cellStates[index])
                {
                    ++num;
                }
            }
            if (mark != -1 && num == rowNum)
            {
                int markValue = (int)mark;
                return (playerTypeValue == markValue) ? Winner.Player : Winner.Opponent;
            }
        }
        //세로 체크
        for (int x = 0; x < rowNum; ++x)
        {
            int mark = cellStates[x];
            int num = 0;
            for (int y = 0; y < rowNum; ++y)
            {
                int index = y * rowNum + x;
                if (mark == cellStates[index])
                {
                    ++num;
                }
            }
            if (mark != -1 && num == rowNum)
            {
                int markValue = (int)mark;
                return (playerTypeValue == markValue) ? Winner.Player : Winner.Opponent;
            }
        }

        {
            int mark = cellStates[0];
            int num = 0;
            for (int xy = 0; xy < rowNum; ++xy)
            {
                int index = xy * rowNum + xy;
                if (mark == cellStates[index])
                {
                    ++num;
                }
            }
            if (mark != -1 && num == rowNum)
            {
                int markValue = (int)mark;
                return (playerTypeValue == markValue) ? Winner.Player : Winner.Opponent;
            }
        }

        {
            int mark = cellStates[rowNum - 1];
            int num = 0;
            for (int xy = 0; xy < rowNum; ++xy)
            {
                int index = xy * rowNum + rowNum - xy - 1;
                if (mark == cellStates[index])
                {
                    ++num;
                }
            }

            if (mark != -1 && num == rowNum)
            {
                int markValue = (int)mark;
                return (playerTypeValue == markValue) ? Winner.Player : Winner.Opponent;
            }
        }

        {
            int num = 0;
            foreach (int cellState in cellStates)
            {
                if (cellState == -1)
                {
                    ++num;
                }
            }
            if (num == 0)
            {
                return Winner.Tie;
            }
        }
        return Winner.None;
    }

    public void StartGame(PlayerType type)
    {
        playerType = type;
        if(type == PlayerType.PlayerOne)
        {
            gameState = GameState.PlayerTurn;
        }
        else
        {
            gameState = GameState.OpponentTurn;
        }
    }


    public void ShowGameOverPanel()
    {
        //gameOverPanel.DOLocalMove
        gameOverPanel.DOLocalMoveY(0, 1).SetEase(Ease.InOutBack);
        gameOverPanel.GetComponent<CanvasGroup>().DOFade(1, 2);
    }


}

