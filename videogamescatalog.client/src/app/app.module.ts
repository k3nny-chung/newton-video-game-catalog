import {
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { VideoGameCatalogComponent } from './video-game-catalog/video-game-catalog.component';
import { VideoGameDetailsComponent } from './video-game-details/video-game-details.component';
import { ReactiveFormsModule } from '@angular/forms';
import { FileUploadComponent } from './file-upload/file-upload.component';
import { CurrencyPipe, DatePipe } from '@angular/common';

@NgModule({
  declarations: [AppComponent, VideoGameDetailsComponent, FileUploadComponent],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    VideoGameCatalogComponent,
    ReactiveFormsModule,
    CurrencyPipe,
    DatePipe,
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    CurrencyPipe,
    DatePipe,
  ],
})
export class AppModule {}
