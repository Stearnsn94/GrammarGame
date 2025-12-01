using UnityEngine;
public enum difficultiy
{
    EASY,
    MEDIUM,
    HARD
}

public class diffculty_manager : MonoBehaviour
{
    private difficultiy current_diff;
    difficultiy get_difficulty() { return current_diff; }

    public void set_difficulty(difficultiy _diff)
    {
        current_diff = _diff;
    }
}