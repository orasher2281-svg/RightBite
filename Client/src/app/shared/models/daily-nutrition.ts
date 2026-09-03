export interface DailyNutrition {

    id: number;
    userId: number;
    date: Date;
    dailyCalories: number;
    targetProtein: number;
    targetCarbs: number;
    targetFat: number;
    currentCalories: number;
    currentProtein: number;
    currentCarbs: number;
    currentFat: number;
}
