import { Component, inject } from '@angular/core';
import { History, Result, SearchResult } from '../../../types';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-history',
  imports: [CommonModule, DatePipe],
  templateUrl: './history.component.html',
  styleUrl: './history.component.scss'
})
export class HistoryComponent {
  private http = inject(HttpClient);
  history: History[] = [];
  results: SearchResult = {};

  getObjectKeys(obj: object): string[] {
    return Object.keys(obj);
  }

  ngOnInit() {
    this.http.get(`${environment.apiUrl}/search/search-history`)
    .subscribe({
      next: (res) => {
        this.history = (res as Result<History[]>).data;
      },
      // complete: () => {
      //   this.history = this.history.sort((a, b) => new Date(b.searchDate).getTime() - new Date(a.searchDate).getTime());
      // }
    });
  }

  selectResult(item: History) {
    this.results = item.rankings;
  }
}
