export interface Genre {
  id: number;
  name: string;
}

export interface AgeRating {
  id: number;
  description: string;
}

export interface Platform {
  id: number;
  name: string;
}

export interface VideoGame {
  id: number;
  title: string;
  description: string;
  releaseDate: Date;
  price: number;
  ageRatingId: number;
  platformId: number;
  platform?: string;
  imageId?: number;
  imageUrl?: string;
  genres: Genre[];
}

export interface PagedResult<T> {
  results: T[];
  totalCount: number;
  pageSize: number;
  pageNumber: number;
}

export interface VideoGameImage {
  id: number;
  imageUrl: string;
}
