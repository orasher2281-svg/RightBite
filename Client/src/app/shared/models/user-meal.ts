import { Food } from "./food";

export interface UserMeal {
    id?: number;
    userId?: number;
    foodId: number;
    quantity: number;
    mealType: string;
    mealDate: string;
    food?:Food
}
