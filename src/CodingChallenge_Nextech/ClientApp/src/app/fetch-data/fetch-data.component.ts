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

  // public stories: Story[] = [];

  // constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
  //   http.get<Story[]>(baseUrl + 'weatherforecast').subscribe(result => {
  //     this.stories = result;
  //   }, error => console.error(error));
  // }

  //url: any;

  //constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
  //  this.url = baseUrl + 'weatherforecast';
  //}

  //getEmployees(options: Options):Observable {
  //  const url = `${this.url}?page=${options.page}&size=${options.size}&search=${options.search}&orderBy=${options.orderBy}&orderDir=${options.orderDir}`;
  //  return this.http.get(url).pipe(map(response => response));
  //}

}

// interface Story {
//   id: number;
//   title: string;
//   url: string;
// };

// interface Response {
//   records: Story[];
//   filtered: number;
//   total: number;
// };

// interface Options {
//   orderBy: string;
//   orderDir: 'ASC' | 'DESC';
//   search: string,
//   size: number,
//   page: number;
// };


// function search(this: any, $event: any, any: any) {
//   const text = $event.target.value;
//   this.options.search = text;
//   this.options.page = 1;
//   this.getEmployees();
// }
