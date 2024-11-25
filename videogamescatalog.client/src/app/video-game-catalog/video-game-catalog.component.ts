import { Component, OnInit } from '@angular/core';
import { VideoGame } from '../models/video-game-models';
import { VideoGameService } from '../services/video-game.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import {
  debounceTime,
  distinctUntilChanged,
  Observable,
  of,
  startWith,
  switchMap,
  tap,
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

  constructor(private videoGameService: VideoGameService) {}

  ngOnInit(): void {
    this.searchResults$ = this.titleSearchControl.valueChanges.pipe(
      startWith(''),
      debounceTime(this.DEBOUNCE_TIME),
      distinctUntilChanged(),
      switchMap((title) =>
        this.fetchData(title, this.pageNumber, this.pageSize)
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
          return of(response.results);
        })
      );
  }

  totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  nextPage() {
    if (this.pageNumber > this.totalPages()) {
      return;
    }

    this.pageNumber++;
    const title = this.titleSearchControl.value;
    this.searchResults$ = this.fetchData(title, this.pageNumber, this.pageSize);
  }

  prevPage() {
    if (this.pageNumber <= 1) {
      return;
    }

    this.pageNumber--;
    const title = this.titleSearchControl.value;
    this.searchResults$ = this.fetchData(title, this.pageNumber, this.pageSize);
  }
}
