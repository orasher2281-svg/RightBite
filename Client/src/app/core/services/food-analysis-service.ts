import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { NutritionalInfo } from '../../shared/models/nutritional-info';
import { API_URL } from '../../api.config';
@Injectable({
  providedIn: 'root',
})
export class FoodAnalysisService {

private apiUrl = API_URL + '/api/UserMeal/analyze';

  constructor(private http: HttpClient) {}

  analyzeMeal(foodName: string, description: string, file: File | null): Observable<NutritionalInfo> {
    const formData = new FormData();
    
    // מוסיפים את השדות לטופס
    formData.append('FoodName', foodName);
    formData.append('Description', description);
    
    // מוסיפים את הקובץ אם קיים
    if (file) {
      formData.append('File', file, file.name);
    }

    return this.http.post<NutritionalInfo>(this.apiUrl, formData);
  }
}
