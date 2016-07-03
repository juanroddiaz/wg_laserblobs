using UnityEngine;
using System.Collections;

public class MainScenarioLogic : MonoBehaviour
{
    [SerializeField]
    private LaserBeamGroupLogic _laserGroupLogic;

    public void Init()
    {
        _laserGroupLogic.Init();
    }

    public void UpdateLogic()
    {
        _laserGroupLogic.UpdateLogic();
    }
}
