import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';

/**
 * ATOM: a labelled text input wrapper. Two-way value via model-less input/output so
 * it works both with template bindings and reactive forms (used standalone here).
 */
@Component({
  selector: 'ngs-text-input',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="mb-0">
      @if (label()) {
        <label class="form-label" [attr.for]="inputId()">{{ label() }}</label>
      }
      <input
        [id]="inputId()"
        [type]="type()"
        [class]="'form-control ' + (invalid() ? 'is-invalid' : '')"
        [placeholder]="placeholder()"
        [value]="value()"
        (input)="valueChange.emit($any($event.target).value)"
      />
    </div>
  `,
})
export class TextInputComponent {
  readonly label = input('');
  readonly placeholder = input('');
  readonly type = input<'text' | 'number' | 'date'>('text');
  readonly value = input<string | number>('');
  readonly inputId = input('ngs-input');
  readonly invalid = input(false);
  readonly valueChange = output<string>();
}
