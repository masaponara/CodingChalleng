import { Component, OnInit } from '@angular/core';
import { StoriesService } from '../services/stories.service';


@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})

export class FetchDataComponent implements OnInit {

  Stories: any;
  totalRows: number = 0;
  pagination: number = 1;
  titleSearch: string = "";

  constructor(private storiesService: StoriesService) {}
  ngOnInit() {
    this.fetchStudents();
    console.log(this.fetchStudents());
  }
  fetchStudents() {
    this.storiesService.getNewStories(this.pagination, this.titleSearch).subscribe((res: any) => {
      this.Stories = res.data;
      this.totalRows = res.total;
      console.log(res.total);
    });
  }
  renderPage(event: number) {
    this.pagination = event;
    this.fetchStudents();
  }

  searchFilter() {
    this.pagination = 1;
    this.fetchStudents();
  }
}