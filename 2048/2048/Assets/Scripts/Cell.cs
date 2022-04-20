using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public int X { get; private set; }

    public int Y { get; private set; }

    // Хранение наминналла плитки
    public int Value { get; private set; }


    public int Points => IsEmpty ? 0 : (int)Mathf.Pow(2, Value);

    public bool IsEmpty => Value == 0;

    public bool HasMerged { get; private set; } // Обединялас ли плитка или нет

    public const int MaxValue = 11;

    // Вывод данных
    [SerializeField]
    private Image image; // Цвет блока
    [SerializeField]
    private TextMeshProUGUI points; // Текст на блоке

    private CellAnimation currentAnimation;

    public void SetValue(int x, int y, int value, bool updateUI = true)
    {
        X = x;
        Y = y;
        Value = value;

        if (updateUI) 
        {
            UpdateCell(); // Отобразить изменения
        }
    }

    public void IncreaseValue() // Метод при обединении 2 ячеек 
    {
        Value++;
        HasMerged = true;
        
        GameControler.Instance.AddPoints(Points);  //Передача очков 

    }

    public void ResetFlags()
    {
        HasMerged = false;
    }

    public void MergeWithCell(Cell otherCell)
    {
        CellAnimationController.Instance.SmoothTransition(this, otherCell, true); // передача значений в CellAnimationController

        otherCell.IncreaseValue();
        SetValue(X, Y, 0);
    }

    public void MoveToCell(Cell target)
    {
        CellAnimationController.Instance.SmoothTransition(this, target, true); // передача значений в CellAnimationController

        target.SetValue(target.X, target.Y, Value, false);
        SetValue(X, Y, 0);

    }


    // Отображаем наминал и изминяем цвет в зависимости от степени

    public void UpdateCell()
    {
        points.text = IsEmpty ? string.Empty : Points.ToString(); // если клетка пустая то пустая строка
        points.color = Value <= 2 ? ColorManager.Instance.PointsDarkColor :
            ColorManager.Instance.PointsLightColor;

        image.color = ColorManager.Instance.CellColors[Value];
    }


    public void SetAnimation(CellAnimation animation)
    {
        currentAnimation = animation;
    }



    public void CancelAnimation()
    {
        if (currentAnimation != null)
            currentAnimation.Destroy();
    }

 
}
