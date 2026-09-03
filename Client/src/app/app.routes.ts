import { Routes } from '@angular/router';
import { Register } from './core/component/register/register';
import { Login } from './core/component/login/login';
import { AddFood } from './core/component/add-food/add-food';
import { DailyTrackerComponent } from './core/component/daily-tracker-component/daily-tracker-component';
import { HomePage } from './core/component/home-page/home-page';

export const routes: Routes = [
    // 1. שורת ברירת המחדל - כשנכנסים לאתר בלי נתיב, הוא יפנה אוטומטית ל-home
    { path: '', redirectTo: 'home', pathMatch: 'full' },
     { path: 'register', component: Register },
    { path: 'login', component: Login },
    { path: 'addFood', component: AddFood },
    { path: 'dailyTracker', component: DailyTrackerComponent },
     { path: 'home', component: HomePage },
];
