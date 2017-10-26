/// <summary>
/// Author: Dante Nardo
/// Last Modified: 10/25/2017
/// Purpose: Holds the data for each cell, most importantly its type.
/// </summary>
public class Cell
{
    #region Cell Members
    public enum LevelType
    {
        Tree,
        Rock,
        None
    }
    private LevelType m_type;
    public LevelType Type
    {
        get { return m_type; }
        set
        {
            m_type = value;
            if (m_type == LevelType.Tree ||
                m_type == LevelType.Rock)
                Obstacle = true;
        }
    }
    public bool Obstacle { get; set; }

    public int X { get; set; }
    public int Y { get; set; }
    #endregion

    #region Cell Methods
    public Cell(int y, int x)
    {
        Type = LevelType.None;
        Obstacle = false;
        Y = y;
        X = x;
    }
    #endregion
}
