using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Field : MonoBehaviour
{

    public static Field Instance;

    [Header("Flild Properties")]
    public float CellSize;
    public float Spacing;
    public int FieldSize;
    public int InitCellsCount;
    public int Algoritm; // алгоритм

    //public Algoritm1 alg;// передача с файла
    public int Allgo;

    [Space(10)]
    [SerializeField]
    private Cell cellPref;
    [SerializeField]
    private RectTransform rt;

    private Cell[,] field;

    private Cell[,] newField;

    private bool anyCellMoved;

    private Cell cellToMerge;

    private int cellPlitOb;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        Allgo = DataHolder._levelStart;
    }


    private void Start()
    {
        SwipeDetection.SwipeEvent += OnInput;
    }


    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
            OnInput(Vector2.left);
        if (Input.GetKeyDown(KeyCode.D))
            OnInput(Vector2.right);
        if (Input.GetKeyDown(KeyCode.W))
            OnInput(Vector2.up);
        if (Input.GetKeyDown(KeyCode.S))
            OnInput(Vector2.down);
#endif
        //Algoritm = allgo1;
        
        //Algoritm = allgo1;
        //Debug.Log(Allgo);
        //Algoritm = allgo1; 
        Algoritm1();
        Algoritm2();
        Algoritm3();

        Ai1();
        Ai2();
        //Debug.Log(field[0, 0].Value); 
        //Debug.Log(Allgo);
        //Debug.Log(field[0, 0].IsEmpty); //- true/false
    }

    //-----------------------Algoritm----------------------------
    private void Algoritm1()
    {

        if (Allgo == 1)
        {
            //yield return new WaitForSeconds(5);
            //bool lose = true;
            for (int x = 0; x <= 4; x++)
            {
                for (int y = 0; y <= 4; y++)
                {
                    if (field[x, y].IsEmpty)
                    {
                        OnInput(Vector2.left);
                        OnInput(Vector2.down);
                    }
                    if (!field[x, y].HasMerged)
                    {
                        OnInput(Vector2.right);
                        OnInput(Vector2.up);
                    }
                }
            }
        }

    }

    private void Algoritm2()
    {
        if (Allgo == 2)
        {
            OnInput(Vector2.left);
            OnInput(Vector2.up);
            OnInput(Vector2.right);
            OnInput(Vector2.down);
        }
    }
    int klon = 1;
    private void Algoritm3()
    {
       
        if (Allgo == 3)
        {
            if (klon == 1)
            {
                field[0, 0].SetValue(0, 0, 10);
                field[0, 1].SetValue(0, 1, 10);
                klon = 0;
            }
            OnInput(Vector2.left);
            OnInput(Vector2.up);
            OnInput(Vector2.right);
            OnInput(Vector2.down);
        }
    }

    //-----------------------------------------------------------

    //--------------------------Ai-------------------------------
    private void Ai1()
    {

        if (Allgo == 4)
        {
            if (anyCellMoved)
            {
                OnInput(Vector2.down);
                if (cellToMerge == null)
                {
                    OnInput(Vector2.left);
                }
            }
            else { OnInput(Vector2.right); }
        }
    }

    private void Ai2()
    {
        if (Allgo == 5)
        {
            for (int i = 0; i < 11; i++)
            {
                if (cellPlitOb == i)
                {

                }
            }
        }
    }
    //-----------------------------------------------------------



    private void OnInput(Vector2 direction) //совершение хода игрока свайп
    {
        if (!GameControler.GameStarted)
            return;

        anyCellMoved = false;
        ResetCellsFlags();

        Move(direction);

        if (anyCellMoved)// если хоть одна плитка изменилось генерируем новую
        {
            GenerateRandomCell();
            CheckGameResult();
        }
    }

    private void Move(Vector2 direction)// логика совершения хода
    {
        int startXY = direction.x > 0 || direction.y < 0 ? FieldSize - 1 : 0;
        int dir = direction.x != 0 ? (int)direction.x : -(int)direction.y;

        for (int i = 0; i < FieldSize; i++)
        {
            for (int k = startXY; k >= 0 && k < FieldSize; k -= dir)
            {
                var cell = direction.x != 0 ? field[k, i] : field[i, k];

                if (cell.IsEmpty)
                    continue;

                cellToMerge = FindCellToMerge(cell, direction);
                if (cellToMerge != null)
                {
                    cell.MergeWithCell(cellToMerge);
                    anyCellMoved = true;
                    continue;
                }
                var emptyCell = FindEmptyCell(cell, direction);
                if (emptyCell != null)
                {
                    cell.MoveToCell(emptyCell);
                    anyCellMoved = true;

                    continue;
                }

            }
        }
    }


    private Cell FindCellToMerge(Cell cell, Vector2 direction)// нахождение плитки с которой можно обединиться
    {
        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY;                            // проверка плиток
            x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;     
            x += (int)direction.x, y -= (int)direction.y)          
        {
            if (field[x, y].IsEmpty) // если плитка пустая то продолжаем
                continue;
            cellPlitOb = cell.Value;
            if (field[x, y].Value == cell.Value && !field[x, y].HasMerged) // если плитка не пустая, такого же наминала что и обединяемая, обединялась ли в єтом ходу
            {
                //Debug.Log(cellPlitOb);
                return field[x, y]; // возврашяем эту плитку
            }
            break;
        }

        return null; // обединиться с неским
    }


    private Cell FindEmptyCell(Cell cell, Vector2 direction) //поис пустой плитки
    {
        Cell emptyCell = null;

        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY;                            // проверка плиток
            x >= 0 && x < FieldSize && y >= 0 && y < FieldSize;
            x += (int)direction.x, y -= (int)direction.y)
        {
            if (field[x, y].IsEmpty)
                emptyCell = field[x, y];
            else
                break;
        }

        return emptyCell;
    }


    private void CheckGameResult() //проверка на победу проигрыш
    {
        bool lose = true;

        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                if (lose &&
                    field[x, y].IsEmpty ||
                    FindCellToMerge(field[x, y], Vector2.left) ||
                    FindCellToMerge(field[x, y], Vector2.right) ||
                    FindCellToMerge(field[x, y], Vector2.up) ||
                    FindCellToMerge(field[x, y], Vector2.down))
                {
                    lose = false;
                }
            }
        }

        if (lose) 
        {
            GameControler.Instance.Lose();
        }
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    GenerateField();
    //}

    // Создание поля 
    private void CreateField()
    {
        field = new Cell[FieldSize, FieldSize];

        float fieldWidth = FieldSize * (CellSize + Spacing) + Spacing;
        rt.sizeDelta = new Vector2(fieldWidth, fieldWidth);

        float startX = -(fieldWidth / 2) + (CellSize / 2) + Spacing;
        float startY = (fieldWidth / 2) - (CellSize / 2) - Spacing;

        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                var cell = Instantiate(cellPref, transform, false);
                var position = new Vector2(startX + (x * (CellSize + Spacing)), startY - (y * (CellSize + Spacing)));
                cell.transform.localPosition = position;

                field[x, y] = cell;

                cell.SetValue(x, y, 0);
            }
        }
    }

    public void GenerateField() // Очистка поля
    {
        if (field == null)
            CreateField();

        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                field[x, y].SetValue(x, y, 0);

        for (int i = 0; i < InitCellsCount; i++)
            GenerateRandomCell();


        //2048
        //field[0, 0].SetValue(0, 0, 10);
        //field[0, 1].SetValue(0, 1, 10);
    }


    private void GenerateRandomCell() //Создание начальных плиток
    {
        var emptyCells = new List<Cell>();

        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                if (field[x, y].IsEmpty)
                    emptyCells.Add(field[x, y]);

        if (emptyCells.Count == 0)
            throw new System.Exception("There is no any empty cell!");

        int value = Random.Range(0, 10) == 0 ? 2 : 1;

        var cell = emptyCells[Random.Range(0, emptyCells.Count)];
        cell.SetValue(cell.X, cell.Y, value, false);

        CellAnimationController.Instance.SmoothAppear(cell);
    }


    private void ResetCellsFlags() // обнуление Хеч
    {
        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                field[x, y].ResetFlags();
    }
}
