import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PagedResult, VideoGame } from '../models/video-game-models';

@Injectable({
  providedIn: 'root',
})
export class VideoGameService {
  private videoGameUrl = 'api/videogame';
  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(private http: HttpClient) {}

  searchCatalog(title = '', pageNumber = 0, pageSize = 0) {
    return this.http.get<PagedResult<VideoGame>>(this.videoGameUrl, {
      params: {
        ...(title?.trim() ? { title } : {}),
        ...(pageNumber > 0 ? { pageNumber } : {}),
        ...(pageSize > 0 ? { pageSize } : {}),
      },
    });
  }

  getVideoGame(id: number) {
    return this.http.get<VideoGame>(`${this.videoGameUrl}/${id}`);
  }

  saveVideoGame(videoGame: VideoGame) {
    if (videoGame.id > 0) {
      return this.http.put(
        `${this.videoGameUrl}/${videoGame.id}`,
        videoGame,
        this.httpOptions
      );
    } else {
      return this.http.post(this.videoGameUrl, videoGame, this.httpOptions);
    }
  }
}
