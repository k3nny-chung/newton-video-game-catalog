import { Component, OnInit } from '@angular/core';
import { VideoGame } from '../models/video-game-models';
import { VideoGameService } from '../services/video-game.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import {
  combineLatestWith,
  debounceTime,
  distinctUntilChanged,
  Observable,
  of,
  startWith,
  Subject,
  switchMap,
} from 'rxjs';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-video-game-catalog',
  standalone: true,
  imports: [CommonModule, RouterLink, ReactiveFormsModule],
  templateUrl: './video-game-catalog.component.html',
  styleUrl: './video-game-catalog.component.css',
})
export class VideoGameCatalogComponent implements OnInit {
  private DEBOUNCE_TIME = 350;
  titleSearchControl = new FormControl();
  videoGames: VideoGame[] = [];
  searchResults$!: Observable<VideoGame[]>;
  totalCount = 0;
  pageNumber = 1;
  pageSize = 6;
  isLoading = false;
  private pageChange$ = new Subject<number>();

  constructor(private videoGameService: VideoGameService) {}

  ngOnInit(): void {
    this.searchResults$ = this.titleSearchControl.valueChanges.pipe(
      startWith(''),
      debounceTime(this.DEBOUNCE_TIME),
      distinctUntilChanged(),
      combineLatestWith(this.pageChange$.pipe(startWith(1))),
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

  totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  setPage(pageNumber: number) {
    this.pageNumber = pageNumber;
    this.pageChange$.next(this.pageNumber);
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
