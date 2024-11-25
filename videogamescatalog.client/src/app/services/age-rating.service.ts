import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AgeRating } from '../models/video-game-models';

@Injectable({
  providedIn: 'root',
})
export class AgeRatingService {
  private ageRatingUrl = 'api/ageRating';

  constructor(private http: HttpClient) {}

  getAgeRatings() {
    return this.http.get<AgeRating[]>(this.ageRatingUrl);
  }
}
