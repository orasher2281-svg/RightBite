import { UserGender } from "./user-gender";
import { UserGoal } from "./user-goal";

export interface User {
    id?: number;
    name: string;
    email: string;
    password: string;
    weight: number;
    height: number;
    age: number;
    gender:UserGender;
    goal:UserGoal;
     dailyCalories?: number;
    targetProtein?: number;
    targetCarbs?: number;
    targetFat?: number;
}
