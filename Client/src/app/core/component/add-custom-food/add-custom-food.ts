import { ChangeDetectorRef, Component, EventEmitter, Output } from '@angular/core';
import { FoodAnalysisService } from '../../services/food-analysis-service';
import { FormsModule } from '@angular/forms';
import { NutritionalInfo } from '../../../shared/models/nutritional-info';

@Component({
  selector: 'app-add-custom-food',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-custom-food.html',
  styleUrl: './add-custom-food.css',
})
export class AddCustomFood {
  @Output() onClose=new EventEmitter<boolean>();
  @Output() foodAdded=new EventEmitter<any>();
  // משתנה לאחסון תוצאות הניתוח
  calculatedNutrition: NutritionalInfo | null = null;
  // הוספת המשתנה בראש הקלאס
  isLoading: boolean = false;
  // נתוני הטופס
  name: string = '';
  description: string = '';
  selectedFile: File | null = null;
  constructor(private foodAnalysisService: FoodAnalysisService,private cdr: ChangeDetectorRef) {}
  onFileSelected(event: any){
    const file: File = event.target.files[0];
    this.selectedFile = file;
  }
  // שלב 1: שליחת הקובץ לניתוח
  saveNewFood(){
  this.isLoading = true;
  this.foodAnalysisService.analyzeMeal(this.name, this.description, this.selectedFile)
    .subscribe({
      next: (res: NutritionalInfo) => {
        // כאן האתחול מתבצע ברגע קבלת המידע
        this.calculatedNutrition = {
          name: res.name || this.name,
          description: res.description || this.description,
          imageUrl: res.imageUrl,
          calories: Number(res.calories) || 0,
          protein: Number(res.protein) || 0,
          carbs: Number(res.carbs) || 0,
          fat: Number(res.fat) || 0
        };
        this.isLoading = false;
          this.cdr.detectChanges(); // עדכון תצוגת הנתונים
        },
      error: (error) => {
        console.error('Error analyzing meal:', error);
        this.isLoading = false;
        this.cdr.detectChanges(); // עדכון תצוגת הנתונים
      }
    });
}
  // שלב 2: אישור סופי ושליחה לרכיב האב
  confirmAndSave() {
    if (this.calculatedNutrition) {
      // מוסיפים את שם המאכל והתיאור לנתונים הסופיים לפני השליחה
      this.calculatedNutrition.name = this.name;
      this.calculatedNutrition.description = this.description;
      
      this.foodAdded.emit(this.calculatedNutrition); // שולח את המידע לתוך הרכיב האב
       this.onClose.emit(false); // סוגר את הטופס


        // איפוס הנתונים אחרי השמירה
    this.name = '';
    this.description = '';
    this.calculatedNutrition = null;
  
    }
  }
}
