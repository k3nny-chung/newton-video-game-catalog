import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Platform } from '../models/video-game-models';

@Injectable({
  providedIn: 'root',
})
export class PlatformService {
  private platformUrl = 'api/platform';

  constructor(private http: HttpClient) {}

  getPlatforms() {
    return this.http.get<Platform[]>(this.platformUrl);
  }
}
