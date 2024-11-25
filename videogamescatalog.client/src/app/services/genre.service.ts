import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Genre } from '../models/video-game-models';

@Injectable({
  providedIn: 'root',
})
export class GenreService {
  private genreUrl = 'api/genre';

  constructor(private http: HttpClient) {}

  getGenres() {
    return this.http.get<Genre[]>(this.genreUrl);
  }
}
