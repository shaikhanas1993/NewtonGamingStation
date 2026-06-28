import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { GameService } from './game.service';
import { GameGenre, PagedResult, Game } from '../models/game.model';

describe('GameService', () => {
  let service: GameService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GameService, provideHttpClient(), provideHttpClientTesting()],
    });
    service = TestBed.inject(GameService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpMock.verify());

  it('builds query params for search/filter/paginate', () => {
    const empty: PagedResult<Game> = {
      items: [], page: 1, pageSize: 9, totalCount: 0, totalPages: 0, hasPrevious: false, hasNext: false,
    };

    service
      .search({ search: 'zelda', genre: GameGenre.Adventure, page: 2, pageSize: 9, sortBy: 'price', desc: true })
      .subscribe((res) => expect(res.totalCount).toBe(0));

    const req = httpMock.expectOne((r) => r.url.endsWith('/games'));
    expect(req.request.params.get('search')).toBe('zelda');
    expect(req.request.params.get('genre')).toBe(String(GameGenre.Adventure));
    expect(req.request.params.get('page')).toBe('2');
    expect(req.request.params.get('sortBy')).toBe('price');
    expect(req.request.params.get('desc')).toBe('true');
    req.flush(empty);
  });

  it('posts a new game', () => {
    service
      .create({
        title: 'New', genre: GameGenre.Action, platform: 'PC', price: 10,
        releaseDate: '2024-01-01', publisherId: 1,
      })
      .subscribe();

    const req = httpMock.expectOne((r) => r.url.endsWith('/games'));
    expect(req.request.method).toBe('POST');
    expect(req.request.body.title).toBe('New');
    req.flush({ id: 1 });
  });

  it('deletes a game by id', () => {
    service.delete(5).subscribe();
    const req = httpMock.expectOne((r) => r.url.endsWith('/games/5'));
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });
});
