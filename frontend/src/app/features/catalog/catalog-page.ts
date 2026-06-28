import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GameService } from '@core/services/game.service';
import { PublisherService } from '@core/services/publisher.service';
import { Game, GameGenre, Publisher } from '@core/models/game.model';
import { PageShellComponent } from '@shared/templates/page-shell/page-shell';
import { SearchBarComponent } from '@shared/molecules/search-bar/search-bar';
import { FilterBarComponent, FilterState } from '@shared/molecules/filter-bar/filter-bar';
import { GameGridComponent } from '@shared/organisms/game-grid/game-grid';
import { ConfirmDialogComponent } from '@shared/molecules/confirm-dialog/confirm-dialog';

/**
 * PAGE: the browsing catalogue. Owns the query state (signals) and orchestrates the
 * organisms. This is the only place in the feature that talks to services.
 */
@Component({
  selector: 'ngs-catalog-page',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    RouterLink,
    PageShellComponent,
    SearchBarComponent,
    FilterBarComponent,
    GameGridComponent,
  ],
  template: `
    <ngs-page-shell heading="Game Catalogue" subheading="Browse, search and manage the collection">
      <a actions class="btn btn-success" routerLink="/games/new">+ Add Game</a>

      <div class="card card-body mb-3">
        <div class="mb-3">
          <ngs-search-bar
            [term]="search()"
            (termChange)="search.set($event)"
            (search)="onSearch($event)"
          />
        </div>
        <ngs-filter-bar
          [genre]="genre()"
          [publisherId]="publisherId()"
          [publishers]="publishers()"
          [sortBy]="sortBy()"
          [desc]="desc()"
          (filterChange)="onFilterChange($event)"
        />
      </div>

      @if (error()) {
        <div class="alert alert-danger" role="alert">{{ error() }}</div>
      }

      <p class="text-muted small">{{ totalCount() }} game(s) found.</p>

      <ngs-game-grid
        [games]="games()"
        [loading]="loading()"
        [page]="page()"
        [pageSize]="pageSize()"
        [totalItems]="totalCount()"
        (edit)="onEdit($event)"
        (remove)="onRemove($event)"
        (pageChange)="onPageChange($event)"
      />
    </ngs-page-shell>
  `,
})
export class CatalogPage {
  private readonly gameService = inject(GameService);
  private readonly publisherService = inject(PublisherService);
  private readonly router = inject(Router);
  private readonly modal = inject(NgbModal);

  // --- query state ---
  protected readonly search = signal('');
  protected readonly genre = signal<GameGenre | null>(null);
  protected readonly publisherId = signal<number | null>(null);
  protected readonly sortBy = signal('title');
  protected readonly desc = signal(false);
  protected readonly page = signal(1);
  protected readonly pageSize = signal(9);

  // --- result state ---
  protected readonly games = signal<Game[]>([]);
  protected readonly totalCount = signal(0);
  protected readonly publishers = signal<Publisher[]>([]);
  protected readonly loading = signal(false);
  protected readonly error = signal<string | null>(null);

  protected readonly totalPages = computed(() =>
    Math.ceil(this.totalCount() / this.pageSize()),
  );

  constructor() {
    this.publisherService.getAll().subscribe({
      next: (p) => this.publishers.set(p),
      error: () => this.publishers.set([]),
    });
    this.load();
  }

  protected onSearch(term: string): void {
    this.search.set(term);
    this.page.set(1);
    this.load();
  }

  protected onFilterChange(state: FilterState): void {
    this.genre.set(state.genre);
    this.publisherId.set(state.publisherId);
    this.sortBy.set(state.sortBy);
    this.desc.set(state.desc);
    this.page.set(1);
    this.load();
  }

  protected onPageChange(page: number): void {
    this.page.set(page);
    this.load();
  }

  protected onEdit(id: number): void {
    this.router.navigate(['/games', id, 'edit']);
  }

  protected async onRemove(game: Game): Promise<void> {
    const ref = this.modal.open(ConfirmDialogComponent, { centered: true });
    ref.componentInstance.title = 'Delete game';
    ref.componentInstance.message = `Delete "${game.title}"? This cannot be undone.`;
    ref.componentInstance.confirmLabel = 'Delete';

    try {
      const confirmed = await ref.result;
      if (confirmed) {
        this.gameService.delete(game.id).subscribe({
          next: () => this.load(),
          error: () => this.error.set('Failed to delete the game. Please try again.'),
        });
      }
    } catch {
      // dismissed — no-op
    }
  }

  private load(): void {
    this.loading.set(true);
    this.error.set(null);
    this.gameService
      .search({
        search: this.search(),
        genre: this.genre(),
        publisherId: this.publisherId(),
        sortBy: this.sortBy(),
        desc: this.desc(),
        page: this.page(),
        pageSize: this.pageSize(),
      })
      .subscribe({
        next: (result) => {
          this.games.set(result.items);
          this.totalCount.set(result.totalCount);
          this.loading.set(false);
        },
        error: () => {
          this.error.set('Could not load games. Is the API running?');
          this.games.set([]);
          this.loading.set(false);
        },
      });
  }
}
