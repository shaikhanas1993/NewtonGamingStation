import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';

type Variant = 'primary' | 'secondary' | 'success' | 'danger' | 'outline-secondary' | 'outline-primary';
type Size = 'sm' | 'md' | 'lg';

/**
 * ATOM: a styled button. The smallest reusable interactive element. It owns nothing
 * but its own appearance and emits a click event.
 */
@Component({
  selector: 'ngs-button',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <button
      [type]="type()"
      [class]="cssClass()"
      [disabled]="disabled()"
      (click)="clicked.emit($event)"
    >
      <ng-content />
    </button>
  `,
})
export class ButtonComponent {
  readonly variant = input<Variant>('primary');
  readonly type = input<'button' | 'submit'>('button');
  readonly size = input<Size>('md');
  readonly disabled = input(false);
  readonly clicked = output<MouseEvent>();

  protected readonly cssClass = computed(() => {
    const size = this.size() === 'md' ? '' : `btn-${this.size()}`;
    return ['btn', `btn-${this.variant()}`, size].filter(Boolean).join(' ');
  });
}
