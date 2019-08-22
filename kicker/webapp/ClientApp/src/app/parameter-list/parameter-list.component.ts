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
          console.log(error);
        }
      );
    }
  }

}
