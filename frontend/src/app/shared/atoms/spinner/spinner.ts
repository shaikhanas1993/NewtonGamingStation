import { ChangeDetectionStrategy, Component, input } from '@angular/core';

/** ATOM: a loading spinner. */
@Component({
  selector: 'ngs-spinner',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="d-flex justify-content-center align-items-center py-5" role="status">
      <div class="spinner-border text-primary" aria-hidden="true"></div>
      <span class="ms-2">{{ label() }}</span>
    </div>
  `,
})
export class SpinnerComponent {
  readonly label = input('Loading…');
}
