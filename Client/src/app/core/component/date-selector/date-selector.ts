import { Component, EventEmitter, Output, output, signal } from '@angular/core';

@Component({
  selector: 'app-date-selector',
  imports: [],
  templateUrl: './date-selector.html',
  styleUrl: './date-selector.css',
})
export class DateSelector {
@Output() dateSelected = new EventEmitter<string>();
selectedDate = signal(new Date());
dates = signal(this.generateDates());
//מציג בסרגל את הימים שלפני ואחרי היום הנוכחי
generateDates() {
  const daysHebrew = ['א', 'ב', 'ג', 'ד', 'ה', 'ו', 'ש'];
  const arr = [];
  
  for (let i = -3; i <= 3; i++) {
    const d = new Date();
    d.setDate(d.getDate() + i);
    
    arr.push({
      date: d, // האובייקט המלא (לוגיקה)
      dayLabel: daysHebrew[d.getDay()], // אות (א, ב...)
      dayNumber: d.getDate() // המספר (1-31)
    });
  }
  return arr;
}
isSameDay(d: Date) {
    return d.toDateString() === this.selectedDate().toDateString();
  }

 selectDate(d: Date) {
    this.selectedDate.set(d);
    this.dateSelected.emit(d.toISOString().split('T')[0]);
  }
}
