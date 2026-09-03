import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user-service';
import { User } from '../../../shared/models/user';
import { UserGender } from '../../../shared/models/user-gender';
import { UserGoal } from '../../../shared/models/user-goal';
import { CommonModule } from '@angular/common';
import { DeferBlockBehavior } from '@angular/core/testing';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [FormsModule, CommonModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private userService = inject(UserService);
  private router = inject(Router);
  currentStep = 1;
  totalSteps = 3;
  UserGender = UserGender;
  UserGoal = UserGoal;
  genderOptions = [UserGender.Male, UserGender.Female];
  goalOptions = [UserGoal.Lose, UserGoal.Gain, UserGoal.Maintain];
  user: User = {
    name: '',
    password: '',
    email: '',
    weight: Number.NaN,
    height: Number.NaN,
    age: Number.NaN,
    gender: UserGender.Male,
    goal: UserGoal.Lose
  };


  //קשור לעיצוב של הטופס, לא צריך לגעת בו
  get bmi(): number {
    const h = this.user.height / 100;
    return Math.round((this.user.weight / (h * h)) * 10) / 10;
  }

  get bmiCategory(): string {
    if (this.bmi < 18.5) return 'תת משקל';
    if (this.bmi < 25) return 'משקל תקין';
    if (this.bmi < 30) return 'עודף משקל';
    return 'השמנה';
  }

  get bmiColor(): string {
    if (this.bmi < 18.5) return '#64b5f6';
    if (this.bmi < 25) return '#C5E45D';
    if (this.bmi < 30) return '#FF7B2F';
    return '#e53935';
  }

  get bmiPercent(): number {
    return Math.min(Math.max(((this.bmi - 10) / 30) * 100, 0), 100);
  }

  get stepInfo() {
    const steps = [
      { char: '🥚', title: 'נחשב לך הכל', sub: 'מדדים גופניים' },
      { char: '🍎', title: 'מה המטרה שלך?', sub: 'יעד תזונתי' },
      { char: '🍞', title: 'כמעט סיימנו!', sub: 'פרטי חשבון' }
    ];
    return steps[this.currentStep - 1];
  }

  get goalLabel(): string {
    if (this.user.goal === UserGoal.Lose) return 'ירידה במשקל';
    if (this.user.goal === UserGoal.Gain) return 'עלייה במשקל';
    return 'שמירה על משקל';
  }

  nextStep() { if (this.currentStep < this.totalSteps) this.currentStep++; }
  prevStep() { if (this.currentStep > 1) this.currentStep--; }

  adj(field: 'weight' | 'height' | 'age', delta: number) {
    this.user[field] = Math.round((this.user[field] + delta) * 10) / 10;
  }

  onRegister() {
    this.userService.register(this.user!).subscribe({
      next: (result) => {
        console.log('Registration successful:', result);
        Swal.fire({
          title: 'נירשמת בהצלחה!',
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
        this.userService.login({ email: this.user.email, password: this.user.password }).subscribe({
          next: (result) => {
            console.log('Login successful:', result);
            this.router.navigate(['/addFood']);
          },
          error: (err) => {
            console.error('Login failed after registration:', err);
              alert('Registration succeeded but automatic login failed. Please try logging in manually.');
            }   
        });
      },
      error: (error) => {
        debugger;
        alert('Registration failed. Please check your input and try again.');
        console.error('Registration failed:', error);
      }
    });
  }

}
