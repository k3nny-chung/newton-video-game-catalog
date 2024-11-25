import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ImageService } from '../services/image.service';
import { VideoGameImage } from '../models/video-game-models';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrl: './file-upload.component.css',
})
export class FileUploadComponent {
  loading = false;
  isRemoving = false;
  file?: File | null;

  @Input() imageId?: number | null;
  @Input() imageUrl?: string | null;
  @Output() onImageChange = new EventEmitter<VideoGameImage | null>();

  constructor(private imageService: ImageService) {}

  onChange(event: Event) {
    if ((event.target as HTMLInputElement)?.files?.length) {
      this.file = (event.target as HTMLInputElement)?.files?.[0];
    }

    this.onUpload();
  }

  onUpload() {
    if (!this.file) {
      return;
    }

    this.loading = true;
    this.imageService.uploadImage(this.file).subscribe((result) => {
      const videoGameImage = result as VideoGameImage;
      this.imageId = videoGameImage.id;
      this.imageUrl = videoGameImage.imageUrl;
      this.onImageChange.emit(videoGameImage);
      this.loading = false;
    });
  }

  onRemovePhoto() {
    if (!this.imageId) {
      return;
    }

    this.isRemoving = true;
    this.imageService.removeImage(this.imageId).subscribe((_) => {
      this.isRemoving = false;
      this.file = null;
      this.imageId = null;
      this.imageUrl = null;
      this.onImageChange.emit(null);
    });
  }
}
