import { ChangeDetectorRef, Component, inject, ViewEncapsulation } from '@angular/core';
import { Food } from '../../../shared/models/food';
import { FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, filter, Subject, Subscription } from 'rxjs';
import { FoodService } from '../../services/food-service';
import { MealService } from '../../services/meal-service';
import { UserMeal } from '../../../shared/models/user-meal';
import { AddCustomFood } from '../add-custom-food/add-custom-food';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-add-food',
  encapsulation: ViewEncapsulation.None,  // עיצובים יוחלו גם על רכיבים פנימיים כמו AddCustomFood
  standalone: true,
  imports: [FormsModule, AddCustomFood, CommonModule],
  templateUrl: './add-food.html',
  styleUrl: './add-food.css',
})
export class AddFood {
  foods: Food[] = [];
  selectFood: Food | null = null;
  nameFood: string = '';
  userMeal: UserMeal = {
    foodId: Number.NaN,
    quantity: 100,              // ברירת מחדל לכמות (למשל 100 גרם)
    mealType: 'Breakfast',// ברירת מחדל לסוג הארוחה
    mealDate: new Date().toISOString().split('T')[0]
  }
  isAddingNew: boolean = false; // מצב להצגת הטופס של הבן
  // Subject שיקלוט את כל שינויי ההקלדה
  private searchSubject = new Subject<string>();
  private searchSubscription!: Subscription;
  private router = inject(Router);
  defaultFoods: any[] = [
    {
      id: 65,
      name: "חזה עוף צלוי",
      description: "עשיר בחלבון",
      imageUrl: "https://localhost:7231/images/chicken.jpg",
      calories: 165,
      protein: 31,
      carbs: 0,
      fat: 3.6
    },
    {
      id: 66,
      name: "אורז לבן",
      description: "פחמימה בסיסית",
      imageUrl: "https://localhost:7231/images/rice.jpg",
      calories: 130,
      protein: 2.4,
      carbs: 28,
      fat: 0.3
    },
    {
      id: 67,
      name: "אבוקדו",
      description: "שומן בריא",
      imageUrl: "https://localhost:7231/images/avocado.jpg",
      calories: 160,
      protein: 2,
      carbs: 9,
      fat: 15
    },
    {
      id: 68,
      name: "ביצה קשה",
      description: "חלבון איכותי",
      imageUrl: "https://localhost:7231/images/egg.jpg",
      calories: 155,
      protein: 13,
      carbs: 1,
      fat: 11
    },
    {
      id: 69,
      name: "סלמון",
      description: "אומגה 3",
      imageUrl: "https://localhost:7231/images/salmon.jpg",
      calories: 208,
      protein: 20,
      carbs: 0,
      fat: 13
    },
    {
      id: 70,
      name: "לחם מלא",
      description: "דגן מלא",
      imageUrl: "https://localhost:7231/images/bread.jpg",
      calories: 247,
      protein: 13,
      carbs: 41,
      fat: 4
    },
    {
      id: 71,
      name: "ברוקולי",
      description: "ירק בריא",
      imageUrl: "https://localhost:7231/images/broccoli.jpg",
      calories: 55,
      protein: 3.7,
      carbs: 11,
      fat: 0.6
    },
    {
      id: 72,
      name: "תפוח",
      description: "פרי מתוק",
      imageUrl: "https://localhost:7231/images/apple.jpg",
      calories: 52,
      protein: 0.3,
      carbs: 14,
      fat: 0.2
    },
    {
      id: 75,
      name: "בננה",
      description: "אנרגיה מהירה",
      imageUrl: "https://localhost:7231/images/banana.jpg",
      calories: 89,
      protein: 1.1,
      carbs: 23,
      fat: 0.3
    }
  ];

  quickFoods = [
    { emoji: '🍽️', name: 'חזה עוף צלוי', cal: 165, fat: 3.6, carb: 0, prot: 31 },
    { emoji: '🍽️', name: 'אורז לבן', cal: 130, fat: 0.3, carb: 28, prot: 2.4 },
    { emoji: '🍽️', name: 'ביצה קשה', cal: 155, fat: 11, carb: 1, prot: 13 },
    { emoji: '🍽️', name: 'סלמון', cal: 208, fat: 13, carb: 0, prot: 20 }
  ];

  selectedQuickFood = this.quickFoods[0];
  quickFoodQty: number = 100;
  quickCalories: number = 160;
  quickFat: number = 15;
  quickCarbs: number = 9;
  quickProtein: number = 2;
  constructor(private foodService: FoodService, private mealService: MealService, private cdr: ChangeDetectorRef) { }
  ngOnInit() {
    this.foods = [...this.defaultFoods];
    // נרשם ל-Subject כדי לקבל את כל שינויי ההקלדה
    this.searchSubscription = this.searchSubject.pipe(
      // כאן אפשר להוסיף debounceTime אם רוצים להמתין קצת לפני החיפוש
      debounceTime(300),
      distinctUntilChanged(), // מונע קריאות חוזרות לאותו ערך
      // התיקון כאן: הצינור ימשיך הלאה רק אם המשתמש הקליד לפחות אות אחת (אורך גדול מ-0)
      filter(value => value.trim().length > 0)
    )
      .subscribe(nameFood => {
        // בכל פעם שיש שינוי, נקרא לפונקציה שמבצעת את החיפוש
        this.searchFoodByName(nameFood);
      });
  }

