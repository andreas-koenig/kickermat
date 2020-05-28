using System;
using System.Collections.Generic;
using System.Text;

namespace MotorController
{
    public class Diagnostics
    {
        public static Motor[] Collect()
        {
            return new Motor[] {
            new Motor("Telemecanique", Function.Shift, Bar.Keeper, 1, NmtState.Operational,
                OperatingState.NotReadyToSwitchOn, OperatingMode.ProfilePositionMode, null),
            new Motor("Telemecanique", Function.Shift, Bar.Defense, 2, NmtState.Initialization,
                OperatingState.SwitchedOn, OperatingMode.ProfilePositionMode, null),
            new Motor("Telemecanique", Function.Shift, Bar.Midfield, 3, NmtState.PreOperational,
                OperatingState.OperationEnable, OperatingMode.ProfilePositionMode, null),
            new Motor("Telemecanique", Function.Shift, Bar.Striker, 4, NmtState.Operational,
                OperatingState.QuickStopActive, OperatingMode.ProfilePositionMode, null),
            new Motor("Faulhaber", Function.Rotation, Bar.Keeper, 10, NmtState.Operational,
                OperatingState.SwitchOnDisabled, OperatingMode.ProfileVelocityMode, null),
            new Motor("Faulhaber", Function.Rotation, Bar.Defense, 11, NmtState.Initialization,
                OperatingState.SwitchedOn, OperatingMode.ProfileVelocityMode, null),
            new Motor("Faulhaber", Function.Rotation, Bar.Midfield, 12, NmtState.Stopped,
                OperatingState.Fault, OperatingMode.ProfileVelocityMode, "Overheated"),
            new Motor("Faulhaber", Function.Rotation, Bar.Striker, 13, NmtState.Operational,
                OperatingState.FaultReactionActive, OperatingMode.ProfileVelocityMode, null),
            };
        }
    }

    public class Motor
    {
        public Motor() { }

        public Motor(string model, Function function, Bar bar, byte canId, NmtState nmtState,
            OperatingState operatingState, OperatingMode operatingMode, string error)
        {
            Model = model;
            Function = function;
            Bar = bar;
            CanId = canId;
            NmtState = nmtState;
            OperatingState = operatingState;
            OperatingMode = operatingMode;
            Error = error;
        }

        public string Model { get; set; }
        public Function Function { get; set; }
        public Bar Bar { get; set; }
        public byte CanId { get; set; }
        public NmtState NmtState { get; set; }
        public OperatingState OperatingState { get; set; }
        public OperatingMode OperatingMode { get; set; }
        public string Error { get; set; }
    }

    public enum Bar
    {
        Keeper = 0,
        Defense = 1,
        Midfield = 2,
        Striker = 3
    }

    public enum Function
    {
        Rotation = 0,
        Shift = 1
    }

    public enum NmtState
    {
        Initialization = 0,
        PreOperational = 1,
        Operational = 2,
        Stopped = 3
    }

    public enum OperatingState
    {
        NotReadyToSwitchOn = 0,
        SwitchOnDisabled = 1,
        ReadyToSwitchOn = 2,
        SwitchedOn = 3,
        OperationEnable = 4,
        QuickStopActive = 5,
        FaultReactionActive = 6,
        Fault = 7
    }

    public enum OperatingMode
    {
        ProfilePositionMode = 0, // Punkt zu Punkt
        ProfileVelocityMode = 1, // Geschwindigkeitsprofil
        HomingMode = 2,          // Referenzierung
        OscillatorMode = 3,      // Drehzahlregelung
        GearingMode = 4,         // Elektronisches Getriebe
        JogMode = 5              // Manuellfahrt
    }
}
