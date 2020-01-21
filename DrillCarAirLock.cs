using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;

using System;


namespace IngameScript
{
    partial class DrillCarAirLock : MyGridProgram
    {
        public DrillCarAirLock()
        {
            // Configure this program to run the Main method every 100 update ticks
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            return;
        }
        public void Main()
        {
            string outputText;
            var airVent = GridTerminalSystem.GetBlockWithName("AirLockVent") as IMyAirVent;
            var airLockOutterDoor = GridTerminalSystem.GetBlockWithName("AirlockOutterDoor") as IMyAirtightSlideDoor;
            var airLockInnderDoorCockpit = GridTerminalSystem.GetBlockWithName("AirlockInnerDoorCockpit") as IMyAirtightSlideDoor;
            var airLockInnderDoorProcessing = GridTerminalSystem.GetBlockWithName("AirlockInnerDoorProcessing") as IMyAirtightSlideDoor;
            IMyBlockGroup lcdPanelsGroup = GridTerminalSystem.GetBlockGroupWithName("AirLockPanels");
            var oxygenLevel = airVent.GetOxygenLevel();

            if (lcdPanelsGroup == null)
            {
                Echo("Lcd Panels not found");
                return;
            }

            List<IMyTextPanel> textPanels = new List<IMyTextPanel>();
            lcdPanelsGroup.GetBlocksOfType(textPanels);
            foreach (var textPanel in textPanels)
            {
                outputText = airVent.CustomName + "'s Pressure: " + String.Format("{0:P2}", oxygenLevel);
                textPanel.WriteText(
                    outputText + "\n" +
                    "Pressure Unedited: " + oxygenLevel + "\n" +
                    "Inner Door Cockpit: " + airLockInnderDoorCockpit.Status + "\n" +
                    "Inner Door Processing Room: " + airLockInnderDoorCockpit.Status + "\n" +
                    "Outter Door Open: " + airLockOutterDoor.Status + "\n" +
                    "Vent Status: " + airVent.Status);
            }
            if (airVent.Depressurize)
            {
                airLockInnderDoorCockpit.CloseDoor();
                airLockInnderDoorProcessing.CloseDoor();
                airLockOutterDoor.Enabled = true;
                if (oxygenLevel <= 0.0)
                {
                    airLockInnderDoorCockpit.Enabled = false;
                    airLockInnderDoorProcessing.Enabled = false;
                    airLockOutterDoor.OpenDoor();
                }
            }
            else if (airVent.PressurizationEnabled)
            {
                airLockOutterDoor.CloseDoor();
                airLockInnderDoorCockpit.Enabled = true;
                airLockInnderDoorProcessing.Enabled = true;
                if (oxygenLevel >= 1.0f)
                {
                    airLockOutterDoor.Enabled = false;
                    airLockInnderDoorCockpit.OpenDoor();
                    airLockInnderDoorProcessing.OpenDoor();
                }
            }
        }
    }
}