import {
  Component,
  Input,
  Output,
  EventEmitter,
  ChangeDetectionStrategy,
  Injector
} from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
  selector: 'abp-modal-header',
  templateUrl: './abp-modal-header.component.html',
  styleUrls: ['./abp-modal.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AbpModalHeaderComponent extends AppComponentBase {
  @Input() title: string;
  @Input() isShowClose: boolean = true;
  @Input() subTitle: string = '';
  @Output() onCloseClick = new EventEmitter<number>();

  constructor(injector: Injector) {
    super(injector);
  }
}
