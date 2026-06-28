import {
  ChangeDetectionStrategy,
  Component,
  input,
  output,
} from "@angular/core";
import { GAME_GENRES, GameGenre, Publisher } from "@core/models/game.model";

export interface FilterState {
  genre: GameGenre | null;
  publisherId: number | null;
  sortBy: string;
  desc: boolean;
}

/**
 * MOLECULE: genre + publisher + sort controls. Emits the full filter state whenever
 * any control changes so the parent owns a single source of truth.
 */
@Component({
  selector: "ngs-filter-bar",
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: "./filter-bar.html",
})
export class FilterBarComponent {
  protected readonly genres = GAME_GENRES;

  readonly genre = input<GameGenre | null>(null);
  readonly publisherId = input<number | null>(null);
  readonly publishers = input<Publisher[]>([]);
  readonly sortBy = input<string>("title");
  readonly desc = input<boolean>(false);

  readonly filterChange = output<FilterState>();

  protected genreValue = () =>
    this.genre() === null ? "" : String(this.genre());
  protected publisherValue = () =>
    this.publisherId() === null ? "" : String(this.publisherId());
  protected descValue = () => String(this.desc());

  protected onGenre(value: string): void {
    this.emit({ genre: value === "" ? null : (Number(value) as GameGenre) });
  }

  protected onPublisher(value: string): void {
    this.emit({ publisherId: value === "" ? null : Number(value) });
  }

  protected onSort(value: string): void {
    this.emit({ sortBy: value });
  }

  protected onDesc(value: string): void {
    this.emit({ desc: value === "true" });
  }

  private emit(patch: Partial<FilterState>): void {
    this.filterChange.emit({
      genre: this.genre(),
      publisherId: this.publisherId(),
      sortBy: this.sortBy(),
      desc: this.desc(),
      ...patch,
    });
  }
}
