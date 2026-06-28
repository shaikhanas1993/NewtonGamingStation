import { render, screen } from '@testing-library/angular';
import userEvent from '@testing-library/user-event';
import { GameCardComponent } from './game-card';
import { Game, GameGenre } from '@core/models/game.model';

const game: Game = {
  id: 42,
  title: 'Celeste',
  description: 'A precision platformer.',
  genre: GameGenre.Platformer,
  genreName: 'Platformer',
  platform: 'PC',
  price: 19.99,
  releaseDate: '2018-01-25',
  publisherId: 1,
  publisherName: 'Matt Makes Games',
};

describe('GameCardComponent', () => {
  it('renders the game details', async () => {
    await render(GameCardComponent, { inputs: { game } });

    expect(screen.getByText('Celeste')).toBeInTheDocument();
    expect(screen.getByText('Platformer')).toBeInTheDocument();
    expect(screen.getByText('PC')).toBeInTheDocument();
    expect(screen.getByText(/Matt Makes Games/)).toBeInTheDocument();
  });

  it('emits edit with the game id', async () => {
    const edit = jest.fn();
    await render(GameCardComponent, { inputs: { game }, on: { edit } });

    await userEvent.click(screen.getByRole('button', { name: /edit/i }));
    expect(edit).toHaveBeenCalledWith(42);
  });

  it('emits remove with the game', async () => {
    const remove = jest.fn();
    await render(GameCardComponent, { inputs: { game }, on: { remove } });

    await userEvent.click(screen.getByRole('button', { name: /delete/i }));
    expect(remove).toHaveBeenCalledWith(game);
  });
});
