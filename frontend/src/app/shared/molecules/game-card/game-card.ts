import {
  ChangeDetectionStrategy,
  Component,
  input,
  output,
} from "@angular/core";
import { CurrencyPipe, DatePipe } from "@angular/common";
import { Game } from "@core/models/game.model";
import { BadgeComponent } from "@shared/atoms/badge/badge";
import { ButtonComponent } from "@shared/atoms/button/button";

/**
 * MOLECULE: presents a single game and exposes edit / delete intents. It is purely
 * presentational — it performs no data access of its own.
 */
@Component({
  selector: "ngs-game-card",
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CurrencyPipe, DatePipe, BadgeComponent, ButtonComponent],
  templateUrl: "./game-card.html",
})
export class GameCardComponent {
  readonly game = input.required<Game>();
  readonly edit = output<number>();
  readonly remove = output<Game>();
}
