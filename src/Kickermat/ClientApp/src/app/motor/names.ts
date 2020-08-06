import { OperatingMode, OperatingState, NmtState, Bar } from "@api/api.model";

export function opModeToString(mode: OperatingMode): string {
  switch (mode) {
    case OperatingMode.ProfilePositionMode:
      return "Profile Position Mode";
    case OperatingMode.ProfileVelocityMode:
      return "Profile Velocity Mode";
    case OperatingMode.HomingMode:
      return "Homing Mode";
    case OperatingMode.OscillatorMode:
      return "Oscillator Mode";
    case OperatingMode.GearingMode:
      return "Gearing Mode";
    case OperatingMode.JogMode:
      return "Jog Mode";
  }

  // omit 'default' case to provoke compile time error when new operating mode
  // is added while making sure text is return for unknown value at runtime
  return "unknown operating mode";
}

export function opStateToString(state: OperatingState): string {
  switch (state) {
    case OperatingState.Fault:
      return "Fault";
    case OperatingState.FaultReactionActive:
      return "Fault Reaction active";
    case OperatingState.NotReadyToSwitchOn:
      return "Not ready to switch on";
    case OperatingState.OperationEnable:
      return "Operation enable";
    case OperatingState.QuickStopActive:
      return "Quick-Stop active";
    case OperatingState.ReadyToSwitchOn:
      return "Ready to switch on";
    case OperatingState.SwitchedOn:
      return "Switched On";
    case OperatingState.SwitchOnDisabled:
      return "Switch on disabled";
  }
}

export function nmtStateToString(state: NmtState): string {
  switch (state) {
    case NmtState.Initialization:
      return "Initialization";
    case NmtState.PreOperational:
      return "Pre-Operational";
    case NmtState.Operational:
      return "Operational";
    case NmtState.Stopped:
      return "Stopped";
  }
}

export function barToString(bar: Bar): string {
  switch (bar) {
    case Bar.Keeper:
      return "Keeper";
    case Bar.Defense:
      return "Defense";
    case Bar.Midfield:
      return "Midfield";
    case Bar.Striker:
      return "Striker";
  }
}
