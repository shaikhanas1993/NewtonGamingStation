import { render, screen } from '@testing-library/angular';
import userEvent from '@testing-library/user-event';
import { SearchBarComponent } from './search-bar';

describe('SearchBarComponent', () => {
  it('emits termChange as the user types', async () => {
    const termChange = jest.fn();
    await render(SearchBarComponent, { on: { termChange } });

    await userEvent.type(screen.getByPlaceholderText(/search games/i), 'halo');
    expect(termChange).toHaveBeenLastCalledWith('halo');
  });

  it('emits search when the button is clicked', async () => {
    const search = jest.fn();
    await render(SearchBarComponent, { inputs: { term: 'mario' }, on: { search } });

    await userEvent.click(screen.getByRole('button', { name: /search/i }));
    expect(search).toHaveBeenCalledWith('mario');
  });
});
