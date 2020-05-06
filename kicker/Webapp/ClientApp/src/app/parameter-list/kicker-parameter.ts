import { OnInit, Input } from '@angular/core';
import { KickerParameter, KickerComponent } from '@api/api.model';
import { ApiService } from '@api/api.service';
import { NzMessageService } from 'ng-zorro-antd';
import { HttpErrorResponse } from '@angular/common/http';

export abstract class KickerParameterComponent<T extends KickerParameter> implements OnInit {
  @Input('parameter') public parameter: T | undefined;
  @Input('component') public component: KickerComponent | undefined;

  public isUpdating = false;
  
  constructor(private api: ApiService, private message: NzMessageService) { }

  protected abstract updateModel(paramValue: any): void;

  ngOnInit() {
    if (this.parameter) {
      this.updateModel(this.parameter.value);
    }
  }

  public setParameter(updatedValue: any) {
    if (this.component && this.parameter) {
      this.isUpdating = true;

      this.api.setParameter(this.component, this.parameter.name, updatedValue).subscribe(
        () => {
          this.isUpdating = false;
          if (this.parameter) {
            this.parameter.value = updatedValue;
            let msg = this.parameter.name + ' was set to ' + this.parameter.value;
            this.message.success(msg);
          }
        },
        (error: HttpErrorResponse) => {
          this.isUpdating = false;
          console.log(error);
          if (this.parameter) {
            this.updateModel(this.parameter.value);
            this.message.error(error.error);
          }
        }
      );
    }
  }
}
