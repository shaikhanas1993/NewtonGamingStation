import {
  ChangeDetectionStrategy,
  Component,
  inject,
  input,
  signal,
} from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { GameService } from "@core/services/game.service";
import { PublisherService } from "@core/services/publisher.service";
import { Game, Publisher, SaveGame } from "@core/models/game.model";
import { PageShellComponent } from "@shared/templates/page-shell/page-shell";
import { GameFormComponent } from "@shared/organisms/game-form/game-form";
import { SpinnerComponent } from "@shared/atoms/spinner/spinner";

/**
 * PAGE: add (when no :id) or edit (when :id present) a game. The :id route param is
 * bound directly via withComponentInputBinding().
 */
@Component({
  selector: "ngs-game-edit-page",
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [PageShellComponent, GameFormComponent, SpinnerComponent],
  templateUrl: "./game-edit-page.html",
})
export class GameEditPage {
  private readonly gameService = inject(GameService);
  private readonly publisherService = inject(PublisherService);
  private readonly router = inject(Router);

  private readonly activeatedRoute = inject(ActivatedRoute);

  readonly id = input<string | undefined>();

  protected readonly game = signal<Game | null>(null);
  protected readonly publishers = signal<Publisher[]>([]);
  protected readonly loading = signal(false);
  protected readonly submitting = signal(false);
  protected readonly error = signal<string | null>(null);

  protected isEdit(): boolean {
    return !!this.id();
  }

  constructor() {
    this.publisherService.getAll().subscribe({
      next: (p) => this.publishers.set(p),
      error: () => this.publishers.set([]),
    });

    const idValue = Number(this.activeatedRoute.snapshot.paramMap.get("id"));

    console.log(idValue);

    if (idValue) {
      this.loading.set(true);
      this.gameService.getById(Number(idValue)).subscribe({
        next: (g) => {
          this.game.set(g);
          this.loading.set(false);
        },
        error: () => {
          this.error.set("Game not found.");
          this.loading.set(false);
        },
      });
    }
  }

  protected onSave(payload: SaveGame): void {
    this.submitting.set(true);
    this.error.set(null);

    const request$ = this.isEdit()
      ? this.gameService.update(Number(this.id()), payload)
      : this.gameService.create(payload);

    request$.subscribe({
      next: () => this.router.navigate(["/"]),
      error: () => {
        this.error.set(
          "Failed to save the game. Please check the fields and try again.",
        );
        this.submitting.set(false);
      },
    });
  }

  protected onCancel(): void {
    this.router.navigate(["/"]);
  }
}
