import { ChangeDetectionStrategy, Component, input } from "@angular/core";

/**
 * A generic page layout to include all shell components
 */
@Component({
  selector: "ngs-page-shell",
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: "./page-shell.html",
})
export class PageShellComponent {
  readonly heading = input.required<string>();
  readonly subheading = input("");
}
