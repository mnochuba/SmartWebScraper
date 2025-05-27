import { Component, inject, Injectable } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { CardModule } from 'primeng/card';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { ChipModule } from 'primeng/chip';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Result, SearchResult } from '../../../types';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    InputTextModule,
    ButtonModule,
    CheckboxModule,
    CardModule,
    InputGroupModule,
    InputGroupAddonModule,
    ChipModule,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
@Injectable({ providedIn: 'root' })
export class HomeComponent {
  private http = inject(HttpClient);

  url: string = '';
  newKeyword: string = '';
  keywords: string[] = [];
  loading: boolean = false;
  savingRankings: boolean = false;
  results: SearchResult = {};

  addKeyword() {
    if (this.newKeyword.trim()) {
      this.keywords.push(this.newKeyword.trim());
      this.newKeyword = '';
    }
  }

  removeKeyword(index: number) {
    this.keywords.splice(index, 1);
  }

  getObjectKeys(obj: object): string[] {
    return Object.keys(obj);
  }

  async beginSearch() {
    this.loading = true;
    try {
      this.http.post(`${environment.apiUrl}/search/keyword-search`, {
        searchPhrase: this.keywords.join(' '),
        targetUrl: this.url,
      })
      .subscribe({
        next: (res) => {
          this.results = (res as Result<SearchResult>).data;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
        },
      });
    } catch (error) {
      this.loading = false;
    }
  }

  saveResults() {
    this.savingRankings = true;
    try {
      this.http.post(`${environment.apiUrl}/search/save-results`, {
        searchPhrase: this.keywords.join(' '),
        targetUrl: this.url,
        rankings: this.results
      })
      .subscribe({
        next: () => {
          this.savingRankings = false;
        },
        error: () => {
          this.savingRankings = false;
        },
      });
    } catch (error) {
      this.savingRankings = false;
    }
  }
}
