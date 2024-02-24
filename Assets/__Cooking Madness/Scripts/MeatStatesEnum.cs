using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public enum MeatStates
{
   raw,girlled,burned
}

public interface IMeatState
{
    void HandleButtonClick(Meat meat);
    void UpdateDisplay(Meat meat);
}

// Concrete state classes
public interface IMeatStateChanger
{
    void ChangeToGrilled();
    void ChangeToBurned();
}
public interface IClockControl
{
    void StopClock();
}
// Context class (Meat)

