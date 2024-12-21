using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Script
{
    public class Main : MonoBehaviour
    {
        #region Prefab

        public GameObject cellPrefab;

        #endregion

        #region Links

        public GridLayoutGroup gridLayoutBoard;

        public Slider sliderWidth;
        public Slider sliderHeight;
        public TMP_Text textWidth;
        public TMP_Text textHeight;

        public Slider sliderCellSize;
        public TMP_Text textCellSize;

        public TMP_Dropdown dropdownFillType;

        public Slider sliderTimer;
        public TMP_Text textTimer;

        public Toggle toggleTimerOn;

        public CanvasGroup[] canvasGroups;

        #endregion

        #region Private Var

        private bool[,] _board;
        private Toggle[,] _boardToggles;
        private Coroutine _coroutineTimer;

        #endregion

        #region Unity Functions

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            SetEnableCanvas();
        }
        
        #endregion

        #region Callbacks for UI

        public void OnSliderWidthChanged()
        {
            textWidth.text = "Width: " + sliderWidth.value;
        }

        public void OnSliderHeightChanged()
        {
            textHeight.text = "Height: " + sliderHeight.value;
        }

        public void OnSliderCellSizeChanged()
        {
            textCellSize.text = "Cell Size: " + sliderCellSize.value;
            gridLayoutBoard.cellSize = new Vector2(sliderCellSize.value, sliderCellSize.value);
        }

        public void OnButtonBuildBoardClicked()
        {
            //todo: performance upgrade Suggestions: Reused old Toggles.
            //clear board
            if (_boardToggles != null)
            {
                foreach (var toggle in _boardToggles)
                {
                    Destroy(toggle.gameObject);
                }
            }

            int width = (int)sliderWidth.value;
            int height = (int)sliderHeight.value;


            _boardToggles = new Toggle[height, width];
            _board = new bool[height, width];


            gridLayoutBoard.constraintCount = width;

            RectTransform rectTransformBoard = gridLayoutBoard.GetComponent<RectTransform>();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    GameObject cell = Instantiate(cellPrefab, rectTransformBoard, false);
                    Toggle toggle = cell.GetComponent<Toggle>();
                    _boardToggles[i, j] = toggle;

                    //need to pass in this because i and j will be the height and width.
                    int y = i;
                    int x = j;
                    //add a listener to know which one was pressed
                    toggle.onValueChanged.AddListener((isOn) => OnCellClicked(y, x, isOn));
                }
            }

            SetEnableCanvas();
        }

        public void OnButtonFillClicked()
        {
            //clear all
            int rowSize = _board.GetLength(0);
            int columnSize = _board.GetLength(1);

            //Iterate though the board
            for (int iRow = 0; iRow < rowSize; iRow++)
            {
                for (int iColumn = 0; iColumn < columnSize; iColumn++)
                {
                    //random true or false
                    _board[iRow, iColumn] = false;
                }
            }

            switch (dropdownFillType.value)
            {
                case 0: //TrueRandom
                    BuildBoard_Randomize();
                    break;
                case 1:
                    BuildBoard_TestRandom();
                    break;
                case 2:
                    BuildBoard_Blinker();
                    break;
                case 3:
                    BuildBoard_Toad();
                    break;
                case 4:
                    BuildBoard_Beacon();
                    break;
                case 5:
                    BuildBoard_Glider();
                    break;
                default:
                    Debug.LogError($"Invalid selection: {dropdownFillType.value}");
                    break;
            }

            DisplayBoard();
        }

        public void OnButtonNextGenerationClicked()
        {
            NextBoard();
            DisplayBoard();
        }


        public void OnSliderTimerChanged()
        {
            //public Slider sliderTimer;    
            //public TMP_Text textTimer;
            textTimer.text = "Auto Next Generation in Seconds:\n" + sliderTimer.value;
        }

        public void OnToggleTimerOnChanged()
        {
            //toggleTimerOn.isOn = toggleTimerOn.isOn;
            if (toggleTimerOn.isOn)
            {
                _coroutineTimer = StartCoroutine(CoTimer());
            }
            else
            {
                StopCoroutine(_coroutineTimer);
                _coroutineTimer = null;
            }

            SetEnableCanvas();
        }

        private void OnCellClicked(int y, int x, bool isOn)
        {
            _board[y, x] = isOn;
        }

        #endregion

        #region Private Functions

        private IEnumerator CoTimer()
        {
            while (true)
            {
                OnButtonNextGenerationClicked();
                yield return new WaitForSeconds(sliderTimer.value);
                // print("WaitAndPrint " + Time.time);
            }
        }

        private void SetEnableCanvas()
        {
            canvasGroups[0].interactable = true;
            canvasGroups[1].interactable = true;
            canvasGroups[2].interactable = true;

            if (_board == null)
            {
                canvasGroups[1].interactable = false;
                canvasGroups[2].interactable = false;
                return;
            }

            if (_coroutineTimer != null)
            {
                canvasGroups[0].interactable = false;
                canvasGroups[1].interactable = false;
            }
        }

        private void DisplayBoard()
        {
            if (_boardToggles == null)
            {
                Debug.LogError("There is no board");
                return;
            }

            //_stringBuilder.Clear();

            int rowSize = _boardToggles.GetLength(0);
            int columnSize = _boardToggles.GetLength(1);

            //iterate through the 2D array board
            for (int iRow = 0; iRow < rowSize; iRow++)
            {
                for (int iColumn = 0; iColumn < columnSize; iColumn++)
                {
                    _boardToggles[iRow, iColumn].SetIsOnWithoutNotify(_board[iRow, iColumn]);
                }
            }
        }

        private void NextBoard()
        {
            if (_board == null)
            {
                Debug.LogError("There is no board");
                return;
            }

            int rowSize = _board.GetLength(0);
            int columnSize = _board.GetLength(1);

            //creating a new board to apply results to the current board
            //everything will default to false when creating a new board
            bool[,] newBoard = new bool[rowSize, columnSize];

            for (int iRow = 0; iRow < rowSize; iRow++)
            {
                for (int iColumn = 0; iColumn < columnSize; iColumn++)
                {
                    // get the amount of alive neighbors here.
                    int aliveNeighbors = GetAliveNeighbors(iRow, iColumn);

                    //Is the current one alive?
                    if (_board[iRow, iColumn] == true)
                    {
                        //Any live cell with two or three live neighbors lives on to the next generation.
                        if (aliveNeighbors == 2 || aliveNeighbors == 3)
                        {
                            newBoard[iRow, iColumn] = true;
                        }

                        //if alive Neighbors is less than 2 or greater than 3. leave as false
                    }
                    else
                    {
                        //Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction
                        if (aliveNeighbors == 3)
                        {
                            newBoard[iRow, iColumn] = true;
                        }

                        //if not 3 then it stays dead.
                    }
                }
            }

            _board = newBoard;
        }

        private int GetAliveNeighbors(int iRow, int iColumn)
        {
            if (_board == null)
            {
                Debug.LogError("There is no board");
                return -1;
            }

            int aliveNeighborCount = 0;
            int rowSize = _board.GetLength(0);
            int columnSize = _board.GetLength(1);

            //Check the Neighbors which is 3x3 around (iRow, iColumn) index

            //start on previous row, and end on the next row
            for (int iCurrentRow = iRow - 1; iCurrentRow <= iRow + 1; iCurrentRow++)
            {
                //Wrapping the row index in case it goes out of bound
                //adding the size again to handle negative numbers.
                int trueRow = (iCurrentRow + rowSize) % rowSize;


                //start on previous column, and end on the next row
                for (int iCurrentColumn = iColumn - 1; iCurrentColumn <= iColumn + 1; iCurrentColumn++)
                {
                    //skip because this is self and not neighbor
                    if (iRow == iCurrentRow && iColumn == iCurrentColumn)
                    {
                        continue;
                    }

                    //Wrapping the column index in case it goes out of bound
                    //adding the size again to handle negative numbers.
                    int trueColumn = (iCurrentColumn + columnSize) % columnSize;

                    //If this cell is alive, increment count.
                    if (_board[trueRow, trueColumn] == true)
                    {
                        aliveNeighborCount++;
                    }
                }
            }

            return aliveNeighborCount;
        }

        #endregion

        #region Private Functions - build boards

        private void BuildBoard_Randomize()
        {
            int rowSize = _board.GetLength(0);
            int columnSize = _board.GetLength(1);

            //Iterate though the board
            for (int iRow = 0; iRow < rowSize; iRow++)
            {
                for (int iColumn = 0; iColumn < columnSize; iColumn++)
                {
                    //random true or false
                    _board[iRow, iColumn] = Random.Range(0, 2) == 1;
                }
            }
        }

        private void BuildBoard_TestRandom()
        {
            //the exact values on the instruction for Random
            _board[0, 0] = false;
            _board[0, 1] = false;
            _board[0, 2] = false;
            _board[0, 3] = true;
            _board[0, 4] = true;
            _board[0, 5] = false;
            _board[0, 6] = true;
            _board[0, 7] = true;

            _board[1, 0] = false;
            _board[1, 1] = false;
            _board[1, 2] = false;
            _board[1, 3] = true;
            _board[1, 4] = true;
            _board[1, 5] = true;
            _board[1, 6] = true;
            _board[1, 7] = false;

            _board[2, 0] = false;
            _board[2, 1] = true;
            _board[2, 2] = true;
            _board[2, 3] = true;
            _board[2, 4] = true;
            _board[2, 5] = false;
            _board[2, 6] = true;
            _board[2, 7] = false;

            _board[3, 0] = false;
            _board[3, 1] = true;
            _board[3, 2] = true;
            _board[3, 3] = false;
            _board[3, 4] = false;
            _board[3, 5] = false;
            _board[3, 6] = true;
            _board[3, 7] = true;

            _board[4, 0] = false;
            _board[4, 1] = false;
            _board[4, 2] = true;
            _board[4, 3] = false;
            _board[4, 4] = true;
            _board[4, 5] = true;
            _board[4, 6] = true;
            _board[4, 7] = true;

            _board[5, 0] = true;
            _board[5, 1] = true;
            _board[5, 2] = true;
            _board[5, 3] = false;
            _board[5, 4] = true;
            _board[5, 5] = true;
            _board[5, 6] = true;
            _board[5, 7] = true;

            _board[6, 0] = true;
            _board[6, 1] = true;
            _board[6, 2] = true;
            _board[6, 3] = true;
            _board[6, 4] = false;
            _board[6, 5] = true;
            _board[6, 6] = true;
            _board[6, 7] = false;

            _board[7, 0] = true;
            _board[7, 1] = false;
            _board[7, 2] = true;
            _board[7, 3] = false;
            _board[7, 4] = false;
            _board[7, 5] = false;
            _board[7, 6] = false;
            _board[7, 7] = true;
        }

        private void BuildBoard_Blinker()
        {
            _board[3, 4] = true;
            _board[4, 4] = true;
            _board[5, 4] = true;
        }

        private void BuildBoard_Toad()
        {
            _board[3, 3] = true;
            _board[3, 4] = true;
            _board[3, 5] = true;

            _board[4, 2] = true;
            _board[4, 3] = true;
            _board[4, 4] = true;
        }

        private void BuildBoard_Beacon()
        {
            _board[1, 5] = true;
            _board[1, 6] = true;

            _board[2, 5] = true;
            _board[2, 6] = true;

            _board[3, 3] = true;
            _board[3, 4] = true;

            _board[4, 3] = true;
            _board[4, 4] = true;
        }

        private void BuildBoard_Glider()
        {
            _board[1, 3] = true;

            _board[2, 1] = true;

            _board[2, 3] = true;

            _board[3, 2] = true;
            _board[3, 3] = true;
        }

        #endregion
    }
}