import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Email } from '../../shared/models/email';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MailService {
 
// החלף בכתובת השרת האמיתית שלך (למשל: http://localhost:5000)
  private apiUrl = 'https://localhost:7231/api/Email/';

  constructor(private http: HttpClient) {}

  // פונקציה השולחת את הטופס ל-Controller
  sendEmail(resource: Email): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(this.apiUrl+'send', resource);
  }
}
