import {
  ChangeDetectionStrategy,
  Component,
  computed,
  effect,
  inject,
  input,
  output,
} from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import {
  GAME_GENRES,
  Game,
  Publisher,
  SaveGame,
} from "@core/models/game.model";
import { ButtonComponent } from "@shared/atoms/button/button";

/**
 * ORGANISM: the add / edit form
 */
@Component({
  selector: "ngs-game-form",
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ReactiveFormsModule, ButtonComponent],
  templateUrl: "./game-form.html",
})
export class GameFormComponent {
  private readonly fb = inject(FormBuilder);

  protected readonly genres = GAME_GENRES;

  readonly publishers = input<Publisher[]>([]);
  readonly initialValue = input<Game | null>(null);
  readonly submitting = input(false);
  readonly mode = input<"create" | "edit">("create");

  readonly save = output<SaveGame>();
  readonly cancel = output<void>();

  protected readonly submitLabel = computed(() =>
    this.mode() === "edit" ? "Save changes" : "Create game",
  );

  protected readonly form = this.fb.nonNullable.group({
    title: ["", [Validators.required, Validators.maxLength(200)]],
    description: [""],
    genre: [0, [Validators.required]],
    platform: ["", [Validators.required, Validators.maxLength(100)]],
    price: [0, [Validators.required, Validators.min(0)]],
    releaseDate: ["", [Validators.required]],
    publisherId: [null as number | null, [Validators.required]],
  });

  constructor() {
    // Patch the form whenever an initial value arrives (edit mode).
    effect(() => {
      const game = this.initialValue();
      if (game) {
        this.form.patchValue({
          title: game.title,
          description: game.description ?? "",
          genre: game.genre,
          platform: game.platform,
          price: game.price,
          releaseDate: game.releaseDate?.substring(0, 10),
          publisherId: game.publisherId,
        });
      }
    });
  }

  protected isInvalid(control: string): boolean {
    const c = this.form.get(control);
    return !!c && c.invalid && (c.dirty || c.touched);
  }

  protected onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const v = this.form.getRawValue();
    this.save.emit({
      title: v.title,
      description: v.description || null,
      genre: Number(v.genre),
      platform: v.platform,
      price: Number(v.price),
      releaseDate: v.releaseDate,
      publisherId: Number(v.publisherId),
    });
  }
}
