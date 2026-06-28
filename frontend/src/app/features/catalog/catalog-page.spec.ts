import { render, screen } from '@testing-library/angular';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { CatalogPage } from './catalog-page';
import { GameService } from '@core/services/game.service';
import { PublisherService } from '@core/services/publisher.service';
import { Game, GameGenre, PagedResult } from '@core/models/game.model';

function makeGame(id: number, title: string): Game {
  return {
    id, title, description: '', genre: GameGenre.Action, genreName: 'Action',
    platform: 'PC', price: 10, releaseDate: '2024-01-01', publisherId: 1, publisherName: 'Acme',
  };
}

describe('CatalogPage', () => {
  const page: PagedResult<Game> = {
    items: [makeGame(1, 'Doom'), makeGame(2, 'Quake')],
    page: 1, pageSize: 9, totalCount: 2, totalPages: 1, hasPrevious: false, hasNext: false,
  };

  const gameServiceStub = {
    search: jest.fn().mockReturnValue(of(page)),
    delete: jest.fn().mockReturnValue(of(void 0)),
  };
  const publisherServiceStub = {
    getAll: jest.fn().mockReturnValue(of([{ id: 1, name: 'Acme', country: 'US' }])),
  };

  it('loads and renders the seeded games', async () => {
    await render(CatalogPage, {
      providers: [
        provideRouter([]),
        { provide: GameService, useValue: gameServiceStub },
        { provide: PublisherService, useValue: publisherServiceStub },
      ],
    });

    expect(await screen.findByText('Doom')).toBeInTheDocument();
    expect(screen.getByText('Quake')).toBeInTheDocument();
    expect(screen.getByText(/2 game\(s\) found/i)).toBeInTheDocument();
    expect(gameServiceStub.search).toHaveBeenCalled();
  });
});
