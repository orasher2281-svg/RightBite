import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Food } from '../../shared/models/food';

@Injectable({
  providedIn: 'root',
})
export class FoodService {
  constructor(private http: HttpClient) {}
  private apiUrl = 'https://localhost:7231/api/Food/';
  searchFoodByName(name:string):Observable<Food[]>{
    const params = new HttpParams().set('nameFood', name);
    return this.http.get<Food[]>(`${this.apiUrl}search`, { params });
  }
  addFood(food:Food):Observable<number>{
    return this.http.post<number>(this.apiUrl,food);
  }

}
