import { Component, OnInit } from '@angular/core';
import { VideoGame } from '../models/video-game-models';
import { VideoGameService } from '../services/video-game.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import {
  BehaviorSubject,
  combineLatestWith,
  debounceTime,
  distinctUntilChanged,
  Observable,
  of,
  switchMap,
} from 'rxjs';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-video-game-catalog',
  standalone: true,
  imports: [CommonModule, RouterLink, ReactiveFormsModule],
  templateUrl: './video-game-catalog.component.html',
  styleUrl: './video-game-catalog.component.css',
})
export class VideoGameCatalogComponent implements OnInit {
  private DEBOUNCE_TIME = 350;
  videoGames: VideoGame[] = [];
  searchResults$!: Observable<VideoGame[]>;
  totalCount = 0;
  pageNumber = 1;
  pageSize = 6;
  isLoading = false;
  private titleSearchTerms$ = new BehaviorSubject('');
  private pageChange$ = new BehaviorSubject(1);

  constructor(private videoGameService: VideoGameService) {}

  ngOnInit(): void {
    this.searchResults$ = this.titleSearchTerms$.pipe(
      debounceTime(this.DEBOUNCE_TIME),
      distinctUntilChanged(),
      combineLatestWith(this.pageChange$),
      switchMap(([title, pageNumber]) =>
        this.fetchData(title, pageNumber, this.pageSize)
      )
    );
  }

  fetchData(title?: string, pageNumber?: number, pageSize?: number) {
    this.isLoading = true;
    return this.videoGameService
      .searchCatalog(title, pageNumber, pageSize)
      .pipe(
        switchMap((response) => {
          this.videoGames = response.results;
          this.totalCount = response.totalCount;
          this.isLoading = false;
          if (!response.results?.length) {
            this.setPage(1);
          }

          return of(response.results);
        })
      );
  }

  searchTitle(title: string) {
    this.titleSearchTerms$.next(title);
  }

  totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  setPage(pageNumber: number) {
    if (this.pageNumber !== pageNumber) {
      this.pageNumber = pageNumber;
      this.pageChange$.next(this.pageNumber);
    }
  }

  nextPage() {
    if (this.pageNumber > this.totalPages()) {
      return;
    }

    this.setPage(this.pageNumber + 1);
  }

  prevPage() {
    if (this.pageNumber <= 1) {
      return;
    }

    this.setPage(this.pageNumber - 1);
  }
}
