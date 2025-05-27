import { Routes } from '@angular/router';
import { HomeComponent } from './views/home/home.component';
import { HistoryComponent } from './views/history/history.component';

export const routes: Routes = [
  { path: '', title: 'Home', component: HomeComponent },
  { path: 'history', title: 'History', component: HistoryComponent },
];
