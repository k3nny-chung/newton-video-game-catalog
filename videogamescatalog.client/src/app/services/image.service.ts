import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
  private imageUrl = 'api/image';

  constructor(private http: HttpClient) {}

  uploadImage(file: File) {
    const formData = new FormData();
    formData.append('imageFile', file, file.name);
    return this.http.post(this.imageUrl, formData);
  }

  removeImage(videoGameImageId: number) {
    return this.http.delete(`${this.imageUrl}/${videoGameImageId}`);
  }
}
