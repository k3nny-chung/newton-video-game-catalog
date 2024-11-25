import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { VideoGameCatalogComponent } from './video-game-catalog/video-game-catalog.component';
import { VideoGameDetailsComponent } from './video-game-details/video-game-details.component';

const routes: Routes = [
  { path: '', redirectTo: '/catalog', pathMatch: 'full' },
  { path: 'catalog', component: VideoGameCatalogComponent },
  {
    path: 'details/:id',
    component: VideoGameDetailsComponent,
  },
  {
    path: 'add',
    component: VideoGameDetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
