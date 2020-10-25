import { KickerParameter } from '@api/model';

const isNumberParam = (parameter: KickerParameter<any>): boolean => {
  return 'min' in parameter && 'max' in parameter && 'step' in parameter;
};

const isColorRangeParam = (parameter: KickerParameter<any>): boolean => {
  return parameter.value !== null
    && typeof parameter.value === 'object'
    && 'upper' in (parameter.value as any)
    && 'lower' in (parameter.value as any);
};

const isBooleanParam = (parameter: KickerParameter<any>): boolean => {
  return typeof parameter.value === 'boolean';
};

const isEnumParam = (parameter: KickerParameter<any>): boolean => {
  return typeof parameter.value === 'number' && 'options' in parameter;
};

const util = {
  isNumberParam,
  isColorRangeParam,
  isBooleanParam,
  isEnumParam,
};

export default util;
