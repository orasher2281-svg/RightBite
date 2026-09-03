import { Component, effect, inject, signal, untracked } from '@angular/core';
import { UserService } from '../../services/user-service';
import { MealService } from '../../services/meal-service';
import { DailyNutrition } from '../../../shared/models/daily-nutrition';
import { CommonModule } from '@angular/common';
import { UserMeal } from '../../../shared/models/user-meal';
import { MealList } from '../meal-list/meal-list';
import { DateSelector } from '../date-selector/date-selector';
@Component({
  selector: 'app-daily-tracker-component',
  imports: [CommonModule, MealList,DateSelector],
  templateUrl: './daily-tracker-component.html',
  styleUrl: './daily-tracker-component.css',
})
export class DailyTrackerComponent {
  constructor() {
    effect(() => {
      const userId =Number(this.userService.currentUserId() ?? 0);
      const currentDate = this.selectedDate();
      untracked(() => {
        if (userId) {
          this.mealService.loadDailyNutrition(Number(userId), currentDate).subscribe();
          this.mealService.getUserMealsByDate(Number(userId), currentDate).subscribe(meals => {
            this.meals.set(meals);
          });
          if (!this.userService.currentUser()) {
             this.userService.getUserById(Number(userId)).subscribe();
          }
        }
      });
    });


  }
  userService=inject(UserService);
  mealService=inject(MealService);
   // סיגנל מקומי לשמירת התאריך הנבחר במסך (ברירת מחדל: היום)
  selectedDate = signal<string>(new Date().toISOString().split('T')[0]);
 // נשמור את המידע כסיגנל
  meals = signal<UserMeal[]>([]);
  showMeals: boolean = true; // מצב להצגת רשימת הארוחות
 
  // ngOnInit() {
  // const userId =Number(this.userService.currentUserId() ?? 0);
  // if (userId) {
  //   this.mealService.loadDailyNutrition(userId, this.selectedDate()).subscribe();
  //   // בודקים בנפרד: רק אם חסר המידע על המשתמש בגלל רענון, נשלף גם אותו
  //   if (this.userService.currentUser() === null) {
  //     this.userService.getUserById(userId).subscribe();
  //   }
  // }
   
  // }
   get nutrition() : DailyNutrition | null {
    return this.mealService.dailyNutrition();
  }
  // פונקציה שרצה כשמשנים תאריך בסרגל למעלה
  onDateChange(newDate: string) {
    this.selectedDate.set(newDate);
   
  }
  // 🌟 פונקציית עזר לחישוב האחוז (0-100) של ההתקדמות 🌟
  // זה ישמש אותנו כדי לצייר את העיגול ב-CSS
  getPercentage(current: number, target: number): number {
    if (!target || target <= 0) return 0; // מניעת חלוקה ב-0
    const perc = (current / target) * 100;
    return perc > 100 ? 100 : Math.round(perc); // מגביל ל-100% ומעגל
  }

// מחזיר style object עם top/left לפי אחוז על הטבעת
getIconPosition(perc: number): { [key: string]: string } {
  const clampedPerc = Math.min(perc, 99); // לא עובר 99% כדי לא לחזור להתחלה
  const angle = ((clampedPerc / 100) * 360 - 90) * (Math.PI / 180);
  const cx = 50;
  const cy = 50;
  const r = 42;
  const x = cx + r * Math.cos(angle);
  const y = cy + r * Math.sin(angle);
  return {
    'left': `calc(${x}% - 16px)`,
    'top': `calc(${y}% - 16px)`,
  };
}
Math = Math; // כדי שאפשר להשתמש ב-Math בתוך ה-HTML


}
