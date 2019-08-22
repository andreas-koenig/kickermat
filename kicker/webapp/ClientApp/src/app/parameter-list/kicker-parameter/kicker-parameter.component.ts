import { Component, OnInit, Input } from '@angular/core';
import { KickerParameter, KickerComponent } from '../../../api/api.model';
import { ApiService } from '@api/api.service';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'app-kicker-parameter',
  templateUrl: './kicker-parameter.component.html',
  styleUrls: ['./kicker-parameter.component.scss']
})
export class KickerParameterComponent implements OnInit {
  @Input('parameter') public parameter: KickerParameter | undefined;
  @Input('component') public component: KickerComponent | undefined;
  public isUpdating = false;
  
  constructor(private api: ApiService, private message: NzMessageService ) { }

  ngOnInit() {
  }

  public isNumberParameter(parameter: KickerParameter): boolean {
    return 'min' in parameter && 'max' in parameter && 'step' in parameter;
  }

  public setParameter() {
    if (this.component && this.parameter) {
      this.isUpdating = true;

      this.api.setParameter(this.component, this.parameter).subscribe(
        () => {
          this.isUpdating = false;
          if (this.parameter) {
            let msg = this.parameter.name + ' was set to ' + this.parameter.value;
            this.message.success(msg);
          }
        },
        error => {
          this.isUpdating = false;
          console.log(error);
          if (this.parameter) {
            this.message.error(error.msg);
          }
        }
      );
    }
  }
}
