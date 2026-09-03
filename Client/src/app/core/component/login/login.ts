import { Component, inject } from '@angular/core';
import { LoginRequest } from '../../../shared/models/login-request';
import { UserService } from '../../services/user-service';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
@Component({

  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  router = inject(Router);
  userService = inject(UserService);
  loginRequest: LoginRequest = {
    email: '',
    password: ''
  };
  showPassword: boolean = false;
  onLogin() {
    this.userService.login(this.loginRequest).subscribe({
      next: (result) => {

        console.log('Login successful:', result);
        // הצגת הודעת הצלחה עם SweetAlert
        Swal.fire({
          title: 'התחברת בהצלחה!',
          text: 'ברוך הבא ל-RightBite',
          icon: 'success',
          toast: true,
          position: 'top-start', /* פינה שמאלית עליונה, מתאים לעברית */
          showConfirmButton: false,
          timer: 3000, /* ייעלם אוטומטית אחרי 3 שניות */
          timerProgressBar: true, /* פס התקדמות קטן למטה */
          didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
          }
        });
        this.router.navigate(['/addFood']);
      },
      error: (err) => {
        // קוד זה ירוץ אוטומטית בכל מקרה של שגיאה (401, 404, 500 וכו')
        console.error('ההתחברות נכשלה:', err);

        if (err.status === 404) {
          // 1. אם השגיאה היא 404 (NotFound) - המשתמש לא קיים בשרת
          alert('האימייל אינו קיים במערכת, מעביר אותך לעמוד ההרשמה...');
          this.router.navigate(['/register']); // ניווט אוטומטי להרשמה
        }
        else if (err.status === 401) {
          // 2. אם השגיאה היא 401 (Unauthorized) - המשתמש קיים אך הסיסמה שגויה
          alert('שם המשתמש או הסיסמה שגויים. אנא נסה שנית.');
        }
        else {
          // 3. לכל שגיאה אחרת (למשל בעיית תקשורת או שרת כבוי)
          alert('חלה שגיאה זמנית בשרת. אנא נסה שוב מאוחר יותר.');
        }
      }
    });
  }
}
