import { TestBed } from '@angular/core/testing';

import { AgeRatingService } from './age-rating.service';

describe('AgeRatingService', () => {
  let service: AgeRatingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgeRatingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
