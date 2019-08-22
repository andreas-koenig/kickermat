import { Component, OnInit, Input } from '@angular/core';
import { KickerParameter, KickerComponent } from '../../../api/api.model';
import { ApiService } from '@api/api.service';
import { NzMessageService } from 'ng-zorro-antd';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-kicker-parameter',
  templateUrl: './kicker-parameter.component.html',
  styleUrls: ['./kicker-parameter.component.scss']
})
export class KickerParameterComponent implements OnInit {
  @Input('parameter') public parameter: KickerParameter | undefined;
  @Input('component') public component: KickerComponent | undefined;

  public isUpdating = false;
  public value: any;
  
  constructor(private api: ApiService, private message: NzMessageService ) { }

  ngOnInit() {
    if (this.parameter) {
      this.value = this.parameter.value;
    }
  }

  public isNumberParameter(parameter: KickerParameter): boolean {
    return 'min' in parameter && 'max' in parameter && 'step' in parameter;
  }

  public setParameter() {
    if (this.component && this.parameter) {
      this.isUpdating = true;

      this.api.setParameter(this.component, this.parameter.name, this.value).subscribe(
        () => {
          this.isUpdating = false;
          if (this.parameter) {
            this.parameter.value = this.value;
            let msg = this.parameter.name + ' was set to ' + this.parameter.value;
            this.message.success(msg);
          }
        },
        (error: HttpErrorResponse) => {
          this.isUpdating = false;
          console.log(error);
          if (this.parameter) {
            this.value = this.parameter.value;
            this.message.error(error.error);
          }
        }
      );
    }
  }
}
