/** Mirrors the backend GameGenre enum (numeric values must match). */
export enum GameGenre {
  Action = 0,
  Adventure = 1,
  RolePlaying = 2,
  Strategy = 3,
  Simulation = 4,
  Sports = 5,
  Puzzle = 6,
  Shooter = 7,
  Racing = 8,
  Platformer = 9,
}

export const GAME_GENRES: { value: GameGenre; label: string }[] = [
  { value: GameGenre.Action, label: 'Action' },
  { value: GameGenre.Adventure, label: 'Adventure' },
  { value: GameGenre.RolePlaying, label: 'Role Playing' },
  { value: GameGenre.Strategy, label: 'Strategy' },
  { value: GameGenre.Simulation, label: 'Simulation' },
  { value: GameGenre.Sports, label: 'Sports' },
  { value: GameGenre.Puzzle, label: 'Puzzle' },
  { value: GameGenre.Shooter, label: 'Shooter' },
  { value: GameGenre.Racing, label: 'Racing' },
  { value: GameGenre.Platformer, label: 'Platformer' },
];

export interface Game {
  id: number;
  title: string;
  description?: string | null;
  genre: GameGenre;
  genreName: string;
  platform: string;
  price: number;
  releaseDate: string; // ISO date (yyyy-MM-dd)
  publisherId: number;
  publisherName: string;
}

export interface SaveGame {
  title: string;
  description?: string | null;
  genre: GameGenre;
  platform: string;
  price: number;
  releaseDate: string;
  publisherId: number;
}

export interface Publisher {
  id: number;
  name: string;
  country?: string | null;
}

export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface GameQuery {
  search?: string;
  genre?: GameGenre | null;
  platform?: string | null;
  publisherId?: number | null;
  sortBy?: string;
  desc?: boolean;
  page: number;
  pageSize: number;
}
