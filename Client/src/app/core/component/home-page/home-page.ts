import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MailService } from '../../services/mail-service';
import { Email } from '../../../shared/models/email';


interface FoodItem {
  name: string;
  emoji: string;
  calories: number;
  protein: number;
  carbs: number;
  fat: number;
}

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './home-page.html',
  styleUrl: './home-page.css',
})
export class HomePage {
  // 1. נתונים עבור הפיצ'ר היצירתי (צלחת דינמית)
  foodDatabase: FoodItem[] = [
    { name: 'חזה עוף בגריל', emoji: '🍗', calories: 165, protein: 31, carbs: 0, fat: 3.6 },
    { name: 'אורז בסמטי', emoji: '🍚', calories: 130, protein: 2.7, carbs: 28, fat: 0.3 },
    { name: 'אבוקדו טרי', emoji: '🥑', calories: 160, protein: 2, carbs: 8.5, fat: 15 },
    { name: 'שוקולד מריר', emoji: '🍫', calories: 220, protein: 2.5, carbs: 24, fat: 13 },
    { name: 'ביצה קשה', emoji: '🥚', calories: 78, protein: 6.3, carbs: 0.6, fat: 5.3 },
    { name: 'ברוקולי מאודה', emoji: '🥦', calories: 35, protein: 2.4, carbs: 7, fat: 0.4 }
  ];

  selectedFoods: FoodItem[] = [];
  totalCalories = 0;
  totalProtein = 0;
  totalCarbs = 0;
  totalFat = 0;
  feedbackMessage = '';

  // 2. נתונים עבור טופס יצירת קשר ושליחה לשרת (טיפוס מסוג Email)
  contactData: Email = {
    name: '',
    email: '',
    subject: '',
    message: ''
  };
  
  isSubmitting = false;
  submitStatus: 'success' | 'error' | null = null;
  serverErrorMessage = ''; // מחזיק את הודעת השגיאה הדינמית מהשרת (ex.Message)

  // הזרקת ה-MailService בלבד. ה-HttpClient מנוהל כעת בתוך השירות עצמו.
  constructor(private emailService: MailService) {}

  // לוגיקת ניהול הצלחת הדינמית
  addFood(item: FoodItem) {
    if (this.selectedFoods.length >= 8) {
      this.feedbackMessage = 'הצלחת כבר מלאה! נסי לנקות ולהרכיב מחדש.';
      return;
    }
    this.selectedFoods.push(item);
    this.updateTotals();
    this.generateSmartFeedback();
  }

  resetPlate() {
    this.selectedFoods = [];
    this.updateTotals();
    this.feedbackMessage = '';
  }

  updateTotals() {
    this.totalCalories = this.selectedFoods.reduce((sum, f) => sum + f.calories, 0);
    this.totalProtein = this.selectedFoods.reduce((sum, f) => sum + f.protein, 0);
    this.totalCarbs = this.selectedFoods.reduce((sum, f) => sum + f.carbs, 0);
    this.totalFat = this.selectedFoods.reduce((sum, f) => sum + f.fat, 0);
  }

  generateSmartFeedback() {
    const hasChocolate = this.selectedFoods.some(f => f.name === 'שוקולד מריר');
    const hasChicken = this.selectedFoods.some(f => f.name === 'חזה עוף בגריל');

    if (this.totalCalories > 550) {
      this.feedbackMessage = '🚨 חרגת מ-550 קלוריות לארוחה בודדת! באתר RightBite היית מקבלת התאמה אוטומטית למנה הבאה כדי לא להרוס את התהליך.';
    } else if (hasChocolate && !hasChicken) {
      this.feedbackMessage = '🍫 שוקולד הוא מעולה לנפש, אך הוא מקפיץ את הפחמימות. באתר שלנו נמליץ לך להוסיף חלבון (כמו ביצה או עוף) כדי לייצב את רמת הסוכר.';
    } else if (hasChicken && this.totalCarbs < 10) {
      this.feedbackMessage = '💪 פצצת חלבון! ארוחה מעולה לבניית שריר, אך חסרה פחמימה מורכבת לאנרגיה. מה עם קצת אורז?';
    } else {
      this.feedbackMessage = '🎯 שילוב נהדר ומאוזן! המערכת המלאה של RightBite כוללת מאגר של אלפי מאכלים ומחשבת הכל בלייב עבורך.';
    }
  }

  getMacroPercentage(current: number, target: number): number {
    return Math.min((current / target) * 100, 100);
  }

  // פונקציית שליחת המייל שעובדת מול ה-MailService שלך
  onSubmitContact() {
  if (!this.contactData.name || !this.contactData.email || !this.contactData.message) {
    return;
  }

  this.isSubmitting = true;
  this.submitStatus = null;
  this.serverErrorMessage = '';

  // קריאה חיונית ל-Service החדש שלך במקום ל-HttpClient הישיר!
  this.emailService.sendEmail(this.contactData).subscribe({
    next: (response) => {
      this.isSubmitting = false;
      this.submitStatus = 'success';
      // איפוס הטופס
      this.contactData = { name: '', email: '', subject: '', message: '' };
    },
    error: (error) => {
      this.isSubmitting = false;
      this.submitStatus = 'error';
      console.error('Email send failed:', error);
      
      // שליפת הודעת השגיאה הדינמית מה-Controller במידה וקיימת
      if (error.error && error.error.message) {
        this.serverErrorMessage = error.error.message;
      } else {
        this.serverErrorMessage = 'אופס! שגיאה בשילוח ההודעה לשרת ה-Localhost.';
      }
    }
  });
}
  scrollToSection(id: string) {
    document.getElementById(id)?.scrollIntoView({ behavior: 'smooth' });
  }
}