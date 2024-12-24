import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pagination-footer',
  templateUrl: './pagination-footer.component.html',
  styleUrls: ['./pagination-footer.component.css']
})

export class PaginationFooterComponent {
  @Input() pageSizeType: number;
  @Input() totalItems: number;
  @Output() pageSizeChange = new EventEmitter<number>();
  @Output() refreshEvent = new EventEmitter<void>();
  @Output() pageChange = new EventEmitter<number>();

  changePageSize(): void {
    this.pageSizeChange.emit(this.pageSizeType);
  }

  refresh(): void {
    this.refreshEvent.emit();
  }

  getDataPage(event: number): void {
    this.pageChange.emit(event);
  }
}