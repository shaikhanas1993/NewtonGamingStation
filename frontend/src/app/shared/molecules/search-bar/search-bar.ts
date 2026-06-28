import {
  ChangeDetectionStrategy,
  Component,
  input,
  output,
} from "@angular/core";
import { ButtonComponent } from "@shared/atoms/button/button";

/**
 * a search field + button composed from atoms
 */
@Component({
  selector: "ngs-search-bar",
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ButtonComponent],
  templateUrl: "./search-bar.html",
})
export class SearchBarComponent {
  readonly term = input("");
  /** Emitted on every keystroke so the parent can live-search. */
  readonly termChange = output<string>();
  /** Emitted on explicit search (button / enter). */
  readonly search = output<string>();

  protected onInput(value: string): void {
    this.termChange.emit(value);
  }
}
