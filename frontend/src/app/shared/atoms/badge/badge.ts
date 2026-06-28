import { ChangeDetectionStrategy, Component, input } from '@angular/core';

/** ATOM: a small coloured label, used for genre / platform tags. */
@Component({
  selector: 'ngs-badge',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `<span class="badge rounded-pill text-bg-{{ tone() }}"><ng-content /></span>`,
})
export class BadgeComponent {
  readonly tone = input<'primary' | 'secondary' | 'info' | 'light' | 'dark'>('secondary');
}
