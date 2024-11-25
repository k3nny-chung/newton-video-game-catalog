import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideoGameCatalogComponent } from './video-game-catalog.component';

describe('VideoGameCatalogComponent', () => {
  let component: VideoGameCatalogComponent;
  let fixture: ComponentFixture<VideoGameCatalogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [VideoGameCatalogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VideoGameCatalogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
