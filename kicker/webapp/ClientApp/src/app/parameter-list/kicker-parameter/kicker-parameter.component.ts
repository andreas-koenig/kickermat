import { Component, OnInit, Input } from '@angular/core';
import { KickerParameter, KickerDataType } from '../../../api/api.model';

@Component({
  selector: 'app-kicker-parameter',
  templateUrl: './kicker-parameter.component.html',
  styleUrls: ['./kicker-parameter.component.scss']
})
export class KickerParameterComponent implements OnInit {
  @Input('parameter') public parameter: KickerParameter | undefined;
  public kickerDataTypeEnum = KickerDataType;

  constructor(/*private api: ApiService*/) { }

  ngOnInit() {
  }

}