  onFoodNameChange() {
    if (!this.nameFood || this.nameFood.trim() === '') {
      this.foods = [...this.defaultFoods];
      return;
    }
    // כל שינוי בהקלדה יישלח ל-Subject
    this.searchSubject.next(this.nameFood);
  }
  searchFoodByName(name: string) {
    this.foodService.searchFoodByName(name).subscribe(foods => {
      this.foods = foods;

      // עדכון המחשבון המהיר עם תוצאות החיפוש
      this.quickFoods = foods.slice(0, 4).map(f => ({
        emoji: '🍽️',
        name: f.name,
        cal: f.calories,
        fat: f.fat,
        carb: f.carbs,
        prot: f.protein
      }));

      if (this.quickFoods.length > 0) {
        this.selectedQuickFood = this.quickFoods[0];
        this.updateQuickCalc();
      }
    });
  }
  // מניעת דליפות זיכרון בביטול הרכיב
  ngOnDestroy() {
    if (this.searchSubscription) {
      this.searchSubscription.unsubscribe();
    }
  }
  addFoodToEat(food: Food) {
    this.selectFood = food;
  }
  saveNewFood(nutritionalInfo: any) {
    const newFood: Food = {
      id: 0, // השרת ייתן את ה-ID
      name: nutritionalInfo.name,
      description: nutritionalInfo.description,
      imageUrl: nutritionalInfo.imageUrl,
      calories: nutritionalInfo.calories,
      protein: nutritionalInfo.protein,
      carbs: nutritionalInfo.carbs,
      fat: nutritionalInfo.fat
    };
    this.foodService.addFood(newFood).subscribe({
      next: (foodId) => {
        console.log('New food added with ID:', foodId);
        // הוספת השורה הזו לעדכון המזהה
        newFood.id = foodId as number;
        this.isAddingNew = false; // סגירת טופס ה-AI
        this.addFoodToEat(newFood);
        this.cdr.detectChanges(); // 3. מוודא שאנגולר ירענן את המסך מיד
      },
      error: (err) => {
        console.error('Failed to add new food:', err);
        alert('Failed to add new food. Please try again later.');
      }
    });
  }
  saveFoodToEat() {
    if (!this.selectFood) return;
   const userMealToSave = {
    userId: Number(localStorage.getItem('currentUserId')),
    foodId: this.selectFood.id, 
    quantity: this.userMeal.quantity,
    mealType: this.userMeal.mealType,
    mealDate: this.userMeal.mealDate
  };
    this.mealService.addFoodToEat(userMealToSave).subscribe({
      next: (result) => {
        console.log('Food added to eat successfully:', result);
        // החליפי את ה-alert המקורי בשורות האלו:
        Swal.fire({
          title: 'המאכל נוסף בהצלחה! 🍽️',
          icon: 'success',
          toast: true,
          position: 'top-start', /* פינה שמאלית עליונה, מושלם למובייל ולדסקטופ */
          showConfirmButton: false,
          timer: 2000, /* נעלם מהר תוך 2 שניות כדי לא להפריע לזרימה */
          timerProgressBar: true
        });
        this.router.navigate(['/dailyTracker']);
        this.cancelSelection();
      },
      error: (err) => {
        console.error('Failed to add food to eat:', err);
        alert('Failed to add food to eat. Please try again later.');
      }
    });
  }
  cancelSelection() {
    this.userMeal.foodId = Number.NaN;
    this.userMeal.quantity = 100;
    this.userMeal.mealType = 'Breakfast';
    this.userMeal.mealDate = new Date().toISOString().split('T')[0];
    this.selectFood = null;
  }

  //קשור לעיצוב
  selectQuickFood(item: any) {
    this.selectedQuickFood = item;
    this.updateQuickCalc();
  }

  updateQuickCalc() {
    if (!this.selectedQuickFood) return;
    const f = this.quickFoodQty / 100;
    this.quickCalories = Math.round(this.selectedQuickFood.cal * f);
    this.quickFat = Math.round(this.selectedQuickFood.fat * f);
    this.quickCarbs = Math.round(this.selectedQuickFood.carb * f);
    this.quickProtein = Math.round(this.selectedQuickFood.prot * f);
  }
  handleError(event: any) {
    event.target.src = "smalLogo.png" // נתיב לתמונה חלופית קבועה
  }
  // פונקציה חדשה: מופעלת בעמידה עם העכבר על מאכל מהמערך הראשי
  previewFood(food: any) {
    if (!food) return;

    // 1. נתרגם את המאכל למבנה של המחשבון המהיר
    const mappedFood = {
      emoji: food.emoji || '🍽️',
      name: food.name,
      cal: food.calories !== undefined ? food.calories : food.cal,
      fat: food.fat,
      carb: food.carbs !== undefined ? food.carbs : food.carb,
      prot: food.protein !== undefined ? food.protein : food.prot
    };

    // 2. נבדוק אם המאכל הזה כבר נמצא למעלה (לפי השם שלו)
    const existingIndex = this.quickFoods.findIndex(item => item.name === mappedFood.name);

    if (existingIndex !== -1) {
      // אם הוא כבר קיים, נסיר אותו מהמיקום הישן שלו כדי שלא יופיע פעמיים
      this.quickFoods.splice(existingIndex, 1);
    }

    // 3. נדחף את המאכל החדש לתחילת המערך (יופיע ראשון מצד ימין/שמאל למעלה)
    this.quickFoods.unshift(mappedFood);

    // 4. מגבלה: נשמור שתמיד יהיו רק 6 פריטים למעלה כדי שהעיצוב לא יישבר
    if (this.quickFoods.length > 4) {
      this.quickFoods.pop(); // מסיר את האיבר האחרון והישן ביותר ברשימה
    }

    // 5. נסמן אותו כנבחר הנוכחי ונפעיל את החישוב של המספרים
    this.selectedQuickFood = mappedFood;
    this.updateQuickCalc();
  }
}



