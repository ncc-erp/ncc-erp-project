import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
    selector: '[PrjResizeContent]'
})
export class ResizeContentDirective {

    @Input() collapseLine: number = 1;

    constructor(private el: ElementRef<HTMLElement>) {

    }
    ngOnInit(): void {
        this.el.nativeElement.style.webkitBoxOrient = 'vertical'
    }

    @HostListener('click', ['$event'])
    handleKeyDown(event: KeyboardEvent) {
        this.el.nativeElement.classList.toggle(`max-line-content-${this.collapseLine}`);
    }
}
