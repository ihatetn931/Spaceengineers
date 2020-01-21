using Sandbox.ModAPI.Ingame;
using System.Collections.Generic;

namespace IngameScript
{
    partial class DrillCarDrill : MyGridProgram
    {
        public DrillCarDrill()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
        }
        public void Main()
        {
            var panel = GridTerminalSystem.GetBlockWithName("DrillInfo") as IMyTextPanel;
            var centerRotor = GridTerminalSystem.GetBlockWithName("Center Rotor") as IMyMotorAdvancedStator;
            IMyBlockGroup group = GridTerminalSystem.GetBlockGroupWithName("Drill Pistons");
            IMyBlockGroup drillGroup = GridTerminalSystem.GetBlockGroupWithName("Drills");
            IMyBlockGroup warnLightGroup = GridTerminalSystem.GetBlockGroupWithName("RotatingLights");
            if (group == null)
            {
                Echo("Group not found");
                return;
            }
            if (panel == null)
            {
                Echo("Panel not found");
                return;
            }
            if (drillGroup == null)
            {
                Echo("Drills not found");
                return;
            }
            if (warnLightGroup == null)
            {
                Echo("Warn Lights not found");
                return;
            }
            //Echo($"{group.Name}:");
            List<IMyPistonBase> blocks = new List<IMyPistonBase>();
            List<IMyShipDrill> drills = new List<IMyShipDrill>();
            List<IMyLightingBlock> rLights = new List<IMyLightingBlock>();
            group.GetBlocksOfType(blocks);
            drillGroup.GetBlocksOfType(drills);
            warnLightGroup.GetBlocksOfType(rLights);
            foreach (var block in blocks)
            {
                foreach (var drill in drills)
                {
                    foreach (var rlight in rLights)
                    {
                        //Echo($"- {block.CustomName}");
                        panel.WriteText(
                            "Piston Range: " + block.CurrentPosition + "\n" +
                            "Piston Status: " + block.Status + "\n" +
                            "Piston Speed: " + block.Velocity + "\n" +
                            "Drills Enabled: " + drill.Enabled + "\n" +
                            "Rotor Locked: " + centerRotor.RotorLock);

                        Echo(block.MaxLimit.ToString());
                        if(drill.Enabled)
                        {
                            rlight.Enabled = true;
                        }
                        else
                        {
                            rlight.Enabled = false;
                        }
                        if (block.CurrentPosition == block.MaxLimit)
                        {
                            block.Velocity = 0.5f;
                            block.Retract();
                        }
                        else 
                        {
                            if (block.CurrentPosition == block.MinLimit)
                            {
                                drill.Enabled = false;
                                centerRotor.RotorLock = true;
                                block.Velocity = -0.0105f;
                            }
                        }
                    }
                }
            }
        }
    }
}