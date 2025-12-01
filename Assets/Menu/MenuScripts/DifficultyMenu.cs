using System;
using UnityEngine;

public class DifficultyMenu : MonoBehaviour
{
    [SerializeField]diffculty_manager diff_mang;
    public void choose_difficulty(difficultiy _diff)
    {
          if(diff_mang != null)
          {
            diff_mang.set_difficulty(_diff);
          }
    }
}

