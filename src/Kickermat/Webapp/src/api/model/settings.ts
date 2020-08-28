import { Entity, Named } from './base';

// Settings
export interface Settings extends Entity, Named {
  parameters: KickerParameter<any>[];
}

// Parameters
export interface KickerParameter<T> {
  name: string;
  description: string;
  value: T;
  defaultValue: T;
}

export interface NumberParameter extends KickerParameter<number> {
  min: number;
  max: number;
  step: number;
}

export interface ColorRangeParameter extends KickerParameter<ColorRange> {}

export interface ColorRange {
  lower: HsvColor;
  upper: HsvColor;
}

export interface HsvColor {
  hue: number; // [0, 360]
  saturation: number; // [0, 100]
  value: number; // [0, 100]
}

export interface BooleanParameter extends KickerParameter<boolean> {}

export interface EnumParameter extends KickerParameter<number> {
  options: { key: number, value: string }[];
}

// Api request/response types
export interface UpdateSettingsResponse<T extends KickerParameter<T>> {
  message: string;
  value: T;
}

export interface ParameterUpdate {
  settings: string;
  parameter: string;
  value: any;
}
