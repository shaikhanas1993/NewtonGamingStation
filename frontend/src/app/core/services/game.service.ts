import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "@env/environment";
import { Game, GameQuery, PagedResult, SaveGame } from "../models/game.model";

/**
 * HTTP wrapper around the games API.
 *
 */
@Injectable({ providedIn: "root" })
export class GameService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/games`;

  constructor() {
    console.log("--this.baseUrl----");
    console.log(this.baseUrl);
    console.log("------");
  }

  search(query: GameQuery): Observable<PagedResult<Game>> {
    let params = new HttpParams()
      .set("page", query.page)
      .set("pageSize", query.pageSize);

    if (query.search) params = params.set("search", query.search);
    if (query.genre !== null && query.genre !== undefined)
      params = params.set("genre", query.genre);
    if (query.platform) params = params.set("platform", query.platform);
    if (query.publisherId)
      params = params.set("publisherId", query.publisherId);
    if (query.sortBy) params = params.set("sortBy", query.sortBy);
    if (query.desc !== undefined) params = params.set("desc", query.desc);

    return this.http.get<PagedResult<Game>>(this.baseUrl, { params });
  }

  getById(id: number): Observable<Game> {
    return this.http.get<Game>(`${this.baseUrl}/${id}`);
  }

  create(game: SaveGame): Observable<Game> {
    return this.http.post<Game>(this.baseUrl, game);
  }

  update(id: number, game: SaveGame): Observable<Game> {
    return this.http.put<Game>(`${this.baseUrl}/${id}`, game);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
