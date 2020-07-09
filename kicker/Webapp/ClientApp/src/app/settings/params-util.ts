import { KickerParameter } from '@api/api.model';

const isNumberParam = (parameter: KickerParameter<any>): boolean => {
  return "min" in parameter && "max" in parameter && "step" in parameter;
}

const isColorRangeParam = (parameter: KickerParameter<any>): boolean => {
  return parameter.value !== null
    && typeof parameter.value === "object"
    && "upper" in <any>parameter.value
    && "lower" in <any>parameter.value;
}

const isBooleanParam = (parameter: KickerParameter<any>): boolean => {
  return typeof parameter.value === "boolean";
}

const isEnumParam = (parameter: KickerParameter<any>): boolean => {
  return typeof parameter.value === "number" && "options" in parameter;
}

const util = {
  isNumberParam,
  isColorRangeParam,
  isBooleanParam,
  isEnumParam,
}

export default util;
