import {
  ChangeDetectionStrategy,
  Component,
  input,
  output,
} from "@angular/core";
import { RouterLink } from "@angular/router";
import { Game } from "@core/models/game.model";
import { GameCardComponent } from "@shared/molecules/game-card/game-card";
import { PaginationComponent } from "@shared/molecules/pagination/pagination";
import { SpinnerComponent } from "@shared/atoms/spinner/spinner";

/**
 * Container for  game cards + pagination
 *
 */
@Component({
  selector: "ngs-game-grid",
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    GameCardComponent,
    PaginationComponent,
    SpinnerComponent,
    RouterLink,
  ],
  templateUrl: "./game-grid.html",
})
export class GameGridComponent {
  readonly games = input<Game[]>([]);
  readonly loading = input(false);
  readonly page = input(1);
  readonly pageSize = input(12);
  readonly totalItems = input(0);

  readonly edit = output<number>();
  readonly remove = output<Game>();
  readonly pageChange = output<number>();
}
