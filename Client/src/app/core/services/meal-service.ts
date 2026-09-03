import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { UserMeal } from '../../shared/models/user-meal';
import { Observable, tap } from 'rxjs';
import { DailyNutrition } from '../../shared/models/daily-nutrition';

@Injectable({
  providedIn: 'root',
})
export class MealService {

  constructor(private http:HttpClient) { }
  dailyNutrition=signal<DailyNutrition | null>(null);
  private apiUrl='https://localhost:7231/api/UserMeal/';
  loadDailyNutrition(userId: number, date: string): Observable<DailyNutrition> {
    return this.http.get<DailyNutrition>(`${this.apiUrl}GetDailyNutrition?id=${userId.toString()}&date=${date}`).pipe(
      tap((data) => {
        this.dailyNutrition.set(data); // עובד בצורה מושלמת ובטוחה
      })
    );
  }
  addFoodToEat(userMeal:UserMeal):Observable<number>{
    return this.http.post<number>(this.apiUrl, userMeal).pipe(
      tap(() => {
          this.loadDailyNutrition(userMeal.userId!, userMeal.mealDate).subscribe();
      })
    );
  }
  getUserMealsByDate(id: number, date: string): Observable<UserMeal[]> {
    const params = new HttpParams()
      .set('id', id.toString())
      .set('date', date);
    return this.http.get<UserMeal[]>(`${this.apiUrl}GetUserMealsByDate`, { params });
  }
}
