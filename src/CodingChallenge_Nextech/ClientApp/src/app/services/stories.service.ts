import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class StoriesService {

  private url = 'story';

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string) {
     this.url = baseUrl + this.url;
  }
  
  getNewStories(page: number, titleSearch: string) {
    return this.httpClient.get(this.url + '?page=' + page + '&titleOrId=' + titleSearch);
  }
}
