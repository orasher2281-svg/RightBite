import { HttpClient } from '@angular/common/http';
import { computed, Injectable, signal } from '@angular/core';
import { User } from '../../shared/models/user';
import { Observable, tap } from 'rxjs';
import { AuthResult } from '../../shared/models/auth-result';
import { LoginRequest } from '../../shared/models/login-request';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http:HttpClient) {}
  currentUserId = signal<number | null>(
    localStorage.getItem('currentUserId') ? Number(localStorage.getItem('currentUserId')) : null
  );
// 1. המשתנה הבודד במערכת - סיגנל שמחזיק את המשתמש המלא (בהתחלה הוא null)
  currentUser = signal<User | null>(null);
 // 2. סיגנל עבור הטוקן
  token = signal<string | null>(localStorage.getItem('token'));

  // 3. סיגנל מחושב שמציג האם המשתמש מחובר (נוחות לכל הפרויקט)
  isLoggedIn = computed(() => this.token() !== null);
  private apiUrl='https://localhost:7231/api/Users/';
  getUserById(userId:number):Observable<User>{
    return this.http.get<User>(this.apiUrl+userId).pipe(
      tap(user => this.currentUser.set(user)) // ברגע שמקבלים את המשתמש, נשמור אותו בסיגנל
    );
  }

  
  register(user:User):Observable<AuthResult>{
    return this.http.post<AuthResult>(this.apiUrl+'Register',user).pipe(
      tap((response: AuthResult) => {
        this.token.set(response.token) ;
        this.currentUserId.set(response.userId);
            this.getUserById(response.userId).subscribe(); // טוען את פרטי המשתמש ומעדכן את currentUser
        localStorage.setItem('token', response.token);
        localStorage.setItem('currentUserId', response.userId.toString());  
     })
    )
  }
  login(loginRequest: LoginRequest):Observable<AuthResult>{
    return this.http.post<AuthResult>(this.apiUrl+'Login',loginRequest).pipe(
      tap((response) => {
       if (response && response.token && response.userId) {
        this.token.set(response.token);
        this.currentUserId.set(response.userId);
            this.getUserById(response.userId).subscribe(); // טוען את פרטי המשתמש ומעדכן את currentUser
        localStorage.setItem('token', response.token);
         localStorage.setItem('currentUserId', response.userId.toString());}
      })
    )
  }
  logout(){
    this.token.set(null) ;
    this.currentUser.set(null);
    this.currentUserId.set(null);
    localStorage.removeItem('token');
    localStorage.removeItem('currentUserId');
  }
  getToken(): string | null {
    return this.token()|| localStorage.getItem('token');
  }
}
