import { Component, OnInit, Input } from '@angular/core';
import { ApiService } from '@api/api.service';
import { KickerComponent, KickerParameter } from '../../api/api.model';

@Component({
  selector: 'app-parameter-list',
  templateUrl: './parameter-list.component.html',
  styleUrls: ['./parameter-list.component.scss']
})
export class ParameterListComponent implements OnInit {
  @Input('component') private component: KickerComponent | undefined;
  
  public parameters: KickerParameter[] = [];

  constructor(private api: ApiService) { }

  ngOnInit() {
    if (this.component) {
      this.api.getParameters(this.component).subscribe(
        (parameters: KickerParameter[]) => {
          this.parameters = parameters;
        },
        error => {
          console.log('Failed to load parameters for %s: %o', this.component, error);
        }
      );
    }
  }

  public isNumberParameter(parameter: KickerParameter | undefined): boolean {
    if (!parameter) {
      return false;
    }

    return 'min' in parameter && 'max' in parameter && 'step' in parameter;
  }

  public isColorRangeParameter(parameter: KickerParameter | undefined): boolean {
    if (!parameter) {
      return false;
    }

    return typeof parameter.value === 'object' && 'upper' in <any>parameter.value && 'lower' in <any>parameter.value;
  }
}
