import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoGameDetailsComponent } from './video-game-details.component';

describe('VideoGameDetailsComponent', () => {
  let component: VideoGameDetailsComponent;
  let fixture: ComponentFixture<VideoGameDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [VideoGameDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VideoGameDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
