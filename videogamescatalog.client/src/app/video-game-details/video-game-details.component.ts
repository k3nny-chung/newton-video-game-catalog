import { GenreService } from './../services/genre.service';
import { Component, OnInit } from '@angular/core';
import {
  AgeRating,
  Genre,
  Platform,
  VideoGame,
  VideoGameImage,
} from '../models/video-game-models';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { VideoGameService } from '../services/video-game.service';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { AgeRatingService } from '../services/age-rating.service';
import { forkJoin, of } from 'rxjs';
import { PlatformService } from '../services/platform.service';

@Component({
  selector: 'app-video-game-details',
  templateUrl: './video-game-details.component.html',
  styleUrl: './video-game-details.component.css',
})
export class VideoGameDetailsComponent implements OnInit {
  id = 0;
  imageUrl?: string;
  imageId?: number;
  ageRatings: AgeRating[] = [];
  genres: Genre[] = [];
  platforms: Platform[] = [];
  videoGameForm!: FormGroup;
  createMode: boolean = true;
  formSubmitted = false;
  isFormSubmitting = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private videoGameService: VideoGameService,
    private ageRatingService: AgeRatingService,
    private genreService: GenreService,
    private platformService: PlatformService,
    private formBuilder: FormBuilder,
    private datePipe: DatePipe
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.createMode = false;
      this.id = Number(idParam);
    }

    this.videoGameForm = this.formBuilder.group({
      title: '',
      description: '',
      releaseDate: '',
      price: '',
      ageRating: -1,
      platform: -1,
      genres: new FormArray([]),
    });

    forkJoin([
      this.ageRatingService.getAgeRatings(),
      this.genreService.getGenres(),
      this.platformService.getPlatforms(),
      this.createMode
        ? of(undefined)
        : this.videoGameService.getVideoGame(this.id),
    ]).subscribe(([ageRatingData, genreData, platformData, videoGameData]) => {
      this.ageRatings = ageRatingData;
      this.genres = genreData;
      this.platforms = platformData;
      this.imageUrl = videoGameData?.imageUrl;
      this.imageId = videoGameData?.imageId;

      const releaseDateFormatted = this.datePipe.transform(
        videoGameData?.releaseDate,
        'yyyy-MM-dd'
      );

      this.videoGameForm = this.formBuilder.group({
        title: new FormControl(videoGameData?.title ?? '', {
          validators: [Validators.required],
        }),
        description: new FormControl(videoGameData?.description ?? '', {
          validators: Validators.required,
        }),
        releaseDate: new FormControl(releaseDateFormatted, {
          validators: Validators.required,
        }),
        price: new FormControl(videoGameData?.price ?? '', {
          validators: [
            Validators.required,
            Validators.pattern(/^\d+(\.\d{1,2})?$/), // Regex for up to 2 decimal places
          ],
          updateOn: 'change',
        }),
        ageRating: new FormControl(videoGameData?.ageRatingId ?? -1, {
          validators: Validators.min(1),
        }),
        platform: new FormControl(videoGameData?.platformId ?? -1, {
          validators: Validators.min(1),
        }),
        genres: this.formBuilder.array(
          genreData.map((g) => {
            const hasGenre = (videoGameData?.genres ?? []).find(
              (vg) => vg.id == g.id
            );
            return new FormControl(hasGenre);
          }),
          {
            validators: this.atLeastOneGenreChecked,
            updateOn: 'change',
          }
        ),
      });
    });
  }

  get controls() {
    return this.videoGameForm.controls;
  }

  atLeastOneGenreChecked(control: AbstractControl): ValidationErrors | null {
    if (!Array.isArray(control?.value)) {
      return null;
    }

    return (control as FormArray).controls.some((c) => Boolean(c.value))
      ? null
      : { atLeastOneGenreChecked: true };
  }

  onFileUploaded(videoGameImage: VideoGameImage | null) {
    this.imageId = videoGameImage?.id;
    this.imageUrl = videoGameImage?.imageUrl;
  }

  onSubmit() {
    this.formSubmitted = true;

    if (this.videoGameForm.invalid) {
      return;
    }

    const selectedGenres = this.genres.filter(
      (g, index) =>
        (this.videoGameForm.get('genres') as FormArray).at(index).value
    );

    const data: VideoGame = {
      id: this.id,
      title: this.videoGameForm.value.title,
      description: this.videoGameForm.value.description,
      releaseDate: new Date(this.videoGameForm.value.releaseDate),
      price: Number(this.videoGameForm.value.price),
      ageRatingId: this.videoGameForm.value.ageRating,
      platformId: this.videoGameForm.value.platform,
      genres: selectedGenres,
      imageId: this.imageId,
    };

    this.isFormSubmitting = true;
    this.videoGameService.saveVideoGame(data).subscribe((_) => {
      this.isFormSubmitting = false;
      this.router.navigateByUrl('/catalog');
    });
  }
}
