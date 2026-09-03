import { Component, Input } from '@angular/core';
import { UserMeal } from '../../../shared/models/user-meal';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-meal-list',
  imports: [CommonModule],
  standalone: true,
  templateUrl: './meal-list.html',
  styleUrl: './meal-list.css',
})
export class MealList {
 private _meals: UserMeal[] = [];
  @Input() 
  set meals(value: UserMeal[]) {
  this._meals = value;
  console.log('נתונים חדשים הגיעו לבת:', value); // ככה תוכלי לבדוק בקונסול אם זה עובד
 }
get meals(): UserMeal[] {
  return this._meals;
}

 
}
