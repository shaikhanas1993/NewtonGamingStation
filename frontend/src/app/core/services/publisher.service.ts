import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { Publisher } from '../models/game.model';

@Injectable({ providedIn: 'root' })
export class PublisherService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/publishers`;

  getAll(): Observable<Publisher[]> {
    return this.http.get<Publisher[]>(this.baseUrl);
  }
}
